using MailKit.Security;
using System;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;

namespace WebVella.Erp.Plugins.Mail.Api
{
	public class ServiceManager
	{
		public SmtpService GetSmtpService(string name = null)
		{
			EntityRecord smtpServiceRec = null;
			if (name != null)
			{
				var result = new EqlCommand("SELECT * FROM smtp_service WHERE name = @name", new EqlParameter("name", name)).Execute();
				if (result.Count == 0)
					throw new Exception($"SmtpService with name '{name}' not found.");

				smtpServiceRec = result[0];
			}
			else
			{
				var result = new EqlCommand("SELECT * FROM smtp_service WHERE is_default = @is_default", new EqlParameter("is_default", true)).Execute();
				if (result.Count == 0)
					throw new Exception($"Default SmtpService not found.");
				else if (result.Count > 1)
					throw new Exception($"More than one default SmtpService not found.");

				smtpServiceRec = result[0];
			}

			SmtpService smtpService = new SmtpService();
			smtpService.Id = (Guid)smtpServiceRec["id"];
			smtpService.Name = (string)smtpServiceRec["name"];
			smtpService.Server = (string)smtpServiceRec["server"];
			smtpService.Port = (int)((decimal)smtpServiceRec["port"]);
			smtpService.Username = (string)smtpServiceRec["username"];
			smtpService.Password = (string)smtpServiceRec["password"];
			smtpService.DefaultFromEmail = (string)smtpServiceRec["default_from_email"];
			smtpService.DefaultFromName = (string)smtpServiceRec["default_from_name"];
			smtpService.DefaultReplyToEmail = (string)smtpServiceRec["default_reply_to_email"];
			smtpService.MaxRetriesCount = (int)((decimal)smtpServiceRec["max_retries_count"]);
			smtpService.RetryWaitMinutes = (int)((decimal)smtpServiceRec["retry_wait_minutes"]);
			smtpService.IsDefault = (bool)smtpServiceRec["is_default"];
			smtpService.ConnectionSecurity = (SecureSocketOptions)(int.Parse((string)smtpServiceRec["connection_security"]));
			return smtpService;
		}
	}
}
