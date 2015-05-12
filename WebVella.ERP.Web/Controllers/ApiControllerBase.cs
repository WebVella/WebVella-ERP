using Microsoft.AspNet.Mvc;
using System.Net;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Web.Controllers
{
    public class ApiControllerBase : Controller
    {
        protected IERPService service;

        public ApiControllerBase(IERPService service)
        {
            this.service = service;
        }

        public IActionResult DoResponse(BaseResponseModel response )
        {
            if (response.Errors.Count > 0)
                Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return Json(response);
        }
    }
}
