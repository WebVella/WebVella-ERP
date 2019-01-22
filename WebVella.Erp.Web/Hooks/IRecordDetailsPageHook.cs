using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Record details page hooks")]
	public interface IRecordDetailsPageHook
	{
		IActionResult OnPost(RecordDetailsPageModel pageModel);
	}
}
