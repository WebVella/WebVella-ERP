using System;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using WebVella.ERP.Storage;
using WebVella.ERP.Storage.Mongo;
using WebVella.ERP.Api.Models.AutoMapper;
using System.Globalization;
using Microsoft.Framework.Runtime;
using WebVella.ERP.Web.Security;

namespace WebVella.ERP.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var configurationBuilder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
               .AddJsonFile("config.json")
               .AddEnvironmentVariables();
            Configuration = configurationBuilder.Build();

            //beta 4 configuration change
            //Configuration = new Configuration().AddJsonFile("config.json");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton<IStorageService, MongoStorageService>();
            services.AddSingleton<IErpService, ErpService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            Settings.Initialize(Configuration);
            IErpService service = app.ApplicationServices.GetService<IErpService>();
            AutoMapperConfiguration.Configure(service);

            app.UseDebugLogMiddleware();
            app.UseSecurityMiddleware();
           

            //app.Run(async context =>
            //{
            //    IErpService service = app.ApplicationServices.GetService<IErpService>();
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
                app.UseErrorPage();
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
