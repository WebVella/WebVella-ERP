using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Services
{
	public class PageComponentLibraryService
	{
		//components meta can be made static because there is no change after application is started
		private static List<PageComponentMeta> componentsMetaList = null;

		public List<PageComponentMeta> GetPageComponentsList()
		{
			if (componentsMetaList != null)
				return componentsMetaList;

			List<PageComponentMeta> result = new List<PageComponentMeta>();
			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
					.Where(a => !(a.FullName.ToLowerInvariant().StartsWith("microsoft.")
						|| a.FullName.ToLowerInvariant().StartsWith("system.")));

			foreach (var assembly in assemblies)
			{
				foreach (Type type in assembly.GetTypes())
				{

					if (!type.IsSubclassOf(typeof(PageComponent)))
						continue;

					var attr = (PageComponentAttribute)type.GetCustomAttributes(typeof(PageComponentAttribute), true).FirstOrDefault();
					if (attr == null)
						continue;

					string name = $"{type.Namespace}.{type.Name}";

					bool serviceJsExist = FileService.EmbeddedResourceExists("service.js", name, assembly);
					//bool designHtmlExist = FileService.EmbeddedResourceExists("design.html", name, assembly);
					//bool optionsHtmlExist = FileService.EmbeddedResourceExists("options.html", name, assembly);

					PageComponentMeta meta = new PageComponentMeta
					{
						Name = name,
						Label = attr.Label,
						Description = attr.Description,
						Library = attr.Library,
						IsInline = attr.IsInline,
						Category = attr.Category,
						Color = attr.Color,
						IconClass = attr.IconClass,
						Version = attr.Version,
						ServiceJsUrl = serviceJsExist ? $"/api/v3.0/pc/{name}/resource/service.js?v={attr.Version}" : null,
						DesignViewUrl = $"/api/v3.0/pc/{name}/view/design?v={attr.Version}",
						OptionsViewUrl = $"/api/v3.0/pc/{name}/view/options?v={attr.Version}",
						HelpViewUrl = $"/api/v3.0/pc/{name}/view/help?v={attr.Version}",
						LastUsedOn = DateTime.MinValue,
						UsageCounter = 0
					};
					result.Add(meta);
				}
			}
			
			componentsMetaList = result;
			return result;
		}

		public PageComponentMeta GetComponentMeta(string componentName)
		{
			var library = GetPageComponentsList();
			return library.FirstOrDefault(x => x.Name.ToLowerInvariant() == componentName.ToLowerInvariant());
		}
	}
}
