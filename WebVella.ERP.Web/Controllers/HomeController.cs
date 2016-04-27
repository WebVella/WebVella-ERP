using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
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
			ViewBag.CacheBreaker = 20150920;
			//pluginService.
			ViewBag.Lang = Settings.Lang;
            return View();
        }
    }
}
