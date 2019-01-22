using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Logout page hooks")]
	public interface ILogoutPageHook
	{
		IActionResult OnGet(LogoutModel pageModel);
		IActionResult OnPost(LogoutModel pageModel);
	}
}
