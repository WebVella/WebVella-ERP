using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Record related record list page hooks")]
	public interface IRecordRelatedRecordsListPageHook
	{
		IActionResult OnGet(RecordRelatedRecordsListPageModel pageModel);
		IActionResult OnPost(RecordRelatedRecordsListPageModel pageModel);
	}
}
