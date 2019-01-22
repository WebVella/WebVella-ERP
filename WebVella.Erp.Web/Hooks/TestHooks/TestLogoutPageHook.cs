using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages;

namespace WebVella.Erp.Web.Hooks.TestHooks
{
	[HookAttachment]
	class TestLogoutPageHook : ILogoutPageHook
	{
		public IActionResult OnGet(LogoutModel pageModel)
		{
			return null;
		}
		public IActionResult OnPost(LogoutModel pageModel)
		{
			return null;
		}

	}
}
