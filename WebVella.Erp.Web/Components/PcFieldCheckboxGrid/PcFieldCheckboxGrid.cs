using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Web.Components
{
	[PageComponent(Label = "Field Checkbox Grid", Library = "WebVella", Description = "Multiple checkboxes in a grid format", Version = "0.0.1", IconClass = "fas fa-grip-horizontal")]
	public class PcFieldCheckboxGrid : PcFieldBase
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcFieldCheckboxGrid([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcFieldCheckboxGridOptions : PcFieldBaseOptions
		{
			[JsonProperty(PropertyName = "text_true")]
			public string TextTrue { get; set; } = "selected";

			[JsonProperty(PropertyName = "text_false")]
			public string TextFalse { get; set; } = "not selected";

			[JsonProperty(PropertyName = "rows")]
			public string Rows { get; set; } = "";

			[JsonProperty(PropertyName = "columns")]
			public string Columns { get; set; } = "";

			public static PcFieldCheckboxGridOptions CopyFromBaseOptions(PcFieldBaseOptions input)
			{
				return new PcFieldCheckboxGridOptions
				{
					IsVisible = input.IsVisible,
					LabelMode = input.LabelMode,
					LabelText = input.LabelText,
					Mode = input.Mode,
					Name = input.Name,
					TextTrue = "",
					TextFalse = ""
				};
			}
		}

		public async Task<IViewComponentResult> InvokeAsync(PageComponentContext context)
		{
			ErpPage currentPage = null;
			try
			{
				#region << Init >>
				if (context.Node == null)
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: The node Id is required to be set as query parameter 'nid', when requesting this component"));
				}

				var pageFromModel = context.DataModel.GetProperty("Page");
				if (pageFromModel == null)
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: PageModel cannot be null"));
				}
				else if (pageFromModel is ErpPage)
				{
					currentPage = (ErpPage)pageFromModel;
				}
				else
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: PageModel does not have Page property or it is not from ErpPage Type"));
				}

				var baseOptions = InitPcFieldBaseOptions(context);
				var options = PcFieldCheckboxGridOptions.CopyFromBaseOptions(baseOptions);

				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcFieldCheckboxGridOptions>(context.Options.ToString());
				}
				var modelFieldLabel = "";
				var model = (PcFieldCheckboxGridModel)InitPcFieldBaseModel(context, options, label: out modelFieldLabel, targetModel: "PcFieldCheckboxGridModel");
				if (String.IsNullOrWhiteSpace(options.LabelText) && context.Mode != ComponentMode.Options)
				{
					options.LabelText = modelFieldLabel;
				}

				//PcFieldCheckboxGridModel model = PcFieldCheckboxGridModel.CopyFromBaseModel(baseModel);

				//Implementing Inherit label mode
				ViewBag.LabelMode = options.LabelMode;
				ViewBag.Mode = options.Mode;

				if (options.LabelMode == WvLabelRenderMode.Undefined && baseOptions.LabelMode != WvLabelRenderMode.Undefined)
					ViewBag.LabelMode = baseOptions.LabelMode;

				if (options.Mode == WvFieldRenderMode.Undefined && baseOptions.Mode != WvFieldRenderMode.Undefined)
					ViewBag.Mode = baseOptions.Mode;


				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);

				var accessOverride = context.DataModel.GetPropertyValueByDataSource(options.AccessOverrideDs) as WvFieldAccess?;
				if(accessOverride != null){
					model.Access = accessOverride.Value;
				}
				var requiredOverride = context.DataModel.GetPropertyValueByDataSource(options.RequiredOverrideDs) as bool?;
				if(requiredOverride != null){
					model.Required = requiredOverride.Value;
				}
				else{
					if(!String.IsNullOrWhiteSpace(options.RequiredOverrideDs)){
						if(options.RequiredOverrideDs.ToLowerInvariant() == "true"){
							model.Required = true;
						}
						else if(options.RequiredOverrideDs.ToLowerInvariant() == "false"){
							model.Required = false;
						}
					}
				}
				#endregion



				ViewBag.Options = options;
				ViewBag.Model = model;
				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;

				if (context.Mode != ComponentMode.Options && context.Mode != ComponentMode.Help)
				{
					var isVisible = true;
					var isVisibleDS = context.DataModel.GetPropertyValueByDataSource(options.IsVisible);
					if (isVisibleDS is string && !String.IsNullOrWhiteSpace(isVisibleDS.ToString()))
					{
						if (Boolean.TryParse(isVisibleDS.ToString(), out bool outBool))
						{
							isVisible = outBool;
						}
					}
					else if (isVisibleDS is Boolean)
					{
						isVisible = (bool)isVisibleDS;
					}
					ViewBag.IsVisible = isVisible;

					#region << Init DataSources >>

					dynamic valueResult = context.DataModel.GetPropertyValueByDataSource(options.Value);
					if (valueResult == null)
					{
						model.Value = new List<KeyStringList>();
					}
					else if (valueResult is List<KeyStringList>)
					{
						model.Value = (List<KeyStringList>)valueResult;
					}
					else if (valueResult is string)
					{
						var stringProcessed = false;
						if (String.IsNullOrWhiteSpace(valueResult))
						{
							model.Value = new List<KeyStringList>();
							stringProcessed = true;
						}
						if (!stringProcessed && (((string)valueResult).StartsWith("{") || ((string)valueResult).StartsWith("[")))
						{
							try
							{
								model.Value = JsonConvert.DeserializeObject<List<KeyStringList>>(valueResult.ToString());
								stringProcessed = true;
							}
							catch
							{
								stringProcessed = false;
								ViewBag.ExceptionMessage = "Value Json Deserialization failed!";
								ViewBag.Errors = new List<ValidationError>();
								return await Task.FromResult<IViewComponentResult>(View("Error"));
							}
						}
					}
					{
						dynamic rowsOptions = context.DataModel.GetPropertyValueByDataSource(options.Rows);
						if (rowsOptions == null) { }
						if (rowsOptions is List<SelectOption>)
						{
							model.Rows = (List<SelectOption>)rowsOptions;
						}
						else if (rowsOptions is string)
						{
							var stringProcessed = false;
							if (String.IsNullOrWhiteSpace(rowsOptions))
							{
								model.Rows = new List<SelectOption>();
								stringProcessed = true;
							}
							if (!stringProcessed && (((string)rowsOptions).StartsWith("{") || ((string)rowsOptions).StartsWith("[")))
							{
								try
								{
									model.Rows = JsonConvert.DeserializeObject<List<SelectOption>>(rowsOptions);
									stringProcessed = true;
								}
								catch
								{
									stringProcessed = false;
									ViewBag.ExceptionMessage = "Rows Json Deserialization failed!";
									return await Task.FromResult<IViewComponentResult>(View("Error"));
								}
							}
							if (!stringProcessed && ((string)rowsOptions).Contains(",") && !((string)rowsOptions).StartsWith("{") && !((string)rowsOptions).StartsWith("["))
							{
								var optionsArray = ((string)rowsOptions).Split(',');
								var optionsList = new List<SelectOption>();
								foreach (var optionString in optionsArray)
								{
									optionsList.Add(new SelectOption(optionString, optionString));
								}
								model.Rows = optionsList;
							}
						}
					}
					{
						dynamic columnsOptions = context.DataModel.GetPropertyValueByDataSource(options.Columns);
						if (columnsOptions == null) { }
						if (columnsOptions is List<SelectOption>)
						{
							model.Columns = (List<SelectOption>)columnsOptions;
						}
						else if (columnsOptions is string)
						{
							var stringProcessed = false;
							if (String.IsNullOrWhiteSpace(columnsOptions))
							{
								model.Columns = new List<SelectOption>();
								stringProcessed = true;
							}
							if (!stringProcessed && (((string)columnsOptions).StartsWith("{") || ((string)columnsOptions).StartsWith("[")))
							{
								try
								{
									model.Columns = JsonConvert.DeserializeObject<List<SelectOption>>(columnsOptions);
									stringProcessed = true;
								}
								catch
								{
									stringProcessed = false;
									ViewBag.ExceptionMessage = "Columns Json Deserialization failed!";
									return await Task.FromResult<IViewComponentResult>(View("Error"));
								}
							}
							if (!stringProcessed && ((string)columnsOptions).Contains(",") && !((string)columnsOptions).StartsWith("{") && !((string)columnsOptions).StartsWith("["))
							{
								var optionsArray = ((string)columnsOptions).Split(',');
								var optionsList = new List<SelectOption>();
								foreach (var optionString in optionsArray)
								{
									optionsList.Add(new SelectOption(optionString, optionString));
								}
								model.Columns = optionsList;
							}
						}
					}

					#endregion


				}


				switch (context.Mode)
				{
					case ComponentMode.Display:
						return await Task.FromResult<IViewComponentResult>(View("Display"));
					case ComponentMode.Design:
						return await Task.FromResult<IViewComponentResult>(View("Design"));
					case ComponentMode.Options:
						return await Task.FromResult<IViewComponentResult>(View("Options"));
					case ComponentMode.Help:
						return await Task.FromResult<IViewComponentResult>(View("Help"));
					default:
						ViewBag.Error = new ValidationException()
						{
							Message = "Unknown component mode"
						};
						return await Task.FromResult<IViewComponentResult>(View("Error"));
				}

			}
			catch (ValidationException ex)
			{
				ViewBag.Error = ex;
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
			catch (Exception ex)
			{
				ViewBag.Error = new ValidationException()
				{
					Message = ex.Message
				};
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
		}
	}
}
