using Microsoft.AspNetCore.Mvc;
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
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.SDK.Pages.ErpEntity
{
	public class DataManageModel : BaseErpPageModel
	{
		public DataManageModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public Entity ErpEntity { get; set; }

		public EntityRecord Record { get; set; }

		public List<Field> Fields { get; set; } = new List<Field>();

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public void PageInit()
		{
			var entMan = new EntityManager();
			var recMan = new RecordManager();

			ErpEntity = entMan.ReadEntity(ParentRecordId ?? Guid.Empty).Object;
			if (ErpEntity == null) return;

			if (!SecurityContext.HasEntityPermission(EntityPermission.Update, ErpEntity))
			{
				ErpEntity = null;
				return;
			}

			Fields = ErpEntity.Fields.OrderBy(x => x.Name).ToList();
			var idField = Fields.Single(x => x.Name == "id");
			Fields.Remove(idField);
			Fields = Fields.OrderBy(x => x.Name).ToList();
			Fields.Insert(0, idField);

			EntityQuery enQuery = new EntityQuery(ErpEntity.Name, "*", EntityQuery.QueryEQ("id", RecordId));
			var response = recMan.Find(enQuery);
			if (!response.Success)
				throw new Exception(response.Message);

			if (response.Object.Data.Count == 0)
				return;

			Record = response.Object.Data[0];

			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = $"/sdk/objects/entity/r/{ErpEntity.Id}/rl/data/l/";

			HeaderActions.AddRange(new List<string>() {

				PageUtils.GetActionTemplate(PageUtilsActionType.SubmitForm, label: "Save Record",formId:"ManageRecord"),
				PageUtils.GetActionTemplate(PageUtilsActionType.Cancel, returnUrl: $"/sdk/objects/entity/r/{ErpEntity.Id}/rl/data/l/" )
			});

			HeaderToolbar.AddRange(AdminPageUtils.GetEntityAdminSubNav(ErpEntity, "data"));
		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			PageInit();
			if (Record == null)
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
			if (Record == null)
				return NotFound();

			try
			{
				Record = (EntityRecord)new PageService().ConvertFormPostToEntityRecord(PageContext.HttpContext, RecordId, ErpEntity);

				//clear empty password properties from record, only update if new password is set
				//this is because browsers don't render password inputs and don't submit any value, if no user input
				Fields.Where(x => x.GetFieldType() == FieldType.PasswordField && string.IsNullOrWhiteSpace((string)Record[x.Name]))
						.ToList()
						.ForEach(x => Record.Properties.Remove(x.Name));

				var response = new RecordManager().UpdateRecord(ErpEntity, Record);
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

			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}

		public WvFieldAccess GetFieldAccess(Field entityField)
		{
			var canRead = false;
			var canUpdate = false;

			if (entityField.EnableSecurity)
			{
				if (CurrentUser != null)
				{
					foreach (var role in CurrentUser.Roles)
					{
						if (entityField.Permissions.CanRead.Any(x => x == role.Id))
							canRead = true;
						if (entityField.Permissions.CanUpdate.Any(x => x == role.Id))
							canUpdate = true;
					}
				}
				else
				{
					canRead = entityField.Permissions.CanRead.Any(x => x == SystemIds.GuestRoleId);
					canUpdate = entityField.Permissions.CanRead.Any(x => x == SystemIds.GuestRoleId);
				}
			}
			else
				return WvFieldAccess.Full;


			if (canUpdate)
				return WvFieldAccess.Full;
			else if (canRead)
				return WvFieldAccess.ReadOnly;
			else
				return WvFieldAccess.Forbidden;
		}

	}
}
