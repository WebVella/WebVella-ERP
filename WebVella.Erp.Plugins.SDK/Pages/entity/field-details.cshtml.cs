using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
	public class FieldDetailsModel : BaseErpPageModel
	{
		public FieldDetailsModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public Entity ErpEntity { get; set; }

		public Field Field { get; set; }

		public EntityRecord FieldCard { get; set; } = null;

		public string CurrencyCode { get; set; } = "USD";

		public List<SelectOption> CurrencyOptions { get; set; } = new List<SelectOption>();

		[BindProperty]
		public string FieldPermissions { get; set; } = "[]";

		public List<SelectOption> RoleOptions { get; set; } = new List<SelectOption>();

		public List<SelectOption> PermissionOptions { get; set; } = new List<SelectOption>();

		public string ApiUrlFieldInlineEdit { get; set; } = "";

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public void InitPage()
		{
			var entMan = new EntityManager();
			ErpEntity = entMan.ReadEntity(ParentRecordId ?? Guid.Empty).Object;
			if (ErpEntity == null) return;

			Field = ErpEntity.Fields.FirstOrDefault(x => x.Id == RecordId);
			if (Field == null) return;

			var allCards = AdminPageUtils.GetFieldCards();

			FieldCard = allCards.First(x => (string)x["type"] == ((int)Field.GetFieldType()).ToString());


			#region << Init RecordPermissions >>
			var valueGrid = new List<KeyStringList>();
			PermissionOptions = new List<SelectOption>() {
							new SelectOption("read","read"),
							new SelectOption("update","update")
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
				if (Field.Permissions.CanRead.Contains(role.Id))
				{
					keyValuesObj.Values.Add("read");
				}
				if (Field.Permissions.CanUpdate.Contains(role.Id))
				{
					keyValuesObj.Values.Add("update");
				}
				valueGrid.Add(keyValuesObj);
			}
			FieldPermissions = JsonConvert.SerializeObject(valueGrid);
			#endregion

			#region << Actions >>
			if (Field.System)
			{
				HeaderActions.Add(PageUtils.GetActionTemplate(PageUtilsActionType.Disabled, label: "Delete Locked", formId: "DeleteRecord", iconClass: "fa fa-trash-alt", titleText: "System objects cannot be deleted"));
			}
			else
			{
				HeaderActions.Add(PageUtils.GetActionTemplate(PageUtilsActionType.ConfirmAndSubmitForm, label: "Delete Field", formId: "DeleteRecord", btnClass: "btn btn-white btn-sm", iconClass: "fa fa-trash-alt go-red"));
			};
			HeaderActions.Add($"<a href='/sdk/objects/entity/r/{(ErpEntity != null ? ErpEntity.Id : Guid.Empty)}/rl/fields/m/{Field.Id}' class='btn btn-white btn-sm'><i class='fa fa-cog go-orange'></i> Manage</a>");
			#endregion

			ApiUrlFieldInlineEdit = ErpSettings.ApiUrlTemplateFieldInlineEdit.Replace("{entityName}", ErpEntity.Name).Replace("{recordId}", (RecordId ?? Guid.Empty).ToString());

		}
		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			InitPage();

			if (ErpEntity == null || Field == null)
			{
				return NotFound();
			}

			if (String.IsNullOrWhiteSpace(ReturnUrl))
			{
				ReturnUrl = $"/sdk/objects/entity/r/{ErpEntity.Id}/rl/fields/l";
			}

			HeaderToolbar.AddRange(AdminPageUtils.GetEntityAdminSubNav(ErpEntity, "fields"));

			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}

		public IActionResult OnPost()
		{
			if (!ModelState.IsValid) throw new Exception("Antiforgery check failed.");

			var initResult = Init();
			if (initResult != null)
				return initResult;

			InitPage();

			if (ErpEntity == null || Field == null)
			{
				return NotFound();
			}

			if (String.IsNullOrWhiteSpace(ReturnUrl))
			{
				ReturnUrl = $"/sdk/objects/entity/r/{ErpEntity.Id}/rl/fields/l";
			}

			var entMan = new EntityManager();
			try
			{
				var response = new FieldResponse();

				response = entMan.DeleteField(ErpEntity.Id, Field.Id);
				if (!response.Success)
				{
					var exception = new ValidationException(response.Message);
					exception.Errors = response.Errors.MapTo<ValidationError>();
					throw exception;
				}
				return Redirect(ReturnUrl);
			}
			catch (ValidationException ex)
			{
				Validation.Message = ex.Message;
				Validation.Errors = ex.Errors;
			}
			catch (Exception ex)
			{
				Validation.Message = ex.Message;
				Validation.Errors.Add(new ValidationError("", ex.Message, isSystem: true));
			}

			HeaderToolbar.AddRange(AdminPageUtils.GetEntityAdminSubNav(ErpEntity, "fields"));

			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}
	}
}