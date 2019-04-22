using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.Components
{

	public class NavViewComponent : ViewComponent
    {
		protected ErpRequestContext ErpRequestContext { get; set; }

		public NavViewComponent([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public async Task<IViewComponentResult> InvokeAsync( BaseErpPageModel pageModel)
        {
			var appContext = ErpAppContext.Current;
			var currentApp = ErpRequestContext.App;
			ViewBag.CurrentApp = currentApp;
            ViewBag.CurrentArea = ErpRequestContext.SitemapArea;
            ViewBag.CurrentNode = ErpRequestContext.SitemapNode;
            var theme = appContext.Theme;
            ViewBag.Theme = theme;
            ViewBag.BrandLogo = theme.BrandLogo;
            if (!String.IsNullOrWhiteSpace(ErpSettings.NavLogoUrl)) {
                ViewBag.BrandLogo = ErpSettings.NavLogoUrl;
            }

			ViewBag.AppShortName = "";
			ViewBag.AppDefaultLink = "/";
			if (currentApp != null) {
				ViewBag.AppShortName = currentApp.Name.Replace("_", "");
				if (currentApp.HomePages.Count > 0)
				{
					var homePages = currentApp.HomePages.OrderBy(x => x.Weight).ToList();
					ViewBag.AppDefaultLink = $"/{currentApp.Name}/a/{homePages[0].Name}";
				}
				else if(currentApp.Sitemap.Areas.Count > 0){
					var currentAreas = currentApp.Sitemap.Areas.OrderBy(x => x.Weight).ToList();
					foreach (var area in currentAreas)
					{
						if (area.Nodes.Count > 0) {
							var currentNode = area.Nodes[0];
							switch (currentNode.Type) {
								case SitemapNodeType.ApplicationPage:
									ViewBag.AppDefaultLink = $"/{currentApp.Name}/{area.Name}/{currentNode.Name}/a/";
									break;
								case SitemapNodeType.EntityList:
									ViewBag.AppDefaultLink = $"/{currentApp.Name}/{area.Name}/{currentNode.Name}/l/";
									break;
								case SitemapNodeType.Url:
									ViewBag.AppDefaultLink = currentNode.Url;
									break;
								default:
									throw new Exception("Type not found");
							}
							break;
						}
					}
				}
			}


			ViewBag.EmbedJs = "";
			#region << Generate js script >>
				var jsCompressor = new JavaScriptCompressor();
				#region << Init Scripts >>

				var fileName = "script.js";
				var scriptEl = "<script type=\"text/javascript\">";
				var scriptTemplate = FileService.GetEmbeddedTextResource(fileName, "WebVella.Erp.Web.Components.Nav");

				scriptEl += jsCompressor.Compress(scriptTemplate);

				scriptEl += "</script>";

				ViewBag.EmbededJs = scriptEl;
				#endregion
			#endregion

			ViewBag.PageModel = pageModel;

			ViewBag.DefaultAppName = String.IsNullOrWhiteSpace(ErpSettings.AppName) ? "WebVella" : ErpSettings.AppName;

			return await Task.FromResult<IViewComponentResult>(View("Nav.Default"));
        }
    }
}
