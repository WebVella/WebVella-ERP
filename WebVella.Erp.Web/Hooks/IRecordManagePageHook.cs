using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Record manage page hooks")]
	public interface IRecordManagePageHook
	{
		IActionResult OnPreManageRecord(EntityRecord record, Entity entity, RecordManagePageModel pageModel, List<ValidationError> validationErrors );
		IActionResult OnPostManageRecord(EntityRecord record, Entity entity, RecordManagePageModel pageModel);
	}
}
