using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using System.Threading;

namespace WebVella.ERP.Web.Security
{
	public static class LogAppBuilderExtensions
	{
		public static void UseDebugLogMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<DebugLogMiddleware>();
		}
	}

	public class DebugLogMiddleware
	{
		IErpService service;
		RequestDelegate next;

		public DebugLogMiddleware(RequestDelegate next, IErpService service)
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