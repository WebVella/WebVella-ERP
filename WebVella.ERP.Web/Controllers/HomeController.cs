using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace WebVella.ERP.Web.Controllers
{
    public class HomeController : ApiControllerBase
    {
        public HomeController(IErpService service) : base(service)
        {
        }

		[AllowAnonymous]
        // GET: /<controller>/
        public IActionResult Index()
        {
			ViewBag.CacheBreaker = 20150920;
            return View();
        }
    }
}
