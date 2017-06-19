using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WebVella.ERP.Database;
using WebVella.ERP.Api;
using WebVella.ERP.Web.Security;
using System;

namespace WebVella.ERP.Web.Services
{
	public static class AppBuilderExtensions
	{
		public static void UseErpMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<ErpMiddleware>();
		}
		public static void UseDebugLogMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<DebugLogMiddleware>();
		}
	}

	public class ErpMiddleware
	{
		RequestDelegate next;


		public ErpMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			IDisposable dbCtx = DbContext.CreateContext(Settings.ConnectionString);
			IDisposable secCtx = null;

			WebSecurityUtil.Authenticate(context);
			var identity = (context.User as ErpPrincipal)?.Identity as ErpIdentity;
			if (identity != null)
				secCtx = SecurityContext.OpenScope(identity.User);

			await next(context);
			await Task.Run(() => {
				if (dbCtx != null)
					dbCtx.Dispose();
				if (secCtx != null)
					secCtx.Dispose();
			});
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