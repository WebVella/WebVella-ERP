using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using WebVella.ERP.Plugins;

namespace WebVella.ERP.Web.Controllers
{
    public class HomeController : ApiControllerBase
    {
		IPluginService pluginService = null;

		public HomeController(IPluginService pluginService)
		{
			this.pluginService = pluginService;
		}

		[AllowAnonymous]
        // GET: /<controller>/
        public IActionResult Index()
        {
			//CSS files
			var libraryCssFileList = new List<string>();
			var moduleCssFileList = new List<string>();
			//JS files
			var libraryJsFileList = new List<string>();
			var moduleJsFileList = new List<string>();
			//App dependency injections
			var appDependencyInjections = new List<string>();

			#region << Get and init plugin data >>
			foreach(var manifest in pluginService.Plugins) {

				var cacheBreaker = manifest.Version.ToString();

				foreach(var cssFile in manifest.Libraries.Css) {
					libraryCssFileList.Add(AppendCacheBreaker(cssFile, cacheBreaker));
				}
				foreach(var cssFile in manifest.Module.Css) {
					moduleCssFileList.Add(AppendCacheBreaker(cssFile, cacheBreaker));
				}
				foreach(var jsFile in manifest.Libraries.Js) {
					libraryJsFileList.Add(AppendCacheBreaker(jsFile, cacheBreaker));
				}
				foreach(var jsFile in manifest.Module.Js) {
					moduleJsFileList.Add(AppendCacheBreaker(jsFile, cacheBreaker));
				}
				foreach(var appInject in manifest.Module.WVAppInjects) {
					appDependencyInjections.Add(appInject);
				}
			}
			#endregion


			ViewBag.CacheBreaker = 20150920;
			ViewBag.Lang = Settings.Lang;
			ViewBag.CompanyName = Settings.CompanyName;
			ViewBag.CompanyLogo = Settings.CompanyLogo;
			ViewBag.LibraryCssFileList = libraryCssFileList;
			ViewBag.ModuleCssFileList = moduleCssFileList;
			ViewBag.LibraryJsFileList = libraryJsFileList;
			ViewBag.ModuleJsFileList = moduleJsFileList;
			ViewBag.AppDependencyInjections = appDependencyInjections;

            return View();
        }

		private string AppendCacheBreaker(string url, string cacheBreaker)
		{
			if( url.StartsWith("/") )
			{
				if( !url.Contains("?"))
					return $"{url}?{cacheBreaker}";

				return $"{url}&{cacheBreaker}";
			}
			return url;
		}
    }
}
