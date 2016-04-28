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
			var pluginManifests = pluginService.Plugins;
			foreach(var manifest in pluginManifests) {
				foreach(var cssFile in manifest.Libraries.Css) {
					libraryCssFileList.Add(cssFile);
				}
				foreach(var cssFile in manifest.Module.Css) {
					moduleCssFileList.Add(cssFile);
				}
				foreach(var jsFile in manifest.Libraries.Js) {
					libraryJsFileList.Add(jsFile);
				}
				foreach(var jsFile in manifest.Module.Js) {
					moduleJsFileList.Add(jsFile);
				}
				foreach(var appInject in manifest.Module.WVAppInjects) {
					appDependencyInjections.Add(appInject);
				}
			}
			#endregion


			ViewBag.CacheBreaker = 20150920;
			ViewBag.Lang = Settings.Lang;
			ViewBag.LibraryCssFileList = libraryCssFileList;
			ViewBag.ModuleCssFileList = moduleCssFileList;
			ViewBag.LibraryJsFileList = libraryJsFileList;
			ViewBag.ModuleJsFileList = moduleJsFileList;
			ViewBag.AppDependencyInjections = appDependencyInjections;

            return View();
        }
    }
}
