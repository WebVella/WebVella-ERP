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
	public class RelationManageModel : BaseErpPageModel
	{
		public RelationManageModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public Entity ErpEntity { get; set; }

		public EntityRelation Relation { get; set; }

		[BindProperty]
		public string Label { get; set; } = "";

		[BindProperty]
		public string Description { get; set; } = "";

		[BindProperty]
		public bool IsSystem { get; set; }

		public List<SelectOption> TypeOptions { get; set; } = new List<SelectOption>();

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public void PageInit()
		{
			var entMan = new EntityManager();
			var relMan = new EntityRelationManager();

			ErpEntity = entMan.ReadEntity(ParentRecordId ?? Guid.Empty).Object;
			if (ErpEntity == null) return;

			var entityRelations = relMan.Read().Object.Where(x => x.TargetEntityId == ErpEntity.Id || x.OriginEntityId == ErpEntity.Id).ToList();
			Relation = entityRelations.SingleOrDefault(x => x.Id == RecordId);
			if (Relation == null)
				return;

			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = $"/sdk/objects/entity/r/{ErpEntity.Id}/rl/relations/r/{Relation.Id}/";

			HeaderActions.AddRange(new List<string>() {

				PageUtils.GetActionTemplate(PageUtilsActionType.SubmitForm, label: "Save Relation",formId:"ManageRecord"),
				PageUtils.GetActionTemplate(PageUtilsActionType.Cancel, returnUrl: $"/sdk/objects/entity/r/{ErpEntity.Id}/rl/relations/r/{Relation.Id}/" )
			});

			HeaderToolbar.AddRange(AdminPageUtils.GetEntityAdminSubNav(ErpEntity, "relations"));

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

			Label = Relation.Label;
			Description = Relation.Description;
			IsSystem = Relation.System;

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

			try
			{
				var relMan = new EntityRelationManager();
				EntityRelation updRelation = new EntityRelation
				{
					Id = Relation.Id,
					Name = Relation.Name,
					Label = Relation.Name, //Label, Boz: removed for convinience
					Description = "",//Description, Boz: removed for convinience
					System = IsSystem,
					OriginEntityId = Relation.OriginEntityId,
					OriginEntityName = Relation.OriginEntityName,
					OriginFieldId = Relation.OriginFieldId,
					OriginFieldName = Relation.OriginFieldName,
					TargetEntityId = Relation.TargetEntityId,
					TargetEntityName = Relation.TargetEntityName,
					TargetFieldId = Relation.TargetFieldId,
					TargetFieldName = Relation.TargetFieldName,
					RelationType = Relation.RelationType
				};


				var response = relMan.Update(updRelation);
				if (!response.Success)
				{
					var exception = new ValidationException(response.Message);
					exception.Errors = response.Errors.MapTo<ValidationError>();
					throw exception;
				}
				return Redirect($"/sdk/objects/entity/r/{ErpEntity.Id}/rl/relations/r/{Relation.Id}/");
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
