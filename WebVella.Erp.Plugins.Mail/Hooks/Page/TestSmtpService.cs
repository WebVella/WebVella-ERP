using Microsoft.AspNetCore.Mvc;
using System;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Mail.Api;
using WebVella.Erp.Plugins.Mail.Services;
using WebVella.Erp.Utilities;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Pages.Application;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.Mail.Hooks.Page
{
	[HookAttachment(key: "test_smtp_service")]
	public class TestSmtpService : IRecordDetailsPageHook
	{
		public IActionResult OnGet(RecordDetailsPageModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(RecordDetailsPageModel pageModel)
		{
			return new SmtpInternalService().TestSmtpServiceOnPost(pageModel);
		}
	}
}
