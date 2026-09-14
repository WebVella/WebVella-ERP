using System.Collections.Generic;
using Newtonsoft.Json;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.Mapping;

namespace WebVella.Erp.Plugins.Mail.Api.Mapping;

internal class EmailToEntityRecordStrategy : IMapStrategy<Email, EntityRecord>
{
	public EntityRecord Map(Email model)
	{
		if (model == null)
			return null;

		EntityRecord rec = new EntityRecord();
		rec["id"] = model.Id;
		rec["service_id"] = model.ServiceId;
		rec["sender"] = JsonConvert.SerializeObject(model.Sender ?? new EmailAddress());
		rec["recipients"] = JsonConvert.SerializeObject(model.Recipients ?? new List<EmailAddress>());
		rec["subject"] = model.Subject;
		rec["reply_to_email"] = model.ReplyToEmail;
		rec["content_text"] = model.ContentText;
		rec["content_html"] = model.ContentHtml;
		rec["created_on"] = model.CreatedOn;
		rec["sent_on"] = model.SentOn;
		rec["status"] = ((int)model.Status).ToString();
		rec["priority"] = ((int)model.Priority).ToString();
		rec["server_error"] = model.ServerError;
		rec["scheduled_on"] = model.ScheduledOn;
		rec["retries_count"] = (decimal)model.RetriesCount;
		rec["x_search"] = model.XSearch;
		rec["attachments"] = JsonConvert.SerializeObject(model.Attachments ?? new List<string>());
		return rec;
	}
}
