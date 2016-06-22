using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WebVella.ERP.Utilities.Dynamic;
using Microsoft.DotNet.ProjectModel;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace WebVella.ERP.Plugins
{
	public class PluginService : IPluginService
	{
		static List<Plugin> plugins = new List<Plugin>();
		public List<Plugin> Plugins { get { return plugins; } }

		public void Initialize(IServiceProvider serviceProvider)
		{
			IHostingEnvironment hostingEnvironment = (IHostingEnvironment)serviceProvider.GetService(typeof(IHostingEnvironment));
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
			ExecutePluginStart(serviceProvider);
		}

		private void ExecutePluginStart(IServiceProvider serviceProvider)
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
									{
										PluginStartArguments args = new PluginStartArguments { Plugin = plugin, ServiceProvider= serviceProvider };
										method.Invoke(new DynamicObjectCreater(type).CreateInstance(), new object[] { args });
									}
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
			foreach (var fileSystemInfo in binPath.GetFileSystemInfos("*.dll"))
			{
				//assemblies are already loaded from correct destination (depending on referencing) 
				//during the process of plugins registration as application parts
				var assemblyName = AssemblyName.GetAssemblyName(fileSystemInfo.FullName);
				assemblies.Add(Assembly.Load(assemblyName));
			}
			return assemblies;
		}
	}
}
