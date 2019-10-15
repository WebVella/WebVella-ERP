using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using WebVella.Erp.Diagnostics;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Pages.Application
{
	public class ApplicationNodePageModel : BaseErpPageModel
	{
		public ApplicationNodePageModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		/// <summary>
		/// Handles HTTP get requests
		/// </summary>
		/// <returns></returns>
		public IActionResult OnGet()
		{
			try
			{
				var initResult = Init();
				if (initResult != null) return initResult;
				if (ErpRequestContext.Page == null) return NotFound();

				string hookKey = string.Empty;
				if (PageContext.HttpContext.Request.Query.ContainsKey("hookKey"))
					hookKey = HttpContext.Request.Query["hookKey"].ToString();

				var globalHookInstances = HookManager.GetHookedInstances<IPageHook>(hookKey);
				foreach (IPageHook inst in globalHookInstances)
				{
					var result = inst.OnGet(this);
					if (result != null) return result;
				}


				foreach (IApplicationNodePageHook inst in HookManager.GetHookedInstances<IApplicationNodePageHook>(hookKey))
				{
					var result = inst.OnGet(this);
					if (result != null)
						return result;
				}
				BeforeRender();
				return Page();
			}
			catch (Exception ex)
			{
				new Log().Create(LogType.Error, "ApplicationNodePageModel Error on GET", ex);
				Validation.Message = ex.Message;
				BeforeRender();
				return Page();
			}
		}

		/// <summary>
		/// Handles HTTP post requests
		/// </summary>
		/// <returns></returns>
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

				foreach (IApplicationNodePageHook inst in HookManager.GetHookedInstances<IApplicationNodePageHook>(HookKey))
				{
					var result = inst.OnPost(this);
					if (result != null)
						return result;
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
				new Log().Create(LogType.Error, "ApplicationNodePageModel Error on POST", ex);
				Validation.Message = ex.Message;
				BeforeRender();
				return Page();
			}
		}
	}
}