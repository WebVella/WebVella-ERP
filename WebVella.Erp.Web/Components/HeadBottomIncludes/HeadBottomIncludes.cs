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
					linkTagsToInclude.Add(new LinkTagInclude()
					{
						Href = "/api/v3.0/p/core/styles.css?cb=" + cacheKey,
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

				//#region << jquery >>
				//{
				//	if (!includedScriptTags.Any(x => x.Src.Contains("/jquery")))
				//	{
				//		scriptTagsToInclude.Add(new ScriptTagInclude()
				//		{
				//			Src = "/webvella-erp-web/lib/jquery/jquery.min.js?cb=" + cacheKey
				//		});
				//	}
				//}
				//#endregion

				#region << site.js >>
				{
					//Always include
					scriptTagsToInclude.Add(new ScriptTagInclude()
					{
						Src = "/webvella-erp-web/js/site.js?cb=" + cacheKey
					});
				}
				#endregion

				//#region << bootstrap >>
				//{
				//	if (!includedScriptTags.Any(x => x.Src.Contains("/bootstrap")))
				//	{
				//		scriptTagsToInclude.Add(new ScriptTagInclude()
				//		{
				//			Src = "/webvella-erp-web/lib/twitter-bootstrap/js/bootstrap.bundle.min.js?cb=" + cacheKey
				//		});
				//	}
				//}
				//#endregion

				//#region << uri.js >>
				//{
				//	if (!includedScriptTags.Any(x => x.Src.Contains("/uri")))
				//	{
				//		scriptTagsToInclude.Add(new ScriptTagInclude()
				//		{
				//			Src = "/webvella-erp-web/lib/URI.js/URI.min.js?cb=" + cacheKey
				//		});
				//	}
				//}
				//#endregion

				//#region << moment >>
				//{
				//	if (!includedScriptTags.Any(x => x.Src.Contains("/moment")))
				//	{
				//		scriptTagsToInclude.Add(new ScriptTagInclude()
				//		{
				//			Src = "/webvella-erp-web/lib/moment.js/moment.min.js?cb=" + cacheKey
				//		});
				//	}
				//}
				//#endregion

				//#region << ckeditor >>
				//{
				//	if (!includedScriptTags.Any(x => x.Src.Contains("/ckeditor")))
				//	{
				//		scriptTagsToInclude.Add(new ScriptTagInclude()
				//		{
				//			Src = "/webvella-erp-web/lib/ckeditor/ckeditor.js?cb=" + cacheKey
				//		});
				//	}
				//}
				//#endregion

				//#region << lodash >>
				//{
				//	if (!includedScriptTags.Any(x => x.Src.Contains("/lodash")))
				//	{
				//		scriptTagsToInclude.Add(new ScriptTagInclude()
				//		{
				//			Src = "/webvella-erp-web/lib/lodash.js/lodash.min.js?cb=" + cacheKey
				//		});
				//	}
				//}
				//#endregion

				//#region << flatpickr >>
				//{
				//	if (!includedScriptTags.Any(x => x.Src.Contains("/flatpickr")))
				//	{
				//		scriptTagsToInclude.Add(new ScriptTagInclude()
				//		{
				//			Src = "/webvella-erp-web/lib/flatpickr/flatpickr.min.js?cb=" + cacheKey
				//		});
				//	}
				//}
				//#endregion

				//#region << select2 >>
				//{
				//	if (!includedScriptTags.Any(x => x.Src.Contains("/select2")))
				//	{
				//		scriptTagsToInclude.Add(new ScriptTagInclude()
				//		{
				//			Src = "/webvella-erp-web/lib/select2/js/select2.min.js?cb=" + cacheKey
				//		});
				//	}
				//}
				//#endregion

				#region << js-cookie >>
				{
					if (!includedScriptTags.Any(x => x.Src.Contains("/js-cookie")))
					{
						scriptTagsToInclude.Add(new ScriptTagInclude()
						{
							Src = "/webvella-erp-web/lib/js-cookie/js.cookie.min.js?cb=" + cacheKey
						});
					}
				}
				#endregion

				//#region << decimal >>
				//{
				//	if (!includedScriptTags.Any(x => x.Src.Contains("/decimal")))
				//	{
				//		scriptTagsToInclude.Add(new ScriptTagInclude()
				//		{
				//			Src = "/webvella-erp-web/lib/decimal.js/decimal.min.js?cb=" + cacheKey
				//		});
				//	}
				//}
				//#endregion

				//#region << toastr >>
				//{
				//	if (!includedScriptTags.Any(x => x.Src.Contains("/toastr")))
				//	{
				//		scriptTagsToInclude.Add(new ScriptTagInclude()
				//		{
				//			Src = "/webvella-erp-web/lib/toastr.js/toastr.min.js?cb=" + cacheKey
				//		});
				//	}
				//}
				//#endregion

				//#region << colorpicker >>
				//{
				//	if (!includedScriptTags.Any(x => x.Src.Contains("/colorpicker")))
				//	{
				//		scriptTagsToInclude.Add(new ScriptTagInclude()
				//		{
				//			Src = "/webvella-erp-web/lib/spectrum/spectrum.min.js?cb=" + cacheKey
				//		});
				//	}
				//}
				//#endregion


				#region << wv-lazyload >>
				{
					////Always add
					//scriptTagsToInclude.Add(new ScriptTagInclude()
					//{
					//	Src = "/js/stencil/wv-lazyload.esm.js",
					//	Type = "module"
					//});
					//scriptTagsToInclude.Add(new ScriptTagInclude()
					//{
					//	Src = "/js/stencil/wv-lazyload.js",
					//	IsNomodule = true
					//});

					//scriptTagsToInclude.Add(new ScriptTagInclude()
					//{
					//	Src = "/js/stencil/wv-lazyload.js"
					//});

				}
				#endregion


				var stencilComponents = new List<string>(){"wv-lazyload", "wv-timelog-list", "wv-pb-manager", 
					"wv-sitemap-manager", "wv-datasource-manage","wv-post-list", "wv-feed-list", "wv-recurrence-template"};


				foreach (var componentName in stencilComponents)
				{
					scriptTagsToInclude.Add(new ScriptTagInclude()
					{
						Src = $"/webvella-erp-web/js/{componentName}/{componentName}.esm.js",
						Type = "module"
					});

					scriptTagsToInclude.Add(new ScriptTagInclude()
					{
						Src = $"/webvella-erp-web/js/{componentName}/{componentName}.js",
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
