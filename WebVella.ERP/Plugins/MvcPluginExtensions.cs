using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System;

namespace WebVella.ERP.Plugins
{
	public static class MvcPluginExtensions
	{
		public static IMvcBuilder AddCrmPlugins(this IMvcBuilder builder, IHostingEnvironment hostingEnvironment)
		{
			var loadedAsms = AppDomain.CurrentDomain.GetAssemblies();

			var content = hostingEnvironment.WebRootFileProvider.GetDirectoryContents("/plugins");
			if (!content.Exists)
				return builder;

			foreach (var pluginDir in content.Where(x => x.IsDirectory))
			{
				var manifestFilePath = Path.Combine(pluginDir.PhysicalPath, "manifest.json");
				var manifestFile = new FileInfo(manifestFilePath);
				if (!manifestFile.Exists)
					continue;

				var binDir = new DirectoryInfo(Path.Combine(pluginDir.PhysicalPath, "binaries"));
				if (!binDir.Exists)
					continue;

				var assemblies = GetAssembliesInFolder(loadedAsms, binDir);
				foreach (var assembly in assemblies)
					builder.PartManager.ApplicationParts.Add(new AssemblyPart(assembly));
			}
			return builder;
		}

		private static IEnumerable<Assembly> GetAssembliesInFolder(Assembly[] loadedAsms, DirectoryInfo binPath)
		{
			List<Assembly> assemblies = new List<Assembly>();
			foreach (var fileSystemInfo in binPath.GetFileSystemInfos("*.dll"))
			{
				var assemblyName = AssemblyName.GetAssemblyName(fileSystemInfo.FullName);

				//first try to load assembly from refered libraries instead of plugin 'binaries' folder
				Assembly assembly = null;
				if (loadedAsms.Any(x => x.FullName == assemblyName.FullName))
				{
					assembly = Assembly.Load(assemblyName);
					break;
				}

				//if not found in referenced libraries, load from plugin binaries location
				if (assembly == null)
				{
					assembly = Assembly.LoadFile(fileSystemInfo.FullName);
					assemblies.Add(assembly);
				}
			}

			return assemblies;
		}
	}
}
