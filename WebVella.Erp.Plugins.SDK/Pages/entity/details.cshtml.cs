using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.SDK.Pages.ErpEntity
{
	public class DetailsModel : BaseErpPageModel
	{
		public DetailsModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public Entity ErpEntity { get; set; }

		public string RecordScreenIdFieldName { get; set; } = "id";

		public List<SelectOption> RoleOptions { get; set; } = new List<SelectOption>();

		public List<SelectOption> PermissionOptions { get; set; } = new List<SelectOption>();

		[BindProperty]
		public string RecordPermissions { get; set; } = "[]";

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public void PageInit()
		{
			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/objects/entity/l/list";

			#region << Init Entity >>
			var entMan = new EntityManager();
			ErpEntity = entMan.ReadEntity(RecordId ?? Guid.Empty).Object;
			if (ErpEntity == null) return;

			if (ErpEntity.RecordScreenIdField != null) {

				var screenField = ErpEntity.Fields.FirstOrDefault(x => x.Id == ErpEntity.RecordScreenIdField);
				if (screenField != null) {
					RecordScreenIdFieldName = screenField.Name;
				}
			}

			#endregion

			#region << Init RecordPermissions >>
			var valueGrid = new List<KeyStringList>();
			PermissionOptions = new List<SelectOption>() {
							new SelectOption("create","create"),
							new SelectOption("read","read"),
							new SelectOption("update","update"),
							new SelectOption("delete","delete")
						};

			var roles = AdminPageUtils.GetUserRoles(); //Special order is applied

			foreach (var role in roles)
			{
				RoleOptions.Add(new SelectOption(role.Id.ToString(), role.Name));
				var keyValuesObj = new KeyStringList()
				{
					Key = role.Id.ToString(),
					Values = new List<string>()
				};
				if (ErpEntity.RecordPermissions.CanCreate.Contains(role.Id))
				{
					keyValuesObj.Values.Add("create");
				}
				if (ErpEntity.RecordPermissions.CanRead.Contains(role.Id))
				{
					keyValuesObj.Values.Add("read");
				}
				if (ErpEntity.RecordPermissions.CanUpdate.Contains(role.Id))
				{
					keyValuesObj.Values.Add("update");
				}
				if (ErpEntity.RecordPermissions.CanDelete.Contains(role.Id))
				{
					keyValuesObj.Values.Add("delete");
				}

				valueGrid.Add(keyValuesObj);
			}
			RecordPermissions = JsonConvert.SerializeObject(valueGrid);
			#endregion

			#region << Actions >>

			HeaderActions.Add($"<a href='/sdk/objects/entity/m/{(ErpEntity != null ? ErpEntity.Id : Guid.Empty)}/clone' class='btn btn-white btn-sm'><i class='fa fa-file go-gray'></i> Clone</a>");
			if (ErpEntity.System)
			{
				HeaderActions.Add(PageUtils.GetActionTemplate(PageUtilsActionType.Disabled, label: "Delete locked", formId: "DeleteRecord", btnClass:"btn btn-white btn-sm", iconClass:"fa fa-trash-alt", titleText:"System objects cannot be deleted"));
			}
			else
			{
				HeaderActions.Add(PageUtils.GetActionTemplate(PageUtilsActionType.ConfirmAndSubmitForm, label: "Delete Entity", formId: "DeleteRecord", btnClass: "btn btn-white btn-sm", iconClass:"fa fa-trash-alt go-red"));
			};
			HeaderActions.Add($"<a href='/sdk/objects/entity/m/{(ErpEntity != null ? ErpEntity.Id : Guid.Empty)}/manage?returnUrl={HttpUtility.UrlEncode(CurrentUrl)}' class='btn btn-white btn-sm'><i class='fa fa-cog go-orange'></i> Manage</a>");
			

			HeaderToolbar.AddRange( AdminPageUtils.GetEntityAdminSubNav(ErpEntity, "details"));

			#endregion

		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			PageInit();
			if (ErpEntity == null)
				return NotFound();

			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}

		public IActionResult OnPost()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			PageInit();
			if (ErpEntity == null)
				return NotFound();

			var entMan = new EntityManager();
			try
			{
				var response = entMan.DeleteEntity(ErpEntity.Id);
				if (!response.Success)
				{
					var exception = new ValidationException(response.Message);
					exception.Errors = response.Errors.MapTo<ValidationError>();
					throw exception;
				}
				if (!String.IsNullOrWhiteSpace(ReturnUrl))
				{
					return Redirect(ReturnUrl);
				}
				else
				{
					return Redirect($"/sdk/objects/entity/l/list");
				}
			}
			catch (ValidationException ex)
			{
				Validation.Message = ex.Message;
				Validation.Errors = ex.Errors;
			}


			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}


	}
}