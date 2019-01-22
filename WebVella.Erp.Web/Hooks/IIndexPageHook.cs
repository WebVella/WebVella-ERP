using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Index page hooks")]
	public interface IHomePageHook
	{
		IActionResult OnGet(HomePageModel pageModel);
		IActionResult OnPost(HomePageModel pageModel);
	}
}
