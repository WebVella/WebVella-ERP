using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages;

namespace WebVella.Erp.Web.Hooks.TestHooks
{
	[HookAttachment]
	class TestLoginPageHook : ILoginPageHook
	{
		public IActionResult OnPostAfterLogin(ErpUser user, LoginModel pageModel)
		{
			return null;
		}

		public IActionResult OnPostPreLogin(LoginModel pageModel)
		{
			return null;
		}
	}
}
