using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Record create page custom implementation hooks")]
	public interface IRecordCreatePageCustomImplHook
	{
		IActionResult OnCreateRecord(EntityRecord record, Entity entity, RecordCreatePageModel pageModel);
	}
}
