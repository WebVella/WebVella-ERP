using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Plugins.Mail.Api;

namespace WebVella.Erp.Web.Models.AutoMapper.Profiles
{
	internal class EmailProfile : Profile
	{
		public EmailProfile()
		{
			CreateMap<EntityRecord, Email>().ConvertUsing(source => EntityRecordToEmailConvert(source));
			CreateMap<Email, EntityRecord>().ConvertUsing(source => EmailToEntityRecordConvert(source));
		}

		private static Email EntityRecordToEmailConvert(EntityRecord rec)
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


		private static EntityRecord EmailToEntityRecordConvert(Email model)
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
}
