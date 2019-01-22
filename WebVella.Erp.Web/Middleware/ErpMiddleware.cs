using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebVella.Erp.Database;
using WebVella.Erp.Api;
using System;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Web.Middleware
{
	public class ErpMiddleware
	{
		RequestDelegate next;

		public ErpMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			IDisposable dbCtx = DbContext.CreateContext(ErpSettings.ConnectionString);
			IDisposable secCtx = null;

			ErpUser user = AuthService.GetUser(context.User);
			if (user != null)
				secCtx = SecurityContext.OpenScope(user);

			await next(context);
			await Task.Run(() =>
			{
				if (dbCtx != null)
					dbCtx.Dispose();
				if (secCtx != null)
					secCtx.Dispose();
			});
		}
	}
}