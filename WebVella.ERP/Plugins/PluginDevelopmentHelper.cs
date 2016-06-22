using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.DotNet.ProjectModel;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;

namespace WebVella.ERP.Plugins
{
	public static class PluginDevelopmentHelper
	{
		internal static IEnumerable<Microsoft.DotNet.ProjectModel.Compilation.LibraryExport> GetProjectExports(IHostingEnvironment hostingEnvironment)
		{
			IEnumerable<Microsoft.DotNet.ProjectModel.Compilation.LibraryExport> exports = new List<Microsoft.DotNet.ProjectModel.Compilation.LibraryExport>();
			var currentFolder = Directory.GetCurrentDirectory();
			var projectFile = new FileInfo(Path.Combine(currentFolder, "project.json"));
			if (projectFile.Exists)
			{
				var appInfo = new ApplicationInfo("WebVella Erp", currentFolder);
				var runtime = RuntimeEnvironmentExtensions.GetRuntimeIdentifier(PlatformServices.Default.Runtime);
				var projectContext = ProjectContext.CreateContextForEachFramework(Directory.GetCurrentDirectory(), null, new[] { runtime }).First();
				var libraryExporter = new LibraryExporter(projectContext, appInfo);
				exports = libraryExporter.GetAllExports();
			}
			return exports;
		}
	}
}
