using Microsoft.AspNetCore.Mvc;
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
	public class RecordRelatedRecordManagePageModel : BaseErpPageModel
	{
		public RecordRelatedRecordManagePageModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

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
					return Redirect($"/{ErpRequestContext.App.Name}/{ErpRequestContext.SitemapArea.Name}/{ErpRequestContext.SitemapNode.Name}/r/{ErpRequestContext.ParentRecordId}/rl/{ErpRequestContext.RelationId}/m/{ErpRequestContext.RecordId}/{ErpRequestContext.Page.Name}{queryString}");
				}
				if (!RecordsExists()) return NotFound();

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
				new Log().Create(LogType.Error, "RecordRelatedRecordManagePageModel Error on GET", ex);
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
				if (!RecordsExists()) return NotFound();
				if (PageName != ErpRequestContext.Page.Name)
					return Redirect($"/{ErpRequestContext.App.Name}/{ErpRequestContext.SitemapArea.Name}/{ErpRequestContext.SitemapNode.Name}/r/{ErpRequestContext.ParentRecordId}/rl/{ErpRequestContext.RelationId}/m/{ErpRequestContext.Page.Name}");

				//Standard Page functionality
				var PostObject = new PageService().ConvertFormPostToEntityRecord(PageContext.HttpContext, entity: ErpRequestContext.Entity, recordId: RecordId);
				DataModel.SetRecord(PostObject);

				var globalHookInstances = HookManager.GetHookedInstances<IPageHook>(HookKey);
				foreach (IPageHook inst in globalHookInstances)
				{
					var result = inst.OnPost(this);
					if (result != null) return result;
				}

				//record submission validates required fields and auto number - these fields are validated in recordmanager
				//ValidateRecordSubmission(PostObject, ErpRequestContext.Entity, Validation);
				if (Validation.Errors.Count == 0)
				{

					if (!PostObject.Properties.ContainsKey("id"))
						PostObject["id"] = RecordId.Value;

					var hookInstances = HookManager.GetHookedInstances<IRecordRelatedRecordManagePageHook>(HookKey);

					//pre manage hooks
					foreach (IRecordRelatedRecordManagePageHook inst in hookInstances)
					{
						List<ValidationError> errors = new List<ValidationError>();
						var result = inst.OnPreManageRecord(PostObject, ErpRequestContext.Entity, this, errors);
						if (result != null) return result;
						if (errors.Any())
						{
							Validation.Errors.AddRange(errors);

							BeforeRender();
							return Page();
						}
					}

					var updateResponse = new RecordManager().UpdateRecord(ErpRequestContext.Entity.MapTo<Entity>(), PostObject);
					if (!updateResponse.Success)
					{
						Validation.Message = updateResponse.Message;
						foreach (var error in updateResponse.Errors)
							Validation.Errors.Add(new ValidationError(error.Key, error.Message));

						ErpRequestContext.PageContext = PageContext;
						BeforeRender();
						return Page();
					}

					//post manage hook
					foreach (IRecordRelatedRecordManagePageHook inst in hookInstances)
					{
						var result = inst.OnPostManageRecord(PostObject, ErpRequestContext.Entity, this);
						if (result != null) return result;
					}

					if (string.IsNullOrWhiteSpace(ReturnUrl))
						return Redirect($"/{ErpRequestContext.App.Name}/{ErpRequestContext.SitemapArea.Name}/{ErpRequestContext.SitemapNode.Name}/r/{ErpRequestContext.ParentRecordId}/rl/{ErpRequestContext.RelationId}/r/{RecordId}");
					else
						return Redirect(ReturnUrl);


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
				new Log().Create(LogType.Error, "RecordRelatedRecordManagePageModel Error on POST", ex);
				Validation.Message = ex.Message;
				BeforeRender();
				return Page();
			}
		}
	}
}