using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP
{
	public static class Settings
	{
		public static string EncriptionKey { get; private set; }
		public static string ConnectionString { get; private set; }
		public static string CompanyName { get; private set; }
		public static string CompanyLogo { get; private set; }
		public static string Lang { get; private set; }
		public static string DevelopmentMode { get; private set; }
		public static bool EnableBackgroungJobs { get; private set; }
		public static bool EnableFileSystemStorage { get; private set; }
		public static string FileSystemStorageFolder { get; set; }


		public static void Initialize(IConfiguration configuration)
		{
			EncriptionKey = configuration["Settings:EncriptionKey"];
			ConnectionString = configuration["Settings:ConnectionString"];
			CompanyLogo = configuration["Settings:CompanyLogo"];
			CompanyName = configuration["Settings:CompanyName"];
			Lang = configuration["Settings:Lang"];
			DevelopmentMode = configuration["Settings:DevelopmentMode"];
			EnableFileSystemStorage = string.IsNullOrWhiteSpace(configuration["Settings:EnableFileSystemStorage"]) ? false : bool.Parse(configuration["Settings:EnableFileSystemStorage"]);
			FileSystemStorageFolder = string.IsNullOrWhiteSpace(configuration["Settings:FileSystemStorageFolder"]) ? @"c:\erp-files" : configuration["Settings:FileSystemStorageFolder"];
			EnableBackgroungJobs = string.IsNullOrWhiteSpace(configuration["Settings:EnableBackgroungJobs"]) ? false : bool.Parse(configuration["Settings:EnableBackgroungJobs"]);
		}
	}
}
