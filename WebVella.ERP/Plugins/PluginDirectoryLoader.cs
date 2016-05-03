using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;

namespace WebVella.ERP.Plugins
{
	public class PluginDirectoryLoader : IAssemblyLoader
	{
		private readonly IAssemblyLoadContext context;
		private readonly DirectoryInfo path;

		public PluginDirectoryLoader(DirectoryInfo path, IAssemblyLoadContext context)
		{
			this.path = path;
			this.context = context;
		}

		public Assembly Load(AssemblyName assemblyName)
		{
			var assemblyPath = Path.Combine(path.FullName, assemblyName.Name + ".dll");
			if (!File.Exists(assemblyPath))
				return null;

			return context.LoadFile(assemblyPath);
		}

		public IntPtr LoadUnmanagedLibrary(string name)
		{
			//unmanaged asseblies won't be loaded
			throw new NotImplementedException();
		}
	}
}
