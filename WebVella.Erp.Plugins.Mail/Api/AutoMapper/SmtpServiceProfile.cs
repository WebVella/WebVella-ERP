using AutoMapper;
using MailKit.Security;
using System;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Plugins.Mail.Api;

namespace WebVella.Erp.Web.Models.AutoMapper.Profiles
{
	internal class SmtpServiceProfile : Profile
	{
		public SmtpServiceProfile()
		{
			CreateMap<EntityRecord, SmtpService>().ConvertUsing(source => EntityRecordToSmtpServiceConvert(source));
			CreateMap<SmtpService, EntityRecord>().ConvertUsing(source => SmtpServiceToEntityRecordConvert(source));
		}

		private static SmtpService EntityRecordToSmtpServiceConvert(EntityRecord rec)
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
			model.IsEnabled= (bool)rec["is_enabled"];
			model.ConnectionSecurity = (SecureSocketOptions)(int.Parse((string)rec["connection_security"]));
			return model;
		}


		private static EntityRecord SmtpServiceToEntityRecordConvert(SmtpService model)
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
}
