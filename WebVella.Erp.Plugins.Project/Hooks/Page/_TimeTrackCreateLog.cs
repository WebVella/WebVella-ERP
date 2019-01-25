using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;
using WebVella.Erp.Eql;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Project.Services;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Plugins.Project.Hooks.Page
{
	[HookAttachment(key: "TimeTrackCreateLog")]
	public class _TimeTrackCreateLog : IApplicationNodePageHook
	{

		public IActionResult OnGet(ApplicationNodePageModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(ApplicationNodePageModel pageModel)
		{
			return new TimeLogService().PostApplicationNodePageHookLogic(pageModel);
		}
	}

}
