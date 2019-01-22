using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Record manage page custom implementation hooks")]
	public interface IRecordManagePageCustomImplHook
	{
		IActionResult OnManageRecord(EntityRecord record, Entity entity, RecordManagePageModel pageModel);
	}
}
