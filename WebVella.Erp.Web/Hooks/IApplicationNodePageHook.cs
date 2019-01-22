using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Application node page hooks")]
	public interface IApplicationNodePageHook
	{
		IActionResult OnPost(ApplicationNodePageModel pageModel);
		IActionResult OnGet(ApplicationNodePageModel pageModel);
	}
}
