using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace WebVella.ERP.Web.Controllers
{
    public class HomeController : ApiControllerBase
    {
		[AllowAnonymous]
        // GET: /<controller>/
        public IActionResult Index()
        {
			ViewBag.CacheBreaker = 20150920;
            return View();
        }
    }
}
