using System;
using MailKit.Security;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.Mapping;

namespace WebVella.Erp.Plugins.Mail.Api.Mapping;

internal class EntityRecordToSmtpServiceStrategy : IMapStrategy<EntityRecord, SmtpService>
{
	public SmtpService Map(EntityRecord rec)
	{
		if (rec == null)
			return null;

		SmtpService model = new SmtpService();
		model.Id = (Guid)rec["id"];
		model.Name = (string)rec["name"];
		model.Server = (string)rec["server"];
		model.Port = (int)((decimal)rec["port"]);
		model.Username = (string)rec["username"];
		model.Password = (string)rec["password"];
		model.DefaultSenderEmail = (string)rec["default_sender_email"];
		model.DefaultSenderName = (string)rec["default_sender_name"];
		model.DefaultReplyToEmail = (string)rec["default_reply_to_email"];
		model.MaxRetriesCount = (int)((decimal)rec["max_retries_count"]);
		model.RetryWaitMinutes = (int)((decimal)rec["retry_wait_minutes"]);
		model.IsDefault = (bool)rec["is_default"];
		model.IsEnabled = (bool)rec["is_enabled"];
		model.ConnectionSecurity = (SecureSocketOptions)(int.Parse((string)rec["connection_security"]));
		return model;
	}
}
