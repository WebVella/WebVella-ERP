using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Web.Hooks.TestHooks
{
	[HookAttachment]
	class TestApplicationNodePageHook : IApplicationNodePageHook
	{
		public IActionResult OnGet(ApplicationNodePageModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(ApplicationNodePageModel pageModel)
		{
			return null;
		}
	}
}
