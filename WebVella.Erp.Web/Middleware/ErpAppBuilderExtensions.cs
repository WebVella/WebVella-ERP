using Microsoft.AspNetCore.Builder;

namespace WebVella.Erp.Web.Middleware
{
	public static class AppBuilderExtensions
	{
		public static void UseErpMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<ErpMiddleware>();
		}
		public static void UseDebugLogMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<ErpDebugLogMiddleware>();
		}

		public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<ErpErrorHandlingMiddleware>();
		}
	}
}
