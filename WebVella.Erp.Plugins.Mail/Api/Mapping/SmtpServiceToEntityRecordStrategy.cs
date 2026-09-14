using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.Mapping;

namespace WebVella.Erp.Plugins.Mail.Api.Mapping;

internal class SmtpServiceToEntityRecordStrategy : IMapStrategy<SmtpService, EntityRecord>
{
	public EntityRecord Map(SmtpService model)
	{
		if (model == null)
			return null;

		EntityRecord rec = new EntityRecord();
		rec["id"] = model.Id;
		rec["name"] = model.Name;
		rec["server"] = model.Server;
		rec["port"] = model.Port;
		rec["username"] = model.Username;
		rec["password"] = model.Password;
		rec["default_sender_email"] = model.DefaultSenderEmail;
		rec["default_sender_name"] = model.DefaultSenderName;
		rec["default_reply_to_email"] = model.DefaultReplyToEmail;
		rec["max_retries_count"] = (decimal)((int)model.MaxRetriesCount);
		rec["retry_wait_minutes"] = (decimal)((int)model.RetryWaitMinutes);
		rec["is_default"] = model.IsDefault;
		rec["is_enabled"] = model.IsEnabled;
		rec["connection_security"] = ((int)model.ConnectionSecurity).ToString();
		return rec;
	}
}
