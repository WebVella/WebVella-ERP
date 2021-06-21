using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Web.Components
{
	[PageComponent(Label = "Field Checkbox", Library = "WebVella", Description = "The simple on and off switch. This field allows you to get a True (checked) or False (unchecked) value.", Version = "0.0.1", IconClass = "far fa-check-square")]
	public class PcFieldCheckbox : PcFieldBase
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcFieldCheckbox([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcFieldCheckboxOptions : PcFieldBaseOptions
		{
			[JsonProperty(PropertyName = "text_true")]
			public string TextTrue { get; set; } = "";

			[JsonProperty(PropertyName = "text_false")]
			public string TextFalse { get; set; } = "";

			public static PcFieldCheckboxOptions CopyFromBaseOptions(PcFieldBaseOptions input)
			{
				return new PcFieldCheckboxOptions
				{
					IsVisible = input.IsVisible,
					LabelMode = input.LabelMode,
					LabelText = input.LabelText,
					Mode = input.Mode,
					Name = input.Name,
					TextTrue = "",
					TextFalse = "",
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
				var options = PcFieldCheckboxOptions.CopyFromBaseOptions(baseOptions);
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcFieldCheckboxOptions>(context.Options.ToString());
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
					//		if (entityField != null && entityField is CheckboxField)
					//		{
					//			var castedEntityField = ((CheckboxField)entityField);
					//			//No options connected
					//		}
					//	}
					//}
				}
				var modelFieldLabel = "";
				var model = (PcFieldBaseModel)InitPcFieldBaseModel(context, options, label: out modelFieldLabel);
				if (String.IsNullOrWhiteSpace(options.LabelText) && context.Mode != ComponentMode.Options)
				{
					options.LabelText = modelFieldLabel;
				}

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
						model.Value = false;
					}
					else if (valueResult is bool)
					{
						model.Value = (bool)valueResult;
					}
					else if (valueResult is string)
					{
						var stringProcessed = false;
						if (String.IsNullOrWhiteSpace(valueResult))
						{
							model.Value = false;
							stringProcessed = true;
						}
						if (!stringProcessed && ((string)valueResult) == "true")
						{
							model.Value = true;
						}
						else
						{
							model.Value = false;
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
