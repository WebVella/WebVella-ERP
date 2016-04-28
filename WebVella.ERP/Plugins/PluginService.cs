using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WebVella.ERP.Plugins
{
	public class PluginService : IPluginService
	{
		static List<Plugin> plugins = new List<Plugin>();
		public List<Plugin> Plugins { get { return plugins; } }



		public void Initialize(IHostingEnvironment hostingEnvironment, IAssemblyProvider asmProviderService)
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
					throw new Exception("An exception is thrown while parsing plugin manifest file: '" + manifestFilePath +"'", ex);
				}
				plugin.Assemblies = new List<Assembly>();


				var binDir = new DirectoryInfo(Path.Combine(pluginDir.PhysicalPath, "binaries"));
				if (!binDir.Exists)
					continue;

				plugin.Assemblies.AddRange(GetAssembliesInFolder(binDir));

				//we are working with assembly names (not the assemply instances),
				//because in case of development mode, referenced assembly is loaded 
				//we don't want to call 2 or more times start for assembly with same name
				List<string> processedPlugins = new List<string>();
				foreach (var assembly in asmProviderService.CandidateAssemblies)
				{
					if (processedPlugins.Contains(assembly.FullName))
						continue;

					if (plugin.Assemblies.Any(x => x.FullName == assembly.FullName))
					{
						processedPlugins.Add(assembly.FullName);
						foreach (Type type in assembly.GetTypes())
						{
							if (type.GetCustomAttributes(typeof(PluginStartupAttribute), true).Length > 0)
							{
								try {
									var obj = Activator.CreateInstance(assembly.FullName, type.Namespace + "." + type.Name).Unwrap();
									var method = type.GetMethod("Start");
									if (method != null)
										method.Invoke(obj, null);
								}
								catch( Exception ex )
								{
									throw new Exception("An exception is thrown while execute start for plugin : '" +
									 assembly.FullName + ";" + type.Namespace + "." + type.Name + "'", ex);
								}
							}
						}
					}
				}
			}
			plugins = plugins.OrderBy(x => x.LoadPriority).ToList();
		}

		private IEnumerable<Assembly> GetAssembliesInFolder(DirectoryInfo binPath)
		{
			List<Assembly> assemblies = new List<Assembly>();
			var loadContext = PlatformServices.Default.AssemblyLoadContextAccessor.Default;
			PlatformServices.Default.AssemblyLoaderContainer.AddLoader(new PluginDirectoryLoader(binPath, loadContext));

			foreach (var fileSystemInfo in binPath.GetFileSystemInfos("*.dll"))
			{
				var assembly = loadContext.Load(AssemblyName.GetAssemblyName(fileSystemInfo.FullName));
				assemblies.Add(assembly);
			}

			return assemblies;
		}
	}
}
