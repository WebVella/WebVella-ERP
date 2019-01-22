using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Record related record create page hooks")]
	public interface IRecordRelatedRecordCreatePageHook
	{
		IActionResult OnPreCreateRecord(EntityRecord record, Entity entity, RecordRelatedRecordCreatePageModel pageModel, List<ValidationError> validationErrors );
		IActionResult OnPostCreateRecord(EntityRecord record, Entity entity, RecordRelatedRecordCreatePageModel pageModel);
	}
}
