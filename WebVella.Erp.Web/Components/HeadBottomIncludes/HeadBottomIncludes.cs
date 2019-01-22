using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Components
{

	[RenderHookAttachment("head-bottom", 10)]
	public class HeadBottomIncludes : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(BaseErpPageModel pageModel)
        {
			ViewBag.ScriptTags = new List<ScriptTagInclude>();
			ViewBag.LinkTags = new List<LinkTagInclude>();

			#region === <link> ===
			{
				var includedLinkTags = pageModel.HttpContext.Items.ContainsKey(typeof(List<LinkTagInclude>)) ? (List<LinkTagInclude>)pageModel.HttpContext.Items[typeof(List<LinkTagInclude>)] : new List<LinkTagInclude>();
				var linkTagsToInclude = new List<LinkTagInclude>();

				//Your includes below >>>>

				#region << core plugin >>
				{
					//Always include
					linkTagsToInclude.Add(new LinkTagInclude()
					{
						Href = "/api/v3.0/p/core/styles.css",
						CacheBreaker = pageModel.ErpAppContext.StylesHash,
						CrossOrigin = CrossOriginType.Anonymous,
						Integrity = $"sha256-{pageModel.ErpAppContext.StylesHash}"
					});
				}
				#endregion

				//<<<< Your includes up

				includedLinkTags.AddRange(linkTagsToInclude);
				pageModel.HttpContext.Items[typeof(List<LinkTagInclude>)] = includedLinkTags;
				ViewBag.LinkTags = linkTagsToInclude;
			}
			#endregion

			#region === <script> ===
			{
				var includedScriptTags = pageModel.HttpContext.Items.ContainsKey(typeof(List<ScriptTagInclude>)) ? (List<ScriptTagInclude>)pageModel.HttpContext.Items[typeof(List<ScriptTagInclude>)] : new List<ScriptTagInclude>();
				var scriptTagsToInclude = new List<ScriptTagInclude>();

				//Your includes below >>>>

				#region << jquery >>
				{
					if (!includedScriptTags.Any(x => x.Src.Contains("/jquery")))
					{
						scriptTagsToInclude.Add(new ScriptTagInclude()
						{
							Src = "/lib/jquery/jquery.min.js"
						});
					}
				}
				#endregion

				#region << site.js >>
				{
					//Always include
					scriptTagsToInclude.Add(new ScriptTagInclude()
					{
						Src = "/js/site.js"
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
