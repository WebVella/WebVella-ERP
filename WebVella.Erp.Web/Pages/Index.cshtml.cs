using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using WebVella.Erp.Diagnostics;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Pages
{
	public class HomePageModel : BaseErpPageModel
	{
		public HomePageModel([FromServices] ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public IActionResult OnGet()
		{
			try
			{
				Debug.WriteLine("<><><><> ERP Index Start");
				var initResult = Init();
				Debug.WriteLine("<><><><> ERP Index Inited");
				if (initResult != null)
				{
					Debug.WriteLine("<><><><> ERP Index Inited With Result NULL - NOT FOUND");
					return initResult;
				}

				if (ErpRequestContext.Page == null) return NotFound();

				var globalHookInstances = HookManager.GetHookedInstances<IPageHook>(HookKey);
				foreach (IPageHook inst in globalHookInstances)
				{
					var result = inst.OnGet(this);
					if (result != null) return result;
				}

				var hookInstances = HookManager.GetHookedInstances<IHomePageHook>(HookKey);
				foreach (IHomePageHook inst in hookInstances)
				{
					var result = inst.OnGet(this);
					if (result != null) return result;
				}

				BeforeRender();
				return Page();
			}
			catch (Exception ex)
			{
				new Log().Create(LogType.Error, "HomePageModel Error on GET", ex);
				BeforeRender();
				return Page();
			}

		}

		public IActionResult OnPost()
		{
			try
			{
				if (!ModelState.IsValid) throw new Exception("Antiforgery check failed.");
				var initResult = Init();
				if (initResult != null) return initResult;
				if (ErpRequestContext.Page == null) return NotFound();

				var globalHookInstances = HookManager.GetHookedInstances<IPageHook>(HookKey);
				foreach (IPageHook inst in globalHookInstances)
				{
					var result = inst.OnPost(this);
					if (result != null) return result;
				}

				var hookInstances = HookManager.GetHookedInstances<IHomePageHook>(HookKey);
				foreach (IHomePageHook inst in hookInstances)
				{
					var result = inst.OnPost(this);
					if (result != null) return result;
				}

				BeforeRender();
				return Page();
			}
			catch (ValidationException valEx)
			{
				Validation.Message = valEx.Message;
				Validation.Errors.AddRange(valEx.Errors);
				BeforeRender();
				return Page();
			}
			catch (Exception ex)
			{
				new Log().Create(LogType.Error, "HomePageModel Error on POST", ex);
				BeforeRender();
				return Page();
			}
		}
	}
}

