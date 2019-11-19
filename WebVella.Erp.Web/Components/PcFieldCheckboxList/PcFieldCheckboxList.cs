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
	[PageComponent(Label = "Field Checkbox list", Library = "WebVella", Description = "Multiple values can be selected from a provided list", Version = "0.0.1", IconClass = "fas fa-check-double")]
	public class PcFieldCheckboxList : PcFieldBase
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcFieldCheckboxList([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcFieldCheckboxListOptions : PcFieldBaseOptions
		{
			[JsonProperty(PropertyName = "options")]
			public string Options { get; set; } = "";

			public static PcFieldCheckboxListOptions CopyFromBaseOptions(PcFieldBaseOptions input)
			{
				return new PcFieldCheckboxListOptions
				{
					IsVisible = input.IsVisible,
					LabelMode = input.LabelMode,
					LabelText = input.LabelText,
					Mode = input.Mode,
					Name = input.Name,
					Options = ""
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

				var options = PcFieldCheckboxListOptions.CopyFromBaseOptions(baseOptions);

				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcFieldCheckboxListOptions>(context.Options.ToString());
				}
				var modelFieldLabel = "";
				var model = (PcFieldCheckboxListModel)InitPcFieldBaseModel(context, options, label: out modelFieldLabel, targetModel: "PcFieldCheckboxListModel");
				if (String.IsNullOrWhiteSpace(options.LabelText) && context.Mode != ComponentMode.Options)
				{
					options.LabelText = modelFieldLabel;
				}

				//PcFieldCheckboxListModel model = PcFieldCheckboxListModel.CopyFromBaseModel(baseModel);

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
						model.Value = new List<string>();
					}
					else if (valueResult is List<string>)
					{
						model.Value = (List<string>)valueResult;
					}
					else if (valueResult is string)
					{
						var stringProcessed = false;
						if (String.IsNullOrWhiteSpace(valueResult))
						{
							model.Value = new List<string>();
							stringProcessed = true;
						}
						if (!stringProcessed && (((string)valueResult).StartsWith("{") || ((string)valueResult).StartsWith("[")))
						{
							try
							{
								model.Value = JsonConvert.DeserializeObject<List<string>>(valueResult.ToString());
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
						if (!stringProcessed && ((string)valueResult).Contains(",") && !((string)valueResult).Contains("{") && !((string)valueResult).Contains("["))
						{
							var valueArray = ((string)valueResult).Split(',');
							model.Value = new List<string>(valueArray);
						}
					}

					var dataSourceOptions = new List<SelectOption>();
					dynamic optionsResult = context.DataModel.GetPropertyValueByDataSource(options.Options);
					if (optionsResult == null) { }
					if (optionsResult is List<SelectOption>)
					{
						dataSourceOptions = (List<SelectOption>)optionsResult;
					}
					else if (optionsResult is string)
					{
						var stringProcessed = false;
						if (String.IsNullOrWhiteSpace(optionsResult))
						{
							dataSourceOptions = new List<SelectOption>();
							stringProcessed = true;
						}
						if (!stringProcessed && (((string)optionsResult).StartsWith("{") || ((string)optionsResult).StartsWith("[")))
						{
							try
							{
								dataSourceOptions = JsonConvert.DeserializeObject<List<SelectOption>>(optionsResult);
								stringProcessed = true;
							}
							catch
							{
								stringProcessed = false;
								ViewBag.ExceptionMessage = "Options Json Deserialization failed!";
								ViewBag.Errors = new List<ValidationError>();
								return await Task.FromResult<IViewComponentResult>(View("Error"));
							}
						}
						if (!stringProcessed && ((string)optionsResult).Contains(",") && !((string)optionsResult).Contains("{") && !((string)optionsResult).Contains("["))
						{
							var optionsArray = ((string)optionsResult).Split(',');
							var optionsList = new List<SelectOption>();
							foreach (var optionString in optionsArray)
							{
								optionsList.Add(new SelectOption(optionString, optionString));
							}
							dataSourceOptions = optionsList;
						}
					}
					if (dataSourceOptions.Count > 0)
					{
						model.Options = dataSourceOptions;
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
