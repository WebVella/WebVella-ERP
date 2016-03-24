using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using WebVella.ERP.Database;

namespace WebVella.ERP.Web.Security
{
    public static class AppBuilderExtensions
    {
        public static void UseSecurityMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<SecurityMiddleware>();
        }

		public static void UseDatabaseContextMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<DatabaseContextMiddleware>();
		}
	}

    public class SecurityMiddleware
    {
        IErpService service;
        RequestDelegate next;

        public SecurityMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
			using (var dbCtx = DbContext.CreateContext(Settings.ConnectionString))
			{
				WebSecurityUtil.Authenticate(context);
				WebSecurityUtil.OpenScope(context);
				await next(context);
				WebSecurityUtil.CloseScope(context);
			}
		}
    }

	public class DatabaseContextMiddleware
	{
		RequestDelegate next;

		public DatabaseContextMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			
				await next(context);
		}
	}
}