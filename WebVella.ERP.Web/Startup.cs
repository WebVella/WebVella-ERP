using System;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.Runtime.Infrastructure;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using WebVella.ERP;
using WebVella.ERP.Storage;
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
            services.AddMvc();

            services.AddSingleton<IStorageService, MongoStorageService>();
            services.AddSingleton<IERPService, ERPService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.Run(async context =>
            //{
            //    IERPService service = app.ApplicationServices.GetService<IERPService>();
            //    service.Run();
            //    context.Response.ContentType = "text/html";
            //    context.Response.StatusCode = 200;
            //    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            //    byte[] buffer = encoding.GetBytes("<h1>test</h1>");
            //    await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            //});

            // Add the following to the request pipeline only in development environment.
            if (string.Equals(env.EnvironmentName, "Development", StringComparison.OrdinalIgnoreCase))
            {
                app.UseBrowserLink();
                app.UseErrorPage(ErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });
        }
    }
}
