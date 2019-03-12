using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.SDK.Utils;

namespace WebVella.Erp.Plugins.SDK.Pages.ErpEntity
{
	public class CreateModel : BaseErpPageModel
	{
		public CreateModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		[BindProperty]
		public Guid? Id { get; set; } = null;

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

		public List<SelectOption> FieldOptions { get; set; } = new List<SelectOption>();

		public List<SelectOption> RoleOptions { get; set; } = new List<SelectOption>();

		public List<SelectOption> PermissionOptions { get; set; } = new List<SelectOption>();

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public void InitPage()
		{
			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/objects/entity/l/list";


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

				valueGrid.Add(keyValuesObj);
			}

			#endregion

			#region << Actions >>
			HeaderActions.AddRange(new List<string>() {

				PageUtils.GetActionTemplate(PageUtilsActionType.SubmitForm, label: "Create Entity",formId:"CreateRecord", btnClass:"btn btn-green btn-sm", iconClass:"fa fa-plus"),
				PageUtils.GetActionTemplate(PageUtilsActionType.Cancel, returnUrl: ReturnUrl)
			});

			#endregion

		}



		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			InitPage();

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

			var entMan = new EntityManager();
			try
			{
				var entityId = Guid.NewGuid();
				if (Id != null)
					entityId = Id.Value;

				var input = new InputEntity()
				{
					Id = entityId,
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

				var response = entMan.CreateEntity(input);
				if (!response.Success)
				{
					var exception = new ValidationException(response.Message);
					exception.Errors = response.Errors.MapTo<ValidationError>();
					throw exception;
				}
				return Redirect($"/sdk/objects/entity/r/{input.Id}/");
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