using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Site page hooks")]
	public interface ISitePageHook
	{
		IActionResult OnGet(SitePageModel pageModel);
		IActionResult OnPost(SitePageModel pageModel);
	}
}
