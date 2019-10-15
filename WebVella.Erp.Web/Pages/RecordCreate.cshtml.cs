using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Diagnostics;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Web.Pages.Application
{
	public class RecordCreatePageModel : BaseErpPageModel
	{
		public RecordCreatePageModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public IActionResult OnGet()
		{
			try
			{
				var initResult = Init();
				if (initResult != null) return initResult;
				if (ErpRequestContext.Page == null) return NotFound();
				if (PageName != ErpRequestContext.Page.Name)
				{
					var queryString = HttpContext.Request.QueryString.ToString();
					return Redirect($"/{ErpRequestContext.App.Name}/{ErpRequestContext.SitemapArea.Name}/{ErpRequestContext.SitemapNode.Name}/c/{ErpRequestContext.Page.Name}{queryString}");
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
				new Log().Create(LogType.Error, "RecordCreatePageModel Error on GET", ex);
				Validation.Message = ex.Message;
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

				//Standard Page functionality
				var PostObject = (EntityRecord)new PageService().ConvertFormPostToEntityRecord(PageContext.HttpContext, entity: ErpRequestContext.Entity, recordId: RecordId);
				DataModel.SetRecord(PostObject);

				var globalHookInstances = HookManager.GetHookedInstances<IPageHook>(HookKey);
				foreach (IPageHook inst in globalHookInstances)
				{
					var result = inst.OnPost(this);
					if (result != null) return result;
				}


				if (!PostObject.Properties.ContainsKey("id"))
					PostObject["id"] = Guid.NewGuid();

				var hookInstances = HookManager.GetHookedInstances<IRecordCreatePageHook>(HookKey);

				//pre create hooks
				foreach (IRecordCreatePageHook inst in hookInstances)
				{
					List<ValidationError> errors = new List<ValidationError>();
					var result = inst.OnPreCreateRecord(PostObject, ErpRequestContext.Entity, this, errors);
					if (result != null) return result;
					if (errors.Any())
					{
						Validation.Errors.AddRange(errors);
						BeforeRender();
						return Page();
					}
				}

				//record submission validates required fields and auto number - these fields are validated in recordmanager
				ValidateRecordSubmission(PostObject, ErpRequestContext.Entity, Validation);
				if (Validation.Errors.Any())
				{
					BeforeRender();
					return Page();
				}

				var createResponse = new RecordManager().CreateRecord(ErpRequestContext.Entity.MapTo<Entity>(), PostObject);
				if (!createResponse.Success)
				{
					Validation.Message = createResponse.Message;
					foreach (var error in createResponse.Errors)
						Validation.Errors.Add(new ValidationError(error.Key, error.Message));

					ErpRequestContext.PageContext = PageContext;
					BeforeRender();
					return Page();
				}

				//post create hook
				foreach (IRecordCreatePageHook inst in hookInstances)
				{
					var result = inst.OnPostCreateRecord(PostObject, ErpRequestContext.Entity, this);
					if (result != null) return result;
				}

				if (string.IsNullOrWhiteSpace(ReturnUrl))
					return Redirect($"/{ErpRequestContext.App.Name}/{ErpRequestContext.SitemapArea.Name}/{ErpRequestContext.SitemapNode.Name}/r/{createResponse.Object.Data[0]["id"]}");
				else
					return Redirect(ReturnUrl);

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
				new Log().Create(LogType.Error, "RecordCreatePageModel Error on POST", ex);
				Validation.Message = ex.Message;
				BeforeRender();
				return Page();
			}
		}
	}
}

/*
 * system actions: 
 *	OnGet: none
 *	OnPost: PreCreate,PostCreate
 * custom actions: based on form handler
 */
