using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Global page hooks")]
	public interface IPageHook
	{
		IActionResult OnGet(BaseErpPageModel pageModel);
		IActionResult OnPost(BaseErpPageModel pageModel);
	}
}
