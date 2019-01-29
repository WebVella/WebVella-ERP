using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

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

			public static PcFieldSelectOptions CopyFromBaseOptions(PcFieldBaseOptions input)
			{
				return new PcFieldSelectOptions
				{
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
				var instanceOptions = PcFieldSelectOptions.CopyFromBaseOptions(baseOptions);
				if (context.Options != null)
				{
					instanceOptions = JsonConvert.DeserializeObject<PcFieldSelectOptions>(context.Options.ToString());
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
				}
				var modelFieldLabel = "";
				var model = (PcFieldSelectModel)InitPcFieldBaseModel(context, instanceOptions, label: out modelFieldLabel, targetModel: "PcFieldSelectModel");
				if (String.IsNullOrWhiteSpace(instanceOptions.LabelText))
				{
					instanceOptions.LabelText = modelFieldLabel;
				}
				//PcFieldSelectModel model = PcFieldSelectModel.CopyFromBaseModel(baseModel);

				//Implementing Inherit label mode
				ViewBag.LabelMode = instanceOptions.LabelMode;
				ViewBag.Mode = instanceOptions.Mode;

				if (instanceOptions.LabelMode == LabelRenderMode.Undefined && baseOptions.LabelMode != LabelRenderMode.Undefined)
					ViewBag.LabelMode = baseOptions.LabelMode;

				if (instanceOptions.Mode == FieldRenderMode.Undefined && baseOptions.Mode != FieldRenderMode.Undefined)
					ViewBag.Mode = baseOptions.Mode;


				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion

				#region << Init DataSources >>
				if (context.Mode != ComponentMode.Options && context.Mode != ComponentMode.Help)
				{
					model.Value = context.DataModel.GetPropertyValueByDataSource(instanceOptions.Value);

					dynamic optionsResult = context.DataModel.GetPropertyValueByDataSource(instanceOptions.Options);
					var dataSourceOptions = new List<SelectOption>();
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
				}

				#endregion


				ViewBag.Options = instanceOptions;
				ViewBag.Model = model;
				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;

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
