using AutoMapper;
using System;
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
			model.FromName = (string)rec["from_name"];
			model.FromEmail = (string)rec["from_email"];
			model.ToName = (string)rec["to_name"];
			model.ToEmail = (string)rec["to_email"];
			model.ReplyToEmail = (string)rec["reply_to_email"];
			model.Subject= (string)rec["subject"];
			model.ContentText= (string)rec["content_text"];
			model.ContentHtml = (string)rec["content_html"];
			model.CreatedOn = (DateTime)rec["created_on"];
			model.SentOn = (DateTime?)rec["sent_on"];
			model.Status = (EmailStatus)((int)((decimal)rec["status"]));
			model.Priority = (EmailPriority)((int)((decimal)rec["priority"]));
			model.ServerError = (string)rec["server_error"];
			model.LastRetry = (DateTime?)rec["last_retry"];
			model.RetriesCount = (int)rec["retries_count"];
			return model;
		}


		private static EntityRecord EmailToEntityRecordConvert(Email model)
		{
			if (model == null)
				return null; 

			EntityRecord rec = new EntityRecord();
			rec["id"] = model.Id;
			rec["service_id"] = model.ServiceId;
			rec["from_name"] = model.FromName;
			rec["from_email"] = model.FromEmail;
			rec["to_name"] = model.ToName;
			rec["to_email"] = model.ToEmail;
			rec["reply_to_email"] = model.ReplyToEmail;
			rec["subject"] = model.Subject;
			rec["content_text"] = model.ContentText;
			rec["content_html"] = model.ContentHtml;
			rec["created_on"] = model.CreatedOn;
			rec["sent_on"] = model.SentOn;
			rec["status"] = ((int)model.Status).ToString();
			rec["priority"] = ((int)model.Priority).ToString();
			rec["server_error"] = model.ServerError;
			rec["last_retry"] = model.LastRetry;
			rec["retries_count"] = model.RetriesCount;
			return rec;
		}
	}
}
