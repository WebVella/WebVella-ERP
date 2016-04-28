using System;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebVella.ERP.Api.Models.AutoMapper;
using System.Globalization;
using Microsoft.Extensions.PlatformAbstractions;
using WebVella.ERP.Web.Security;
using WebVella.ERP.Database;
using Microsoft.AspNet.Mvc.Infrastructure;
using WebVella.ERP.Plugins;

namespace WebVella.ERP.Web
{
	public class Startup
	{
		public IConfiguration Configuration { get; set; }

		public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
		{
			var configurationBuilder = new ConfigurationBuilder()
			   .SetBasePath(appEnv.ApplicationBasePath)
			   .AddJsonFile("config.json")
			   .AddEnvironmentVariables();
			Configuration = configurationBuilder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
			services.AddSingleton<IErpService, ErpService>();
			services.AddSingleton<IPluginService, PluginService>();

			services.AddSingleton<IAssemblyProvider, PluginAssemblyProvider>(provider =>
			{
				var pluginAssemblyProvider = new PluginDirectoryAssemblyProvider(
					provider.GetRequiredService<IHostingEnvironment>(), 
					PlatformServices.Default.AssemblyLoadContextAccessor,
					PlatformServices.Default.AssemblyLoaderContainer);

				return new PluginAssemblyProvider(provider.GetRequiredService<ILibraryManager>(),new IAssemblyProvider[] { pluginAssemblyProvider });
			});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			//TODO Create db context
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
			Settings.Initialize(Configuration);

			try
			{
				DbContext.CreateContext(Settings.ConnectionString);

				IErpService service = app.ApplicationServices.GetService<IErpService>();
				AutoMapperConfiguration.Configure();
				service.InitializeSystemEntities();

				app.UseDebugLogMiddleware();
				app.UseSecurityMiddleware();
				app.UseDatabaseContextMiddleware();

				IPluginService pluginService = app.ApplicationServices.GetService<IPluginService>();
				IAssemblyProvider asmProviderService = app.ApplicationServices.GetService<IAssemblyProvider>();
				IHostingEnvironment hostingEnvironment = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();
				pluginService.Initialize(hostingEnvironment,asmProviderService);

			}
			finally
			{
				DbContext.CloseContext();
			}

			//Enable CORS
			//app.Use((context, next) =>
			//{
			//	context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
			//	context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "*" });
			//	context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "*" });
			//	return next();
			//});

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
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// Add Error handling middleware which catches all application specific errors and
				// send the request to the following path or controller action.
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseIISPlatformHandler(options => options.AutomaticAuthentication = false);

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

		// Entry point for the application.
		public static void Main(string[] args) => WebApplication.Run<Startup>(args);
	}
}
