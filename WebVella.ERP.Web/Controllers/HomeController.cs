using Microsoft.AspNet.Mvc;

namespace WebVella.ERP.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : ApiControllerBase
    {
        public HomeController(IErpService service) : base(service)
        {
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
			ViewBag.CacheBreaker = 20150920;
            return View();
        }
    }
}
