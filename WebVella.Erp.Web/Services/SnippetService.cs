using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Services
{
	internal static class SnippetService
	{
		public static Dictionary<string, Snippet> Snippets { get; set; } = new Dictionary<string, Snippet>();

		static SnippetService()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
							.Where(a => !(a.FullName.ToLowerInvariant().StartsWith("microsoft.")
								|| a.FullName.ToLowerInvariant().StartsWith("system.")));

			foreach (var assembly in assemblies)
			{
				try
				{
					var resources = assembly.GetManifestResourceNames().Where(x => x.Contains(".Snippets.")).ToList();
					foreach (var resource in resources)
					{
						if(!Snippets.ContainsKey(resource))
							Snippets.Add(resource, new Snippet { Name = resource, Assembly = assembly });
					}
				}
				catch (NotSupportedException)
				{
					continue;
				}
			}
		}

		public static Snippet GetSnippet(string name)
		{
			if (!Snippets.ContainsKey(name))
				return null;

			return Snippets[name];
		}
	}
}
