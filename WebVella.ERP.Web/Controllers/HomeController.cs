using Microsoft.AspNet.Mvc;

namespace WebVella.ERP.Web.Controllers
{
    public class HomeController : ApiControllerBase
    {
        public HomeController(IErpService service) : base(service)
        {
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            service.RunTests();
            return View();
        }
    }
}
