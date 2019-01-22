using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Pages.Application;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web.Hooks.TestHooks
{
	[HookAttachment()]
	public class TestRecordRelatedRecordCreatePageHook : IRecordRelatedRecordCreatePageHook
	{
		public IActionResult OnPostCreateRecord(EntityRecord record, Entity entity, RecordRelatedRecordCreatePageModel pageModel)
		{
			pageModel.TempData.Put("ScreenMessage", new ScreenMessage() { Message = "Record is created successfully" });
			return null;
		}

		public IActionResult OnPreCreateRecord(EntityRecord record, Entity entity, RecordRelatedRecordCreatePageModel pageModel, List<ValidationError> validationErrors)
		{
			if( record.Properties.ContainsKey("name"))
			{
				if (record["name"] as string == "123")
					validationErrors.Add(new ValidationError("name", "123 value is not permitted"));
			}
			return null;
		}
	}
}
