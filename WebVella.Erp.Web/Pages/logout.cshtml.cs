using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Web.Pages
{
	public class LogoutModel : BaseErpPageModel
	{
		public LogoutModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public IActionResult OnGet([FromServices]AuthService authService)
        {
			var initResult = Init();
			if (initResult != null) return initResult;
			authService.Logout();

			var globalHookInstances = HookManager.GetHookedInstances<IPageHook>(HookKey);
			foreach (IPageHook inst in globalHookInstances)
			{
				var result = inst.OnGet(this);
				if (result != null) return result;
			}

			var hookInstances = HookManager.GetHookedInstances<ILogoutPageHook>(HookKey);
			foreach (ILogoutPageHook inst in hookInstances)
			{
				var result = inst.OnGet(this);
				if (result != null) return result;
			}

			return new LocalRedirectResult("/");
		}

		public IActionResult OnPost([FromServices]AuthService authService)
		{
			var initResult = Init();
			if (initResult != null) return initResult;
			authService.Logout();

			var globalHookInstances = HookManager.GetHookedInstances<IPageHook>(HookKey);
			foreach (IPageHook inst in globalHookInstances)
			{
				var result = inst.OnPost(this);
				if (result != null) return result;
			}

			var hookInstances = HookManager.GetHookedInstances<ILogoutPageHook>(HookKey);
			foreach (ILogoutPageHook inst in hookInstances)
			{
				var result = inst.OnPost(this);
				if (result != null) return result;
			}

			return new LocalRedirectResult("/");
		}
	}
}