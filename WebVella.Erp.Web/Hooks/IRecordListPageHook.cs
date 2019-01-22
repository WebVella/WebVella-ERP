using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Record list page hooks")]
	public interface IRecordListPageHook
	{
		IActionResult OnGet(RecordListPageModel pageModel);
		IActionResult OnPost(RecordListPageModel pageModel);
	}
}
