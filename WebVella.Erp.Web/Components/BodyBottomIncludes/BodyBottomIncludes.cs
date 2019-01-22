using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Components
{

	[RenderHookAttachment("body-bottom", 10)]
	public class BodyBottomIncludes : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(BaseErpPageModel pageModel)
        {
			ViewBag.ScriptTags = new List<ScriptTagInclude>();

			#region === <script> ===
			{
				var includedScriptTags = pageModel.HttpContext.Items.ContainsKey(typeof(List<ScriptTagInclude>)) ? (List<ScriptTagInclude>)pageModel.HttpContext.Items[typeof(List<ScriptTagInclude>)] : new List<ScriptTagInclude>();
				var scriptTagsToInclude = new List<ScriptTagInclude>();
				
				//Your includes below >>>>

				#region << globals >>
				{
					//Always include
					var globalScript = $"var SiteLang=\"{ErpSettings.Lang}\";";
					globalScript += $"moment.locale(\"{ErpSettings.Lang}\");";
					scriptTagsToInclude.Add(new ScriptTagInclude()
					{
						InlineContent = globalScript
					});
				}
				#endregion

				//<<<< Your includes up

				includedScriptTags.AddRange(scriptTagsToInclude);
				pageModel.HttpContext.Items[typeof(List<ScriptTagInclude>)] = includedScriptTags;
				ViewBag.ScriptTags = scriptTagsToInclude;
			}
			#endregion
			return await Task.FromResult<IViewComponentResult>(View("Default"));
        }
    }
}
