using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Record related record manage page hooks")]
	public interface IRecordRelatedRecordManagePageHook
	{
		IActionResult OnPreManageRecord(EntityRecord record, Entity entity, RecordRelatedRecordManagePageModel pageModel, List<ValidationError> validationErrors );
		IActionResult OnPostManageRecord(EntityRecord record, Entity entity, RecordRelatedRecordManagePageModel pageModel);
	}
}
