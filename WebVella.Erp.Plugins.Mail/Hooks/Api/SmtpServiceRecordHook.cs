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
		SmtpManagementService smtpManagementService = new SmtpManagementService();

		public void OnPreCreateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			smtpManagementService.ValidatePreCreateRecord(record, errors);
			if (errors.Any())
				return;
			smtpManagementService.HandleDefaultServiceSetup(record,errors);
		}

		public void OnPreUpdateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			smtpManagementService.ValidatePreUpdateRecord(record, errors);
			if (errors.Any())
				return;
			smtpManagementService.HandleDefaultServiceSetup(record,errors);
		}

		public void OnPostCreateRecord(string entityName, EntityRecord record)
		{
			ServiceManager.ClearCache();
		}

		public void OnPostUpdateRecord(string entityName, EntityRecord record)
		{
			ServiceManager.ClearCache();
		}

		public void OnPreDeleteRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			ServiceManager.ClearCache();
		}

	}
}
