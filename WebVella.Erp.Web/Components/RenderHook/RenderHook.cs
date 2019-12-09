using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Components
{

	public class RenderHook : ViewComponent
	{
		private static Dictionary<string, List<string>> componentsDict = null;

		/// <summary>
		/// Method that renders the components attached to a specific render placeholder by its key
		/// </summary>
		/// <param name="placeholder">name of the placeholder</param>
		/// <param name="model">varies based on the current placeholder logic</param>
		/// <param name="pageModel">current page model</param>
		/// <returns></returns>
		public async Task<IViewComponentResult> InvokeAsync(string placeholder, BaseErpPageModel pageModel, dynamic model = null)
		{
			if(pageModel == null)
				return await Task.FromResult<IViewComponentResult>(Content(""));
			if (componentsDict == null)
				componentsDict = Init();

			ViewBag.PageModel = pageModel;
			ViewBag.ComponentNames = null;
			if (componentsDict.ContainsKey(placeholder))
				ViewBag.ComponentNames = componentsDict[placeholder];

			if (componentsDict.ContainsKey(placeholder) && componentsDict[placeholder].Count > 0) {
				pageModel.HttpContext.Items[placeholder + "-render-hook-has-components"] = true;
			}

			return await Task.FromResult<IViewComponentResult>(View("RenderHook"));
		}

		private Dictionary<string, List<string>> Init()
		{
			Dictionary<string, List<dynamic>> processDict = new Dictionary<string, List<dynamic>>();

			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
						.Where(a => !(a.FullName.ToLowerInvariant().StartsWith("microsoft.")
							|| a.FullName.ToLowerInvariant().StartsWith("system.")));

			foreach (var assembly in assemblies)
			{
				foreach (Type type in assembly.GetTypes())
				{

					if (!type.IsSubclassOf(typeof(ViewComponent)))
						continue;

					var attr = (RenderHookAttachmentAttribute)type.GetCustomAttributes(typeof(RenderHookAttachmentAttribute), true).FirstOrDefault();
					if (attr == null)
						continue;

					if (!processDict.ContainsKey(attr.PlaceholderName))
						processDict.Add(attr.PlaceholderName, new List<dynamic>());

					dynamic record = new ExpandoObject();
					record.Name = type.FullName;
					record.Priority = attr.Priority;
					processDict[attr.PlaceholderName].Add(record);
				}
			}

			//process order
			Dictionary<string, List<string>> resultDict = new Dictionary<string, List<string>>();
			foreach (var key in processDict.Keys)
			{
				var prioritizedCompNames = processDict[key].OrderBy(x => x.Priority).Select(x => (string)x.Name).ToList();
				resultDict.Add(key, prioritizedCompNames);

			}

			return resultDict;
		}

	}
}
