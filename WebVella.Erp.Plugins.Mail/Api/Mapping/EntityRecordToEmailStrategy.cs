using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.Mapping;

namespace WebVella.Erp.Plugins.Mail.Api.Mapping;

internal class EntityRecordToEmailStrategy : IMapStrategy<EntityRecord, Email>
{
	public Email Map(EntityRecord rec)
	{
		if (rec == null)
			return null;

		Email model = new Email();
		model.Id = (Guid)rec["id"];
		model.ServiceId = (Guid)rec["service_id"];
		model.Sender = JsonConvert.DeserializeObject<EmailAddress>((string)rec["sender"]);
		model.Recipients = JsonConvert.DeserializeObject<List<EmailAddress>>((string)rec["recipients"]);
		model.ReplyToEmail = (string)rec["reply_to_email"];
		model.Subject = (string)rec["subject"];
		model.ContentText = (string)rec["content_text"];
		model.ContentHtml = (string)rec["content_html"];
		model.CreatedOn = (DateTime)rec["created_on"];
		model.SentOn = (DateTime?)rec["sent_on"];
		model.Status = (EmailStatus)(int.Parse((string)rec["status"]));
		model.Priority = (EmailPriority)(int.Parse((string)rec["priority"]));
		model.ServerError = (string)rec["server_error"];
		model.ScheduledOn = (DateTime?)rec["scheduled_on"];
		model.RetriesCount = (int)((decimal)rec["retries_count"]);
		model.XSearch = (string)rec["x_search"];

		if (!string.IsNullOrWhiteSpace((string)rec["attachments"]))
			model.Attachments = JsonConvert.DeserializeObject<List<string>>((string)rec["attachments"]);
		else
			model.Attachments = new List<string>();

		return model;
	}
}
