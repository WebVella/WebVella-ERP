using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Pages.Application;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web.Hooks.TestHooks
{
	[HookAttachment("test")]
	public class TestRecordCreatePageCustomImplHook : IRecordCreatePageCustomImplHook
	{
		public IActionResult OnCreateRecord(EntityRecord record, Entity entity, RecordCreatePageModel pageModel)
		{
			pageModel.TempData.Put("ScreenMessage", new ScreenMessage() { Message = "Custom implementation of create record was executed. This is hooks test and nothing is created." });
			return null;
		}
	}
}
