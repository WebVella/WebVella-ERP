using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.DotNet.ProjectModel;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace WebVella.ERP.Plugins
{
	public static class MvcPluginExtensions
	{
		public static IMvcBuilder AddCrmPlugins(this IMvcBuilder builder, IHostingEnvironment hostingEnvironment)
		{
			var exports = PluginDevelopmentHelper.GetProjectExports(hostingEnvironment);

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

				var assemblies = GetAssembliesInFolder(exports, binDir);
				foreach (var assembly in assemblies)
					builder.PartManager.ApplicationParts.Add(new AssemblyPart(assembly));
			}
			return builder;
		}

		private static IEnumerable<Assembly> GetAssembliesInFolder(IEnumerable<Microsoft.DotNet.ProjectModel.Compilation.LibraryExport> exports, DirectoryInfo binPath)
		{
			List<Assembly> assemblies = new List<Assembly>();
			foreach (var fileSystemInfo in binPath.GetFileSystemInfos("*.dll"))
			{
				var assemblyName = AssemblyName.GetAssemblyName(fileSystemInfo.FullName);

				//first try to load assembly from refered libraries instead of plugin 'binaries' folder
				Assembly assembly = null;
				foreach (var export in exports)
				{
					var found = export.CompilationAssemblies.Any(x => x.Name == assemblyName.Name);
					if (found)
					{
						assembly = Assembly.Load(assemblyName);
						break;
					}
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
