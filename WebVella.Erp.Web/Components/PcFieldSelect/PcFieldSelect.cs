using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Web.Components
{
	[PageComponent(Label = "Field Select", Library = "WebVella", Description = "One value can be selected from a provided dropdown list", Version = "0.0.1", IconClass = "fas fa-caret-square-down")]
	public class PcFieldSelect : PcFieldBase
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcFieldSelect([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcFieldSelectOptions : PcFieldBaseOptions
		{
			[JsonProperty(PropertyName = "options")]
			public string Options { get; set; } = "";

			[JsonProperty(PropertyName = "show_icon")]
			public bool ShowIcon { get; set; } = false;

			[JsonProperty(PropertyName = "ajax_datasource")]
			public SelectOptionsAjaxDatasource AjaxDatasource { get; set; } = null;

			[JsonProperty(PropertyName = "select_match_type")]
			public WvSelectMatchType SelectMatchingType { get; set; } = WvSelectMatchType.Contains;

			[JsonProperty(PropertyName = "placeholder")]
			public string Placeholder { get; set; } = "";
			/*
			* Datasource for the link
			* Feature: Linkable Text Field
			*Author: Amarjeet-L
			*/
			[JsonProperty(PropertyName = "link")]
			public string Link { get; set; } = "";
			/*
			* Evaluated value for the link
			* Feature: Linkable Text Field
			*Author: Amarjeet-L
			*/
			[JsonProperty(PropertyName = "href")]
			public string Href { get; set; } = "";
			public static PcFieldSelectOptions CopyFromBaseOptions(PcFieldBaseOptions input)
			{
				return new PcFieldSelectOptions
				{
					IsVisible = input.IsVisible,
					LabelMode = input.LabelMode,
					LabelText = input.LabelText,
					Mode = input.Mode,
					Name = input.Name,
					Options = "",
					AjaxDatasource = null,
					SelectMatchingType = WvSelectMatchType.Contains,
					Description = input.Description,
					LabelHelpText = input.LabelHelpText
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
				var options = PcFieldSelectOptions.CopyFromBaseOptions(baseOptions);
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcFieldSelectOptions>(context.Options.ToString());
					if (context.Mode != ComponentMode.Options)
					{
						if (String.IsNullOrWhiteSpace(options.LabelHelpText))
							options.LabelHelpText = baseOptions.LabelHelpText;

						if (String.IsNullOrWhiteSpace(options.Description))
							options.Description = baseOptions.Description;

					}
					////Check for connection to entity field
					//if (instanceOptions.TryConnectToEntity)
					//{
					//	var entity = context.DataModel.GetProperty("Entity");
					//	if (entity != null && entity is Entity)
					//	{
					//		var fieldName = instanceOptions.Name;
					//		var entityField = ((Entity)entity).Fields.FirstOrDefault(x => x.Name == fieldName);
					//		if (entityField != null && entityField is PhoneField)
					//		{
					//			var castedEntityField = ((PhoneField)entityField);
					//			//No options connected
					//		}
					//	}
					//}
					/*
					* If link is present, evaluate the datasource and find the final link and assign to href
					* Feature: Linkable Text Field
					*Author: Amarjeet-L
					*/
					string link = options.Link;
					if (link != "")
					{
						link = context.DataModel.GetPropertyValueByDataSource(options.Link).ToString();
						options.Href = link;
					}
				}
				
				var modelFieldLabel = "";
				var model = (PcFieldSelectModel)InitPcFieldBaseModel(context, options, label: out modelFieldLabel, targetModel: "PcFieldSelectModel");
				if (String.IsNullOrWhiteSpace(options.LabelText) && context.Mode != ComponentMode.Options)
				{
					options.LabelText = modelFieldLabel;
				}
				//PcFieldSelectModel model = PcFieldSelectModel.CopyFromBaseModel(baseModel);

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
					model.Value = context.DataModel.GetPropertyValueByDataSource(options.Value);

					dynamic optionsResult = context.DataModel.GetPropertyValueByDataSource(options.Options);

					var dataSourceOptions = new List<SelectOption>();
					if (optionsResult == null) { }
					if (optionsResult is List<SelectOption>)
					{
						dataSourceOptions = (List<SelectOption>)optionsResult;
					}
					if (optionsResult is List<WvSelectOption>)
					{
						foreach (var option in (List<WvSelectOption>)optionsResult)
						{
						dataSourceOptions.Add(new SelectOption{
							Color = option.Color,
							IconClass = option.IconClass,
							Label = option.Label,
							Value = option.Value
						});
						}
					}
					else if (optionsResult is string)
					{
						var stringProcessed = false;
						if (String.IsNullOrWhiteSpace(optionsResult))
						{
							dataSourceOptions = new List<SelectOption>();
							stringProcessed = true;
						}
						//AJAX Options
						if (!stringProcessed && ((string)optionsResult).StartsWith("{")) {
							try
							{
								options.AjaxDatasource = JsonConvert.DeserializeObject<SelectOptionsAjaxDatasource>(optionsResult,new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error});
								stringProcessed = true;
								ViewBag.Options = options;
							}
							catch {

							}
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
								return await Task.FromResult<IViewComponentResult>(Content("Error: Options Json De-serialization failed!"));
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

				ViewBag.SelectMatchOptions = WebVella.TagHelpers.Utilities.ModelExtensions.GetEnumAsSelectOptions<WvSelectMatchType>();

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
