using Microsoft.AspNetCore.Mvc;
using System;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Mail.Api;
using WebVella.Erp.Plugins.Mail.Services;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Pages.Application;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.Mail.Hooks.Page
{
	[HookAttachment(key: "email_send_now")]
	public class EmailSendNow : IRecordDetailsPageHook
	{
		public IActionResult OnPost(RecordDetailsPageModel pageModel)
		{
			var emailId = (Guid)pageModel.DataModel.GetProperty("Record.id");

			var internalSmtpSrv = new SmtpInternalService();
			Email email = internalSmtpSrv.GetEmail(emailId);
			SmtpService smtpService = new ServiceManager().GetSmtpService(email.ServiceId);
			internalSmtpSrv.SendEmail(email, smtpService);

			if (email.Status == EmailStatus.Sent)
				pageModel.TempData.Put("ScreenMessage", new ScreenMessage() { Message = "Email was successfully sent", Type=ScreenMessageType.Success, Title="Success" });
			else
				pageModel.TempData.Put("ScreenMessage", new ScreenMessage() { Message = email.ServerError, Type = ScreenMessageType.Error, Title="Error" });

			var returnUrl = pageModel.HttpContext.Request.Query["returnUrl"];
			return new RedirectResult($"/mail/emails/all/r/{emailId}/details?returnUrl={returnUrl}");
		}
	}
}
