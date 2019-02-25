using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Project.Services;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Plugins.Project.Hooks.Page
{
	[HookAttachment(key: "SetTaskRecurrence")]
	public class SetTaskRecurrence : IRecordDetailsPageHook
	{
		public IActionResult OnPost(RecordDetailsPageModel pageModel)
		{
			return new TaskService().SetTaskRecurrenceOnPost(pageModel);
		}
	}

}
