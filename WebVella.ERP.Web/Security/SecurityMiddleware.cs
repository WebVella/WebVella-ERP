using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;

namespace WebVella.ERP.Web.Security
{
    public static class AppBuilderExtensions
    {
        public static void UseSecurityMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<SecurityMiddleware>();
        }
    }

    public class SecurityMiddleware
    {
        IErpService service;
        RequestDelegate next;

        public SecurityMiddleware(RequestDelegate next, IErpService service)
        {
            this.next = next;
            this.service = service;
        }

        public async Task Invoke(HttpContext context)
        {
            WebSecurityUtil.Authenticate(context, service);
            WebSecurityUtil.OpenScope(context);
            await next(context);
            WebSecurityUtil.CloseScope(context);
        }


    }


}