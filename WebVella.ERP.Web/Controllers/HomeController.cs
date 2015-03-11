using Microsoft.AspNet.Mvc;

namespace WebVella.ERP.Web.Controllers
{
    public class HomeController : Controller
    {
        private IERPService service;

        public HomeController(IERPService service)
        {
            this.service = service;
        }


        // GET: /<controller>/
        public IActionResult Index( )
        {
            service.RunTests();
            return View();
        }
    }
}
