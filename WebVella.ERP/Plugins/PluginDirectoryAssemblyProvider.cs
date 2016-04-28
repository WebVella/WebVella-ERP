using Microsoft.AspNet.FileProviders;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;
using System;

namespace WebVella.ERP.Plugins
{
	public class PluginDirectoryAssemblyProvider : IAssemblyProvider
	{
		private readonly IHostingEnvironment hostEnvironment;
		private readonly IAssemblyLoadContextAccessor loadContextAccessor;
		private readonly IAssemblyLoaderContainer assemblyLoaderContainer;

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
				List<Assembly> assemblies = new List<Assembly>();
				var content = hostEnvironment.WebRootFileProvider.GetDirectoryContents("/plugins");
				if (!content.Exists)
					return assemblies;

				foreach (var pluginDir in content.Where(x => x.IsDirectory))
				{
					var binDir = new DirectoryInfo(Path.Combine(pluginDir.PhysicalPath, "binaries"));
					if (!binDir.Exists)
						continue;

					foreach (var assembly in GetAssembliesInFolder(binDir))
						assemblies.Add(assembly);
				}

				return assemblies;
			}
		}

		private IEnumerable<Assembly> GetAssembliesInFolder(DirectoryInfo binPath)
		{
			List<Assembly> assemblies = new List<Assembly>();
			var loadContext = loadContextAccessor.Default;
			assemblyLoaderContainer.AddLoader(new PluginDirectoryLoader(binPath, loadContext));

			foreach (var fileSystemInfo in binPath.GetFileSystemInfos("*.dll"))
			{
				var assembly = loadContext.Load(AssemblyName.GetAssemblyName(fileSystemInfo.FullName));
				assemblies.Add(assembly);
			}

			return assemblies;
		}
	}
}
