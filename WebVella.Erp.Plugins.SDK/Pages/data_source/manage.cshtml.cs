using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.SDK.Pages.ErpDataSource
{
	public class ManageModel : BaseErpPageModel
	{
		public ManageModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		private DataSourceManager dsMan = new DataSourceManager();

		public DatabaseDataSource DataSourceObject { get; set; }

		[BindProperty(Name="name")]
		public string Name { get; set; }

		[BindProperty(Name = "eql_input")]
		public string EqlInput { get; set; }

		[BindProperty(Name = "description")]
		public string Description { get; set; }

		[BindProperty(Name = "weight")]
		public int Weight { get; set; }

		[BindProperty(Name = "param_defaults")]
		public string ParamDefaults { get; set; }
		
		[BindProperty(Name = "model")]
		public string ResultModel { get; set; }

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public void InitPage()
		{
			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/objects/data_source/l/list";

			DataSourceObject = dsMan.Get(RecordId ?? Guid.Empty) as DatabaseDataSource;
			if (DataSourceObject == null)
				return;

			HeaderActions.AddRange(new List<string>() {

				PageUtils.GetActionTemplate(PageUtilsActionType.SubmitForm, label: "Save",formId:"UpdateRecord", btnClass:"btn btn-primary btn-sm", iconClass:"fa fa-save"),
				PageUtils.GetActionTemplate(PageUtilsActionType.Cancel, returnUrl: ReturnUrl)
			});

		}


		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			InitPage();

			if (DataSourceObject == null)
				return NotFound();

			Name = DataSourceObject.Name;
			Description = DataSourceObject.Description;
			EqlInput = DataSourceObject.EqlText;
			Weight = DataSourceObject.Weight;
			ResultModel = DataSourceObject.ResultModel;
			ParamDefaults = string.Empty;
			foreach (var par in DataSourceObject.Parameters)
			{
				if (ParamDefaults != string.Empty)
					ParamDefaults += Environment.NewLine;

				ParamDefaults += $"{par.Name},{par.Type},{par.Value}";
			}

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

			if (DataSourceObject == null)
				return NotFound();

			var entMan = new EntityManager();
			try
			{
				try
				{
					dsMan.Update(DataSourceObject.Id, Name, Description, Weight, EqlInput, ParamDefaults);
				}
				catch (EqlException eqlEx)
				{
					ValidationException valEx = new ValidationException(eqlEx.Message, eqlEx);
					foreach (var err in eqlEx.Errors)
						valEx.Errors.Add(new ValidationError("", err.Message));
					throw valEx;
				}

				return Redirect($"/sdk/objects/data_source/r/{RecordId}/");
			}
			catch (ValidationException ex)
			{
				Validation.Message = ex.Message;
				Validation.Errors = ex.Errors;
			}
			catch (Exception ex)
			{
				Validation.Message = ex.Message;
			}

			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}

	}
}