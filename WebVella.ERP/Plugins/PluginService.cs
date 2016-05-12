using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WebVella.ERP.Utilities.Dynamic;

namespace WebVella.ERP.Plugins
{
	public class PluginService : IPluginService
	{
		static List<Plugin> plugins = new List<Plugin>();
		public List<Plugin> Plugins { get { return plugins; } }

		public void Initialize(IHostingEnvironment hostingEnvironment)
		{
			var content = hostingEnvironment.WebRootFileProvider.GetDirectoryContents("/plugins");
			if (!content.Exists)
				return;

			foreach (var pluginDir in content.Where(x => x.IsDirectory))
			{
				var manifestFilePath = Path.Combine(pluginDir.PhysicalPath, "manifest.json");
				var manifestFile = new FileInfo(manifestFilePath);
				if (!manifestFile.Exists)
					continue;

				var manifestJson = manifestFile.OpenText().ReadToEnd();
				Plugin plugin = null;
				try
				{
					plugin = JsonConvert.DeserializeObject<Plugin>(manifestJson);
					plugins.Add(plugin);
				}
				catch (Exception ex)
				{
					throw new Exception("An exception is thrown while parsing plugin manifest file: '" + manifestFilePath + "'", ex);
				}
				plugin.Assemblies = new List<Assembly>();


				var binDir = new DirectoryInfo(Path.Combine(pluginDir.PhysicalPath, "binaries"));
				if (!binDir.Exists)
					continue;

				plugin.Assemblies.AddRange(GetAssembliesInFolder(binDir));
			}

			plugins = plugins.OrderBy(x => x.LoadPriority).ToList();
			ExecutePluginStart();
		}

		private void ExecutePluginStart()
		{
			//search and execute Start method for each plugin
			//if there are multiple types, marked by PluginStartupAttribute, with Start method, they all will be executed
			foreach (var plugin in plugins)
			{
				foreach (var assembly in plugin.Assemblies)
				{
					if (plugin.Assemblies.Any(x => x.FullName == assembly.FullName))
					{
						foreach (Type type in assembly.GetTypes())
						{
							if (type.GetCustomAttributes(typeof(PluginStartupAttribute), true).Length > 0)
							{
								try
								{
									var method = type.GetMethod("Start");
									if (method != null)
										method.Invoke(new DynamicObjectCreater(type).CreateInstance(), null);
								}
								catch (Exception ex)
								{
									throw new Exception("An exception is thrown while execute start for plugin : '" +
									 assembly.FullName + ";" + type.Namespace + "." + type.Name + "'", ex);
								}
							}
						}
					}
				}
			}
		}

		private IEnumerable<Assembly> GetAssembliesInFolder(DirectoryInfo binPath)
		{
			List<Assembly> assemblies = new List<Assembly>();
			var loadContext = PlatformServices.Default.AssemblyLoadContextAccessor.Default;

			foreach (var fileSystemInfo in binPath.GetFileSystemInfos("*.dll"))
			{
				var assemblyName = AssemblyName.GetAssemblyName(fileSystemInfo.FullName);

				//first try to load assembly from refered libraries instead of plugin 'binaries' folder
				Assembly assembly = null;
				foreach (var lib in PlatformServices.Default.LibraryManager.GetLibraries())
				{
					var referencedAssemblyName = lib.Assemblies.SingleOrDefault(x => x.FullName == assemblyName.Name);
					if (referencedAssemblyName != null)
					{
						assembly = loadContext.Load(referencedAssemblyName);
						break;
					}
				}

				//if not found in referenced libraries, load from plugin binaries location
				if (assembly == null)
					assembly = loadContext.Load(assemblyName);

				assemblies.Add(assembly);
			}

			return assemblies;
		}
	}
}
