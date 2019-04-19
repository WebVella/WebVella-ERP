using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Mail.Api;
using WebVella.Erp.Plugins.Mail.Services;

namespace WebVella.Erp.Plugins.Mail.Hooks.Api
{
	[HookAttachment("smtp_service")]
	public class SmtpServiceRecordHook : IErpPreUpdateRecordHook, IErpPreCreateRecordHook,
		IErpPostCreateRecordHook, IErpPostUpdateRecordHook, IErpPreDeleteRecordHook
	{
		SmtpInternalService smtpIntService = new SmtpInternalService();

		public void OnPreCreateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			smtpIntService.ValidatePreCreateRecord(record, errors);
			if (errors.Any())
				return;
			smtpIntService.HandleDefaultServiceSetup(record,errors);
		}

		public void OnPreUpdateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			smtpIntService.ValidatePreUpdateRecord(record, errors);
			if (errors.Any())
				return;
			smtpIntService.HandleDefaultServiceSetup(record,errors);
		}

		public void OnPostCreateRecord(string entityName, EntityRecord record)
		{
			EmailServiceManager.ClearCache();
		}

		public void OnPostUpdateRecord(string entityName, EntityRecord record)
		{
			EmailServiceManager.ClearCache();
		}

		public void OnPreDeleteRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			var service = new EmailServiceManager().GetSmtpService((Guid)record["id"]);
			if( service != null && service.IsDefault )
				errors.Add(new ErrorModel { Key = "id", Message = "Default smtp service cannot be deleted." });
			else
				EmailServiceManager.ClearCache();
		}

	}
}
