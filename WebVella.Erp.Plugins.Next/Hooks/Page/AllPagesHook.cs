using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment]
	public class AllPagesHook : IPageHook
	{
		public IActionResult OnGet(BaseErpPageModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(BaseErpPageModel pageModel)
		{
			return null;
		}
	}
}
