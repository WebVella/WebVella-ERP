using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using WebVella.Erp.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebVella.Erp.Web.Controllers
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

			return Json(response);
			//JsonSerializerSettings settings = new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc };
			//return Json(response, settings);

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

			if (ErpSettings.DevelopmentMode)
			{
				if (ex != null)
					response.Message = ex.Message + ex.StackTrace;
			}
			else
			{
				if (string.IsNullOrEmpty(message))
					response.Message = "An internal error occurred!";
			}

			HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return Json(response);
		}
	}
}
