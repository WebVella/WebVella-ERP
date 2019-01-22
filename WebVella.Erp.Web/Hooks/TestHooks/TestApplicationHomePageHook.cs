using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks.TestHooks
{
	[HookAttachment]
	class TestApplicationHomePageHook : IApplicationHomePageHook
	{
		public IActionResult OnGet(ApplicationHomePageModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(ApplicationHomePageModel pageModel)
		{
			return null;
		}
	}
}
