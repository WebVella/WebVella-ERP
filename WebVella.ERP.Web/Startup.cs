using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.Runtime.Infrastructure;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using WebVella.ERP;
using WebVella.ERP.Storage.Mongo;

namespace WebVella.ERP.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = new Configuration().AddJsonFile("config.json");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IStorageService, MongoStorageService>();
            services.AddSingleton<IERPService, ERPService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env )
        {
           app.Run(async context =>
           {
               IERPService service = app.ApplicationServices.GetService<IERPService>();
               service.Run();
               context.Response.ContentType = "text/html";
               context.Response.StatusCode = 200;
               System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
               byte[] buffer = encoding.GetBytes("<h1>test</h1>");
               await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
           });
        }
    }
}
