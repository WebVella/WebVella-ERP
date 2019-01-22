using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;

namespace WebVella.Erp.Web.Middleware
{
	public class ErpDebugLogMiddleware
	{
		IErpService service;
		RequestDelegate next;

		public ErpDebugLogMiddleware(RequestDelegate next, IErpService service)
		{
			this.next = next;
			this.service = service;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				//var httpFeature = context.GetFeature<IHttpConnectionFeature>();
				await next(context);
			}
			catch (Exception ex)
			{
				var exception = ex;
				throw ex;
			}
		}
	}
}
