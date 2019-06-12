#region <--- DIRECTIVES --->

using WebVella.Erp.Web.Models.AutoMapper.Profiles;
using AutoMapper.Configuration;

#endregion

namespace WebVella.Erp.Plugins.Mail.Api.AutoMapper
{
	public class MailPluginAutoMapperConfiguration
	{
		public static MapperConfigurationExpression MappingExpressions = new MapperConfigurationExpression();

		private static object lockObj = new object();
		private static bool alreadyConfigured = false;

		public static void Configure(MapperConfigurationExpression cfg)
		{
			if (alreadyConfigured)
				return;

			lock (lockObj)
			{
				if (alreadyConfigured)
					return;

				alreadyConfigured = true;

				cfg.AddProfile(new SmtpServiceProfile());
				cfg.AddProfile(new EmailProfile());
			}
		}
	}
}