using Microsoft.AspNetCore.Mvc;
using System;
using WebVella.Erp.Api;
using WebVella.Erp.Diagnostics;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Pages.Application
{
	public class RecordDetailsPageModel : BaseErpPageModel
	{
		public RecordDetailsPageModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }


		public IActionResult OnGet()
		{
			try
			{
				var initResult = Init();
				if (initResult != null) return initResult;
				if (ErpRequestContext.Page == null) return NotFound();
				if (!RecordsExists()) return NotFound();
				if (PageName != ErpRequestContext.Page.Name)
				{
					var queryString = HttpContext.Request.QueryString.ToString();
					return Redirect($"/{ErpRequestContext.App.Name}/{ErpRequestContext.SitemapArea.Name}/{ErpRequestContext.SitemapNode.Name}/r/{ErpRequestContext.RecordId}/{ErpRequestContext.Page.Name}{queryString}");
				}

				var globalHookInstances = HookManager.GetHookedInstances<IPageHook>(HookKey);
				foreach (IPageHook inst in globalHookInstances)
				{
					var result = inst.OnGet(this);
					if (result != null) return result;
				}

				BeforeRender();
				return Page();
			}
			catch (Exception ex)
			{
				new Log().Create(LogType.Error, "RecordDetailsPageModel Error on GET", ex);
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
				if (!RecordsExists()) return NotFound();

				var globalHookInstances = HookManager.GetHookedInstances<IPageHook>(HookKey);
				foreach (IPageHook inst in globalHookInstances)
				{
					var result = inst.OnPost(this);
					if (result != null) return result;
				}

				/// Standard Page Delete behavior
				if (HookKey == "delete" && ErpRequestContext.Entity != null && ErpRequestContext.Entity.Id != null && ErpRequestContext.RecordId != null)
				{
					var deleteRecordResponse = new RecordManager().DeleteRecord(ErpRequestContext.Entity, (ErpRequestContext.RecordId ?? Guid.Empty));
					if (deleteRecordResponse.Success)
						return Redirect($"/{ErpRequestContext.App.Name}/{ErpRequestContext.SitemapArea.Name}/{ErpRequestContext.SitemapNode.Name}/l/");
					else
					{
						Validation.Message = deleteRecordResponse.Message;
						foreach (var error in deleteRecordResponse.Errors)
							Validation.Errors.Add(new ValidationError(error.Key, error.Message));

						BeforeRender();
						return Page();
					}
				}

				var hookInstances = HookManager.GetHookedInstances<IRecordDetailsPageHook>(HookKey);
				foreach (IRecordDetailsPageHook inst in hookInstances)
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
				new Log().Create(LogType.Error, "RecordDetailsPageModel Error on POST", ex);
				Validation.Message = ex.Message;
				BeforeRender();
				return Page();
			}
		}
	}
}