using Microsoft.Extensions.Configuration;
using System;

namespace WebVella.Erp
{
	public static class ErpSettings
	{
		public static string EncriptionKey { get; private set; }
		public static string ConnectionString { get; private set; }
		public static string Lang { get; private set; }
        public static string Locale { get; private set; }
        public static string CacheKey { get; private set; }
        public static bool EnableBackgroungJobs { get; private set; }
		public static bool EnableFileSystemStorage { get; private set; }
		public static string FileSystemStorageFolder { get; set; }
        public static string DevelopmentTestEntityName { get; set; }
        public static Guid DevelopmentTestRecordId { get; set; }
        public static string DevelopmentTestRecordViewName { get; set; }
        public static string DevelopmentTestRecordListName { get; set; }
        public static string TimeZoneName { get; set; }
		public static string JsonDateTimeFormat { get; set; }

        public static bool EmailEnabled { get; private set; }
        public static string EmailSMTPServerName { get; private set; }
        public static int EmailSMTPPort { get; private set; }
        public static string EmailSMTPUsername { get; private set; }
        public static string EmailSMTPPassword { get; private set; }
        public static string EmailFrom { get; private set; }
        public static string EmailTo { get; private set; }

        public static string NavLogoUrl { get; private set; }
        public static string SystemMasterBackgroundImageUrl { get; private set; }
        public static string AppName { get; private set; }

        public static bool ShowAccounting { get; set; }
		public static bool DevelopmentMode { get; private set; }

		//API URLs
		public static string ApiUrlTemplateFieldInlineEdit { get; private set; }

		public static void Initialize(IConfiguration configuration)
		{
			EncriptionKey = configuration["Settings:EncriptionKey"];
			ConnectionString = configuration["Settings:ConnectionString"];
			Lang = string.IsNullOrWhiteSpace(configuration["Settings:Lang"]) ? @"en" : configuration["Settings:Lang"];
            // 125	FLE Standard Time	(GMT+02:00) Helsinki, Kiev, Riga, Sofia, Tallinn, Vilnius
			//TODO - disq about using as default hosting server timezone when not specified in configuration
            TimeZoneName = string.IsNullOrWhiteSpace(configuration["Settings:TimeZoneName"]) ? @"FLE Standard Time" : configuration["Settings:TimeZoneName"];
			JsonDateTimeFormat = string.IsNullOrWhiteSpace(configuration["Settings:JsonDateTimeFormat"]) ? "yyyy-MM-ddTHH:mm:ss.fff" : configuration["Settings:JsonDateTimeFormat"];

            Locale = string.IsNullOrWhiteSpace(configuration["Settings:Locale"]) ? "en-US" : configuration["Settings:Locale"];
            CacheKey = string.IsNullOrWhiteSpace(configuration["Settings:CacheKey"]) ? $"{DateTime.Now.ToString("yyyyMMdd")}" : configuration["Settings:CacheKey"];

            EnableFileSystemStorage = string.IsNullOrWhiteSpace(configuration["Settings:EnableFileSystemStorage"]) ? false : bool.Parse(configuration["Settings:EnableFileSystemStorage"]);
			FileSystemStorageFolder = string.IsNullOrWhiteSpace(configuration["Settings:FileSystemStorageFolder"]) ? @"c:\erp-files" : configuration["Settings:FileSystemStorageFolder"];
			EnableBackgroungJobs = string.IsNullOrWhiteSpace(configuration["Settings:EnableBackgroungJobs"]) ? true : bool.Parse(configuration["Settings:EnableBackgroungJobs"]);

            DevelopmentTestEntityName = string.IsNullOrWhiteSpace(configuration["Development:TestEntityName"]) ? @"test" : configuration["Development:TestEntityName"];
            DevelopmentTestRecordId = new Guid("001ea36f-fd2e-4d1b-b8ee-25d32d4e396c");
            DevelopmentTestRecordViewName = "test";
            DevelopmentTestRecordListName = "test";
            var outGuid = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(configuration["Development:TestRecordId"]) && Guid.TryParse(configuration["Development:TestRecordId"], out outGuid)) {
                DevelopmentTestRecordId = outGuid;
            }

            EmailEnabled = string.IsNullOrWhiteSpace(configuration[$"Settings:EmailEnabled"]) ? false : bool.Parse(configuration[$"Settings:EmailEnabled"]);
            EmailSMTPServerName = configuration[$"Settings:EmailSMTPServerName"];
            EmailSMTPPort = string.IsNullOrWhiteSpace(configuration[$"Settings:EmailSMTPPort"]) ? 25 : int.Parse(configuration[$"Settings:EmailSMTPPort"]);
            EmailSMTPUsername = configuration[$"Settings:EmailSMTPUsername"];
            EmailSMTPPassword = configuration[$"Settings:EmailSMTPPassword"];
            EmailFrom = configuration[$"Settings:EmailFrom"];
            EmailTo = configuration[$"Settings:EmailTo"];

            NavLogoUrl = configuration[$"Settings:NavLogoUrl"];
            SystemMasterBackgroundImageUrl = configuration[$"Settings:SystemMasterBackgroundImageUrl"];
            AppName = configuration[$"Settings:AppName"];

            DevelopmentMode = string.IsNullOrWhiteSpace(configuration[$"Settings:DevelopmentMode"]) ? false : bool.Parse(configuration[$"Settings:DevelopmentMode"]);

			ShowAccounting = string.IsNullOrWhiteSpace(configuration[$"Settings:ShowAccounting"]) ? false : bool.Parse(configuration[$"Settings:ShowAccounting"]);
			

			ApiUrlTemplateFieldInlineEdit = string.IsNullOrWhiteSpace(configuration[$"ApiUrlTemplates:FieldInlineEdit"]) ? "/api/v3/en_US/record/{entityName}/{recordId}" : configuration[$"ApiUrlTemplates:FieldInlineEdit"];
		}
	}
}
