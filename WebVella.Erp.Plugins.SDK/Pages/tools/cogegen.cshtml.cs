using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.SDK.Services;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.SDK.Pages.Tools
{
	public class CodeGenModel : BaseErpPageModel
	{
		public CodeGenModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public List<WvGridColumnMeta> Columns { get; set; } = new List<WvGridColumnMeta>() { 
			new WvGridColumnMeta() { Label = "Element", Name = "", Width = "1%" },
			new WvGridColumnMeta() { Label = "Change", Name = "", Width = "1%" },
			new WvGridColumnMeta() { Label = "Name", Name = "", Width = "1%" },
			new WvGridColumnMeta() { Label = "description", Name = "", Width = "90%" }
		};

		public List<MetaChangeModel> Changes { get; set; } = new List<MetaChangeModel>();

		public string Code { get; set; } = string.Empty;

		public bool ShowResults { get; set; } = false;

		[BindProperty]
		public string ConnectionString { get; set; }

		[BindProperty]
		public bool IncludeEntityMeta { get; set; } = true;

		[BindProperty]
		public bool IncludeEntityRelations { get; set; } = true;

		[BindProperty]
		public bool IncludeUserRoles { get; set; } = true;

		[BindProperty]
		public bool IncludeApplications { get; set; } = true;

		[BindProperty]
		public List<string> IncludeRecordsEntityIdList { get; set; } = new List<string>();

		public List<SelectOption> EntitySelectOptions { get; set; } = new List<SelectOption>();

		[BindProperty]
		public List<string> IncludeNNRelationIdList { get; set; } = new List<string>();

		public List<SelectOption> NNRelationsSelectOptions { get; set; } = new List<SelectOption>();

		private void InitEntitySelectOptions()
		{
			{
				var entities = new EntityManager().ReadEntities().Object;
				entities = entities.OrderBy(x => x.Name).ToList();
				foreach (var entity in entities)
				{
					EntitySelectOptions.Add(new SelectOption(entity.Id.ToString(), entity.Name));
				}
			}
			{
				var relations = new EntityRelationManager().Read().Object;
				relations = relations.FindAll(x=> x.RelationType == EntityRelationType.ManyToMany).OrderBy(x => x.Name).ToList();
				foreach (var relation in relations)
				{
					NNRelationsSelectOptions.Add(new SelectOption(relation.Id.ToString(), relation.Name));
				}
			}
		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			InitEntitySelectOptions();

			BeforeRender();
			return Page();
		}

		public IActionResult OnPost()
		{
			if (!ModelState.IsValid) throw new Exception("Antiforgery check failed.");
			
			var initResult = Init();
			if (initResult != null)
				return initResult;
			
			InitEntitySelectOptions();

			try
			{
				ValidationException valEx = new ValidationException();
				if( string.IsNullOrWhiteSpace(ConnectionString) )
				{
					valEx.AddError("ConnectionString", "Original database connection string is required.");
					ShowResults = false;
					throw valEx;
				}

				var conString = ConnectionString;
				if( !ConnectionString.Contains(";"))
				{
					NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(ErpSettings.ConnectionString);
					builder.Database = ConnectionString;
					conString = builder.ToString();
				}	
				
				var cgService = new CodeGenService();
				var result = cgService.EvaluateMetaChanges(conString, IncludeRecordsEntityIdList, IncludeEntityMeta, IncludeEntityRelations, IncludeUserRoles, IncludeApplications, IncludeNNRelationIdList);
				Code = result.Code;
				Changes = result.Changes;
				ShowResults = true;
			}
			catch (ValidationException valEx)
			{
				Validation.Message = valEx.Message;
				Validation.Errors.AddRange(valEx.Errors);
				ShowResults = false;
			}
			catch (Exception ex)
			{
				Validation.Message = ex.Message;
				Validation.AddError("", ex.Message );
				ShowResults = false;
			}
			BeforeRender();
			return Page();
		}
	}
}
