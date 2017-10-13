using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Web.Security;

namespace WebVella.ERP.Web.Controllers
{
	[Authorize]
	public class ApiControllerBase : Controller
	{
		public ApiControllerBase()
		{
		}

		public IActionResult DoResponse( BaseResponseModel response )
		{
			if (response.Errors.Count > 0 || !response.Success)
			{
				if( response.StatusCode == HttpStatusCode.OK )
					HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				else
					HttpContext.Response.StatusCode = (int)response.StatusCode;
			}

			JsonSerializerSettings settings = new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc };

			return Json(response, settings);
		}

		public IActionResult DoPageNotFoundResponse()
		{
			HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
			return Json(new { });
		}

		public IActionResult DoItemNotFoundResponse(BaseResponseModel response)
		{
			HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
			return Json(response);
		}

		public IActionResult DoBadRequestResponse(BaseResponseModel response, string message = null, Exception ex = null)
		{
			response.Timestamp = DateTime.UtcNow;
			response.Success = false;
#if DEBUG
			if (ex != null)
			{
				response.Message = ex.Message + ex.StackTrace;
			}
#else
			if (string.IsNullOrEmpty( message ))
				response.Message = "An internal error occurred!";
#endif
			HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return Json(response);
		}
	}
}
