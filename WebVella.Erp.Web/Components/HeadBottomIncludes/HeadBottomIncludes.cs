using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Web.Components
{

	[RenderHookAttachment("head-bottom", 10)]
	public class HeadBottomIncludes : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(BaseErpPageModel pageModel)
		{
			ViewBag.ScriptTags = new List<ScriptTagInclude>();
			ViewBag.LinkTags = new List<LinkTagInclude>();

			var cacheKey = new RenderService().GetCacheKey();

			#region === <link> ===
			{
				var includedLinkTags = pageModel.HttpContext.Items.ContainsKey(typeof(List<LinkTagInclude>)) ? (List<LinkTagInclude>)pageModel.HttpContext.Items[typeof(List<LinkTagInclude>)] : new List<LinkTagInclude>();
				var linkTagsToInclude = new List<LinkTagInclude>();

				//Your includes below >>>>

				#region << core plugin >>
				{
					//Always include
					if(pageModel != null && pageModel.ErpAppContext != null && !String.IsNullOrEmpty(pageModel.ErpAppContext.StylesHash))
					{
						linkTagsToInclude.Add(new LinkTagInclude()
						{
							Href = "/api/v3.0/p/core/styles.css?cb=" + cacheKey,
							CacheBreaker = pageModel.ErpAppContext.StylesHash,
							//CrossOrigin = CrossOriginType.Anonymous,
							//Integrity = $"sha256-{pageModel.ErpAppContext.StylesHash}"
						});
					}
					else{
						linkTagsToInclude.Add(new LinkTagInclude()
						{
							Href = "/api/v3.0/p/core/styles.css?cb=" + cacheKey
						});	
					}
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

				#region << site.js >>
				{
					//Always include
					scriptTagsToInclude.Add(new ScriptTagInclude()
					{
						Src = "/_content/WebVella.Erp.Web/js/site.js?cb=" + cacheKey
					});
				}
				#endregion

				#region << js-cookie >>
				{
					if (!includedScriptTags.Any(x => x.Src.Contains("/js-cookie")))
					{
						scriptTagsToInclude.Add(new ScriptTagInclude()
						{
							Src = "/_content/WebVella.Erp.Web/lib/js-cookie/js.cookie.min.js?cb=" + cacheKey
						});
					}
				}
				#endregion


				//var stencilComponents = new List<string>(){"wv-lazyload", "wv-timelog-list", "wv-pb-manager", 
				//	"wv-sitemap-manager", "wv-datasource-manage","wv-post-list", "wv-feed-list", "wv-recurrence-template"};

				var stencilComponents = new List<string>(){"wv-lazyload"};

				foreach (var componentName in stencilComponents)
				{
					scriptTagsToInclude.Add(new ScriptTagInclude()
					{
						Src = $"/_content/WebVella.Erp.Web/js/{componentName}/{componentName}.esm.js",
						Type = "module"
					});

					scriptTagsToInclude.Add(new ScriptTagInclude()
					{
						Src = $"/_content/WebVella.Erp.Web/js/{componentName}/{componentName}.js",
						IsNomodule = true
					});
				}



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
