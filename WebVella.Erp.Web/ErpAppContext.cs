using AutoMapper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Wangkanai.Detection;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web
{
	public class ErpAppContext
	{
		public static ErpAppContext Current { get; private set; }

		public IServiceProvider ServiceProvider { get; private set; }

		//public string StyleFrameworkHash { get; private set; } = "";

		//public string StyleFrameworkContent { get; private set; } = "";

		public string StylesHash { get; private set; } = "";

		public string StylesContent { get; private set; } = "";

		public Theme Theme { get; private set; }

		public WebSettings CoreSettings { get; private set; }

		public IConfigurationRoot Configuration { get; private set; } = null;

		public List<ScriptTagInclude> ScriptIncludes { get; private set; } = new List<ScriptTagInclude>();

		//public List<LinkTagInclude> LinkIncludes { get; private set; } = new List<LinkTagInclude>();

		public Cache Cache { get; private set; } = new Cache();

		//Constructors
		internal static void Init(IServiceProvider serviceProvider)
		{
			Current = new ErpAppContext(serviceProvider);
		}

		private ErpAppContext(IServiceProvider serviceProvider)
		{
			IWebHostEnvironment env = serviceProvider.GetService<IWebHostEnvironment>();
			string configPath = "config.json";
			Configuration = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile(configPath).Build();

			ServiceProvider = serviceProvider;
			InitCoreSettings();
			InitTheme();
		}



		//methods Init
		private void InitCoreSettings()
		{
			CoreSettings = new WebSettingsService().Get();
		}

		private void InitTheme()
		{
			var themeService = new ThemeService();
			Theme = themeService.Get();
			StylesContent = themeService.GenerateStylesContent();
			StylesHash = GetStringHash(StylesContent, new SHA256CryptoServiceProvider());
			//StyleFrameworkContent = themeService.GenerateStyleFrameworkContent();
			//StyleFrameworkHash = GetStringHash(StyleFrameworkContent, new SHA256CryptoServiceProvider());
		}

		public static string GetStringHash(string content, HashAlgorithm algorithm)
		{

			var hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(content));

			return Convert.ToBase64String(hashedBytes);
		}

		//public static void UpdateStyleFrameworkContent(string content)
		//{
		//	Current.StyleFrameworkContent = content;
		//	Current.StyleFrameworkHash = GetStringHash(content, new SHA256CryptoServiceProvider());
		//}

		public static void UpdateStylesFileContent(string content)
		{
			Current.StylesContent = content;
			Current.StylesHash = GetStringHash(content, new SHA256CryptoServiceProvider());
		}


	}
}
