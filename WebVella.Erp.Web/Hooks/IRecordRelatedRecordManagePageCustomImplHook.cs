using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Record related record manage page custom implementation hooks")]
	public interface IRecordRelatedRecordManagePageCustomImplHook
	{
		IActionResult OnManageRecord(EntityRecord record, Entity entity, RecordRelatedRecordManagePageModel pageModel);
	}
}
