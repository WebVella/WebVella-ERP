using Microsoft.AspNet.Mvc;

namespace WebVella.ERP.Project
{
	public class ApiController : Controller
	{
		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-plugin/api/")]
		public IActionResult Index()
		{
			return Json(new { Message = "This is a sample json response from webvella erp sample plugin controller." });
		}
	}
}
