using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Pages.Application;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web.Hooks.TestHooks
{
	[HookAttachment("test")]
	public class TestRecordRelatedRecordManagePageCustomImplHook : IRecordRelatedRecordManagePageCustomImplHook
	{
		public IActionResult OnManageRecord(EntityRecord record, Entity entity, RecordRelatedRecordManagePageModel pageModel)
		{
			pageModel.TempData.Put("ScreenMessage", new ScreenMessage() { Message = "Hook on manage record related record was executed. This is hooks test and nothing is update." });
			return null;
		}
	}
}
