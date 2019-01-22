using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Record create page hooks")]
	public interface IRecordCreatePageHook
	{
		IActionResult OnPreCreateRecord(EntityRecord record, Entity entity, RecordCreatePageModel pageModel, List<ValidationError> validationErrors );
		IActionResult OnPostCreateRecord(EntityRecord record, Entity entity, RecordCreatePageModel pageModel);
	}
}
