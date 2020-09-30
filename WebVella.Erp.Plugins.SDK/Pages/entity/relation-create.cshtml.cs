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
	public class RelationCreateModel : BaseErpPageModel
	{
		public RelationCreateModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public Entity ErpEntity { get; set; }

		[BindProperty]
		public Guid? Id { get; set; } = null;

		[BindProperty]
		public string Name { get; set; } = "";

		[BindProperty]
		public string Label { get; set; } = "";

		[BindProperty]
		public string Description { get; set; } = "";

		[BindProperty]
		public bool IsSystem { get; set; }

		[BindProperty]
		public string Origin { get; set; } = "";

		public List<SelectOption> OriginOptions { get; set; } = new List<SelectOption>();

		[BindProperty]
		public string Target { get; set; } = "";

		public List<SelectOption> TargetOptions { get; set; } = new List<SelectOption>();

		[BindProperty]
		public EntityRelationType Type { get; set; } = EntityRelationType.OneToMany;

		public List<SelectOption> TypeOptions { get; set; } = new List<SelectOption>();

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public void PageInit()
		{
			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = $"/sdk/objects/entity/r/{ErpEntity.Id}/rl/relations/l";

			var entMan = new EntityManager();
			var relMan = new EntityRelationManager();
			var allRelations = relMan.Read().Object;

			foreach (var entity in entMan.ReadEntities().Object)
				foreach (var field in entity.Fields)
				{
					if (field is GuidField)
					{
						var sfo = new SelectOption($"{entity.Id}${field.Id}", $"{entity.Name}.{field.Name}");
						if (field.Unique && field.Required)
							OriginOptions.Add(sfo);

						if (!allRelations.Any(x => x.TargetFieldId == field.Id && x.RelationType != EntityRelationType.ManyToMany))
							TargetOptions.Add(sfo);
					}
				}

			ErpEntity = entMan.ReadEntity(ParentRecordId ?? Guid.Empty).Object;
			if (ErpEntity == null) return;

			HeaderActions.AddRange(new List<string>() {

				PageUtils.GetActionTemplate(PageUtilsActionType.SubmitForm, label: "Create Relation",formId:"CreateRecord"),
				PageUtils.GetActionTemplate(PageUtilsActionType.Cancel, returnUrl: $"/sdk/objects/entity/r/{ErpEntity.Id}/rl/relations/l" )
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

			try
			{
				if (String.IsNullOrWhiteSpace(Origin) || !Origin.Contains("$"))
				{
					throw new ValidationException("Origin field is required!");
				}
				if (String.IsNullOrWhiteSpace(Target) || !Target.Contains("$"))
				{
					throw new ValidationException("Target field is required!");
				}
				var originSections = Origin.Split('$');
				Guid originEntityId = new Guid(originSections[0]);
				Guid originFieldId = new Guid(originSections[1]);

				var targetSections = Target.Split('$');
				Guid targetEntityId = new Guid(targetSections[0]);
				Guid targetFieldId = new Guid(targetSections[1]);

				var relationId = Guid.NewGuid();
				if (Id != null) {
					relationId = Id.Value;
				}

				var relMan = new EntityRelationManager();
				EntityRelation newRelation = new EntityRelation
				{
					Id = relationId,
					Name = Name?.Trim(),
					Label = Name?.Trim(), //Label, Boz: removed for convinience
					Description = "", //Description, Boz: removed for convinience
					System = IsSystem,
					OriginEntityId = originEntityId,
					OriginFieldId = originFieldId,
					TargetEntityId = targetEntityId,
					TargetFieldId = targetFieldId,
					RelationType = Type
				};


				var response = relMan.Create(newRelation);
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
