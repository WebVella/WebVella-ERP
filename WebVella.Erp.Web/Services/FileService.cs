using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WebVella.Erp.Web.Services
{
    public static class FileService 
	{
		public static string GetEmbeddedTextResource(string name, string nameSpace, string assemblyName = null)
		{
			string resourceName = $"{nameSpace}.{name}";
			Assembly assembly = null;
			if (!String.IsNullOrWhiteSpace(assemblyName))
			{
				assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == assemblyName);
			}
			else
			{
				assembly = Assembly.GetExecutingAssembly();
			}
			Stream resource = assembly.GetManifestResourceStream(resourceName);
			StreamReader reader = new StreamReader(resource);
			return reader.ReadToEnd();
		}

		public static string GetEmbeddedTextResource(string name, string nameSpace, Assembly assembly )
		{
			string resourceName = $"{nameSpace}.{name}";
			Stream resource = assembly.GetManifestResourceStream(resourceName);
			StreamReader reader = new StreamReader(resource);
			return reader.ReadToEnd();
		}

		public static bool EmbeddedResourceExists(string name, string nameSpace, string assemblyName = null)
		{
			string resourceName = $"{nameSpace}.{name}";
			Assembly assembly = null;
			if (!String.IsNullOrWhiteSpace(assemblyName))
			{
				assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == assemblyName);
			}
			else
			{
				assembly = Assembly.GetExecutingAssembly();
			}
			var resources = assembly.GetManifestResourceNames();
			return resources.Contains(resourceName);
		}

		public static bool EmbeddedResourceExists(string name, string nameSpace, Assembly assembly )
		{
			string resourceName = $"{nameSpace}.{name}";
			var resources = assembly.GetManifestResourceNames();
			return resources.Contains(resourceName);
		}

		public static Assembly GetTypeAssembly(string typeName )
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
						.Where(a => !(a.FullName.ToLowerInvariant().StartsWith("microsoft.")
							|| a.FullName.ToLowerInvariant().StartsWith("system.")));
			foreach (var assembly in assemblies)
			{
				foreach (Type t in assembly.GetTypes())
				{
					string name = $"{t.Namespace}.{t.Name}";
					if (name == typeName)
						return assembly;
				}
			}
			return null;
		}

		public static Type GetType(string typeName)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
						.Where(a => !(a.FullName.ToLowerInvariant().StartsWith("microsoft.")
							|| a.FullName.ToLowerInvariant().StartsWith("system.")));
			foreach (var assembly in assemblies)
			{
				foreach (Type t in assembly.GetTypes())
				{
					string name = $"{t.Namespace}.{t.Name}";
					if (name == typeName)
						return t;
				}
			}
			return null;
		}


	}
}
