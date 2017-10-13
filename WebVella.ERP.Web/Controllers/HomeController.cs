using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using WebVella.ERP.Plugins;
using WebVella.ERP.Web.Services;

namespace WebVella.ERP.Web.Controllers
{
    public class HomeController : ApiControllerBase
    {
		IPluginService pluginService = null;

		public HomeController(IPluginService pluginService)
		{
			this.pluginService = pluginService;
		}

		[AcceptVerbs(new[] { "GET" }, Route = "ckeditor/image-finder")]
        public IActionResult ImageFinder()
        {
			#region << Framework setup >>
			//CSS files
			var libraryCssFileList = new List<string>();
			var moduleCssFileList = new List<string>();
			//JS files
			var libraryJsFileList = new List<string>();
			var moduleJsFileList = new List<string>();
			//App dependency injections
			var appDependencyInjections = new List<string>();
			ViewBag.CacheBreaker = 20170828;

			#region << Get and init plugin data >>
			foreach(var manifest in pluginService.Plugins) {

				var cacheBreaker = manifest.Version.ToString();
				if(manifest.Name == "webvella-core") {
					ViewBag.CacheBreaker = cacheBreaker;
				}
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

			ViewBag.Lang = Settings.Lang;
			ViewBag.CompanyName = Settings.CompanyName;
			ViewBag.CompanyLogo = Settings.CompanyLogo;
			ViewBag.DevelopmentMode = Settings.DevelopmentMode;
			ViewBag.LibraryCssFileList = libraryCssFileList;
			ViewBag.ModuleCssFileList = moduleCssFileList;
			ViewBag.LibraryJsFileList = libraryJsFileList;
			ViewBag.ModuleJsFileList = moduleJsFileList;
			ViewBag.AppDependencyInjections = appDependencyInjections;
			#endregion

			var currentType = "image";
			var currentSort = 1;
			var currentPage = 1;
			var currentPageSize = 30;
			
			ViewBag.FinderType = currentType;
			ViewBag.CurrentSort = currentSort;
			ViewBag.CurrentPage = currentPage;
			ViewBag.CurrentPageSize = currentPageSize;
			var files = new UserFileService().GetFilesList(currentType,"",currentSort,currentPage,currentPageSize);
			ViewBag.JsonFiles = JsonConvert.SerializeObject(files); 


			return View();
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
			ViewBag.CacheBreaker = 20170405;

			#region << Get and init plugin data >>
			foreach(var manifest in pluginService.Plugins) {

				var cacheBreaker = manifest.Version.ToString();
				if(manifest.Name == "webvella-core") {
					ViewBag.CacheBreaker = cacheBreaker;
				}
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



			ViewBag.Lang = Settings.Lang;
			ViewBag.CompanyName = Settings.CompanyName;
			ViewBag.CompanyLogo = Settings.CompanyLogo;
			ViewBag.DevelopmentMode = Settings.DevelopmentMode;
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
					return $"{url}?v={cacheBreaker}";

				return $"{url}&v={cacheBreaker}";
			}
			return url;
		}
    }
}
