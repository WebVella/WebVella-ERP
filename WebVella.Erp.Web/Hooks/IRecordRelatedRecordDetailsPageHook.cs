using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Related record details page hooks")]
	public interface IRecordRelatedRecordDetailsPageHook
	{
		IActionResult OnPost(RecordRelatedRecordDetailsPageModel pageModel);
	}
}
