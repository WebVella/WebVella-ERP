using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;
using System.Diagnostics;
using Microsoft.AspNet.Http.Features;

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
            //var httpFeature = context.GetFeature<IHttpConnectionFeature>();
            Debug.WriteLine("BEGIN REQUEST:" + context.Request.Host);
            await next(context);
            Debug.WriteLine("END REQUEST:" + context.Request.Host);
        }
    }


}