using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Application home page hooks")]
	public interface IApplicationHomePageHook
	{
		IActionResult OnGet(ApplicationHomePageModel pageModel);
		IActionResult OnPost(ApplicationHomePageModel pageModel);
	}
}
