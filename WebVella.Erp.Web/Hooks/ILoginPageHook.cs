using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Pages;

namespace WebVella.Erp.Web.Hooks
{
	[Hook("Login page hooks")]
	public interface ILoginPageHook
	{
		IActionResult OnPostPreLogin(LoginModel pageModel);
		IActionResult OnPostAfterLogin(ErpUser user, LoginModel pageModel);
	}
}
