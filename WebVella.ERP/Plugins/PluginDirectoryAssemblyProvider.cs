using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;

namespace WebVella.ERP.Plugins
{
	public class PluginDirectoryAssemblyProvider : IAssemblyProvider
	{
		private readonly IHostingEnvironment hostEnvironment;
		private readonly IAssemblyLoadContextAccessor loadContextAccessor;
		private readonly IAssemblyLoaderContainer assemblyLoaderContainer;
		private static readonly object lockObj = new object();
		private static List<Assembly> assemblies;

		public PluginDirectoryAssemblyProvider(
				IHostingEnvironment hostEnvironment,
				IAssemblyLoadContextAccessor loadContextAccessor,
				IAssemblyLoaderContainer assemblyLoaderContainer)
		{
			this.hostEnvironment = hostEnvironment;
			this.loadContextAccessor = loadContextAccessor;
			this.assemblyLoaderContainer = assemblyLoaderContainer;
		}

		public IEnumerable<Assembly> CandidateAssemblies
		{
			get
			{
				lock (lockObj)
				{
					if (assemblies != null)
						return assemblies;

					assemblies = new List<Assembly>();

					var content = hostEnvironment.WebRootFileProvider.GetDirectoryContents("/plugins");
					if (!content.Exists)
						return assemblies;

					foreach (var pluginDir in content.Where(x => x.IsDirectory))
					{
						//skip folders with no manifest file
						var manifestFilePath = Path.Combine(pluginDir.PhysicalPath, "manifest.json");
						var manifestFile = new FileInfo(manifestFilePath);
						if (!manifestFile.Exists)
							continue;

						var binDir = new DirectoryInfo(Path.Combine(pluginDir.PhysicalPath, "binaries"));
						if (!binDir.Exists)
							continue;

						foreach (var assembly in GetAssembliesInFolder(binDir))
							assemblies.Add(assembly);
					}

					return assemblies;
				}
			}
		}

		private IEnumerable<Assembly> GetAssembliesInFolder(DirectoryInfo binPath)
		{
			List<Assembly> assemblies = new List<Assembly>();
			var loadContext = loadContextAccessor.Default;
			assemblyLoaderContainer.AddLoader(new PluginDirectoryLoader(binPath, loadContext));

			foreach (var fileSystemInfo in binPath.GetFileSystemInfos("*.dll"))
				assemblies.Add(loadContext.Load(AssemblyName.GetAssemblyName(fileSystemInfo.FullName)));

			return assemblies;
		}
	}
}
