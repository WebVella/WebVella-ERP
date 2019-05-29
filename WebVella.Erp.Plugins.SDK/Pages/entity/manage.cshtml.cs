using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
	public class ManageModel : BaseErpPageModel
	{
		public ManageModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public Entity ErpEntity { get; set; }

		[BindProperty]
		public string Name { get; set; } = "";

		[BindProperty]
		public string Label { get; set; } = "";

		[BindProperty]
		public string LabelPlural { get; set; } = "";

		[BindProperty]
		public bool? System { get; set; } = false;

		[BindProperty]
		public string IconName { get; set; } = "";

		[BindProperty]
		public string Color { get; set; } = "";

		[BindProperty]
		public string RecordPermissions { get; set; } = "[]";

		[BindProperty]
		public Guid? RecordScreenIdField { get; set; } //If null the ID field of the record is used as ScreenId

		public List<SelectOption> RoleOptions { get; set; } = new List<SelectOption>();

		public List<SelectOption> PermissionOptions { get; set; } = new List<SelectOption>();

		public List<SelectOption> FieldOptions { get; set; } = new List<SelectOption>();

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public void PageInit()
		{
			#region << Init Entity >>
			var entMan = new EntityManager();
			ErpEntity = entMan.ReadEntity(RecordId ?? Guid.Empty).Object;
			if (ErpEntity != null && PageContext.HttpContext.Request.Method == "GET")
			{
				Name = ErpEntity.Name;
				Label = ErpEntity.Label;
				LabelPlural = ErpEntity.LabelPlural;
				System = ErpEntity.System;
				IconName = ErpEntity.IconName;
				Color = ErpEntity.Color;
				RecordScreenIdField = ErpEntity.RecordScreenIdField;

				foreach (var field in ErpEntity.Fields)
				{
					FieldOptions.Add(new SelectOption()
					{
						Value = field.Id.ToString(),
						Label = field.Name
					});
				}
			}
			#endregion

			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = $"/sdk/objects/entity/r/{ErpEntity.Id}/";


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
			if (HttpContext.Request.Method == "GET")
			{
				RecordPermissions = JsonConvert.SerializeObject(valueGrid);
			}

			#endregion

			#region << Actions >>
			HeaderActions.AddRange(new List<string>() {

				PageUtils.GetActionTemplate(PageUtilsActionType.SubmitForm, label: "Save Entity",formId:"ManageRecord"),
				PageUtils.GetActionTemplate(PageUtilsActionType.Cancel, returnUrl: ReturnUrl,btnClass:"btn btn-sm btn-outline-secondary ml-1")
			});

			HeaderToolbar.AddRange(AdminPageUtils.GetEntityAdminSubNav(ErpEntity, "details"));

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
			if (!ModelState.IsValid) throw new Exception("Antiforgery check failed.");

			var initResult = Init();
			if (initResult != null)
				return initResult;

			PageInit();

			if (ErpEntity == null)
			{
				return NotFound();
			}

			var entMan = new EntityManager();
			try
			{
				var input = new InputEntity()
				{
					Id = ErpEntity.Id,
					Name = Name,
					Label = Label,
					LabelPlural = LabelPlural,
					System = System,
					IconName = IconName,
					Color = Color,
					RecordScreenIdField = RecordScreenIdField
				};
				var recordPermissionsKeyValues = JsonConvert.DeserializeObject<List<KeyStringList>>(RecordPermissions);
				input.RecordPermissions = new RecordPermissions();
				input.RecordPermissions.CanCreate = new List<Guid>();
				input.RecordPermissions.CanRead = new List<Guid>();
				input.RecordPermissions.CanUpdate = new List<Guid>();
				input.RecordPermissions.CanDelete = new List<Guid>();

				foreach (var role in recordPermissionsKeyValues)
				{
					var roleId = Guid.Empty;
					if (Guid.TryParse(role.Key, out Guid result))
					{
						roleId = result;
					}
					if (roleId != Guid.Empty)
					{
						if (role.Values.Contains("create"))
						{
							input.RecordPermissions.CanCreate.Add(roleId);
						}
						if (role.Values.Contains("read"))
						{
							input.RecordPermissions.CanRead.Add(roleId);
						}
						if (role.Values.Contains("update"))
						{
							input.RecordPermissions.CanUpdate.Add(roleId);
						}
						if (role.Values.Contains("delete"))
						{
							input.RecordPermissions.CanDelete.Add(roleId);
						}
					}
				}

				var response = entMan.UpdateEntity(input);
				if (!response.Success)
				{
					var exception = new ValidationException(response.Message);
					exception.Errors = response.Errors.MapTo<ValidationError>();
					throw exception;
				}
				return Redirect($"/sdk/objects/entity/r/{ErpEntity.Id}/");
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

			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}


	}
}