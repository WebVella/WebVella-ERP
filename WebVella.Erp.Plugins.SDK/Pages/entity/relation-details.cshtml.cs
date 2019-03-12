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
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.SDK.Pages.ErpEntity
{
	public class RelationDetailsModel : BaseErpPageModel
	{
		public RelationDetailsModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public Entity ErpEntity { get; set; }

		public EntityRelation Relation { get; set; }

		public List<SelectOption> TypeOptions { get; set; } = new List<SelectOption>();

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public void PageInit()
		{
			#region << Init Entity >>
			var entMan = new EntityManager();
			var relMan = new EntityRelationManager();

			ErpEntity = entMan.ReadEntity(ParentRecordId ?? Guid.Empty).Object;
			if (ErpEntity == null) return;

			var entityRelations = relMan.Read().Object.Where(x => x.TargetEntityId == ErpEntity.Id || x.OriginEntityId == ErpEntity.Id).ToList();
			Relation = entityRelations.SingleOrDefault(x => x.Id == RecordId);
			if (Relation == null)
				return;

			#endregion

			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = $"/sdk/objects/entity/r/{ErpEntity.Id}/rl/relations/l";

			#region << Actions >>

			if (Relation.System)
			{
				HeaderActions.Add(PageUtils.GetActionTemplate(PageUtilsActionType.Disabled, label: "Delete locked", formId: "DeleteRecord", btnClass: "btn btn-white btn-sm", iconClass: "fa fa-trash-alt", titleText: "System objects cannot be deleted"));
			}
			else
			{
				HeaderActions.Add(PageUtils.GetActionTemplate(PageUtilsActionType.ConfirmAndSubmitForm, label: "Delete Relation", formId: "DeleteRecord", btnClass: "btn btn-white btn-sm", iconClass: "fa fa-trash-alt go-red"));
			};

			HeaderActions.Add($"<a href='/sdk/objects/entity/r/{ErpEntity.Id}/rl/relations/m/{RecordId}' class='btn btn-white btn-sm'><i class='fa fa-cog go-orange'></i> Manage Relation</a>");

			HeaderToolbar.AddRange(AdminPageUtils.GetEntityAdminSubNav(ErpEntity, "relations"));

			#endregion

			TypeOptions = ModelExtensions.GetEnumAsSelectOptions<EntityRelationType>().FindAll(x => x.Value != "1").ToList();
		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			PageInit();
			if (Relation == null)
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
			if (Relation == null)
				return NotFound();

			var entMan = new EntityManager();
			try
			{
				var relMan = new EntityRelationManager();
				var response = relMan.Delete(Relation.Id);
				if (!response.Success)
				{
					var exception = new ValidationException(response.Message);
					exception.Errors = response.Errors.MapTo<ValidationError>();
					throw exception;
				}
				return Redirect($"/sdk/objects/entity/r/{ErpEntity.Id}/rl/relations/l");
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
