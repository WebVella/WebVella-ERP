using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;


namespace WebVella.Erp.Web.Pages.Application
{
	[Authorize]
	public class RecordListPageModel : BaseErpPageModel
	{
		public RecordListPageModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public IActionResult OnGet()
		{
			Init();
			if (ErpRequestContext.Page == null) return NotFound();
			if (PageName != ErpRequestContext.Page.Name)
			{
				var queryString = HttpContext.Request.QueryString.ToString();
				return Redirect($"/{ErpRequestContext.App.Name}/{ErpRequestContext.SitemapArea.Name}/{ErpRequestContext.SitemapNode.Name}/l/{ErpRequestContext.Page.Name}{queryString}");
			}

			var globalHookInstances = HookManager.GetHookedInstances<IPageHook>(HookKey);
			foreach (IPageHook inst in globalHookInstances)
			{
				var result = inst.OnGet(this);
				if (result != null) return result;
			}

			var hookInstances = HookManager.GetHookedInstances<IRecordListPageHook>(HookKey);
			foreach (IRecordListPageHook inst in hookInstances)
			{
				var result = inst.OnGet(this);
				if (result != null) return result;
			}

			BeforeRender();
			return Page();
		}

		public IActionResult OnPost()
		{
			if (!ModelState.IsValid) throw new Exception("Antiforgery check failed.");
			Init();
			if (ErpRequestContext.Page == null) return NotFound();

			var globalHookInstances = HookManager.GetHookedInstances<IPageHook>(HookKey);
			foreach (IPageHook inst in globalHookInstances)
			{
				var result = inst.OnPost(this);
				if (result != null) return result;
			}

			var hookInstances = HookManager.GetHookedInstances<IRecordListPageHook>(HookKey);
			foreach (IRecordListPageHook inst in hookInstances)
			{
				var result = inst.OnPost(this);
				if (result != null) return result;
			}

			BeforeRender();
			return Page();
		}
	}
}