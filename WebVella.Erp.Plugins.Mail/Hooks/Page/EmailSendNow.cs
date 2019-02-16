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
			return new SmtpInternalService().EmailSendNowOnPost(pageModel);
		}
	}
}
