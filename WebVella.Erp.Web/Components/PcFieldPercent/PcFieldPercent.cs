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
	[PageComponent(Label = "Field Percent", Library = "WebVella", Description = "This will automatically present any number you enter as a percent value", Version = "0.0.1", IconClass = "fas fa-percentage")]
	public class PcFieldPercent : PcFieldBase
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcFieldPercent([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcFieldPercentOptions : PcFieldBaseOptions
		{

			[JsonProperty(PropertyName = "min")]
			public decimal? Min { get; set; } = null;

			[JsonProperty(PropertyName = "max")]
			public decimal? Max { get; set; } = null;

			[JsonProperty(PropertyName = "step")]
			public decimal? Step { get; set; } = null;

			[JsonProperty(PropertyName = "decimal_digits")]
			public int DecimalDigits { get; set; } = 2;


			public static PcFieldPercentOptions CopyFromBaseOptions(PcFieldBaseOptions input)
			{
				return new PcFieldPercentOptions
				{ 
					LabelMode = input.LabelMode,
					LabelText = input.LabelText,
					Mode = input.Mode,
					Name = input.Name,
					Min = null,
					Max = null,
					Step = null,
					DecimalDigits = 2
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
					return await Task.FromResult<IViewComponentResult>(Content("Error: The node Id is required to be set as query param 'nid', when requesting this component"));
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
				var instanceOptions = PcFieldPercentOptions.CopyFromBaseOptions(baseOptions);
				if (context.Options != null)
				{
					instanceOptions = JsonConvert.DeserializeObject<PcFieldPercentOptions>(context.Options.ToString());
					//Check for connection to entity field
					if (instanceOptions.TryConnectToEntity)
					{
						var entity = context.DataModel.GetProperty("Entity");
						if (entity != null && entity is Entity)
						{
							var fieldName = instanceOptions.Name;
							var entityField = ((Entity)entity).Fields.FirstOrDefault(x => x.Name == fieldName);
							if (entityField != null && entityField is PercentField)
							{
								var castedEntityField = ((PercentField)entityField);
								instanceOptions.Min = castedEntityField.MinValue;
								instanceOptions.Max = castedEntityField.MaxValue;
								instanceOptions.DecimalDigits = (int)(castedEntityField.DecimalPlaces ?? 0);
							}
						}
					}
				}
				var modelFieldLabel = "";
				var model = (PcFieldBaseModel)InitPcFieldBaseModel(context, instanceOptions, label: out modelFieldLabel);
				if (String.IsNullOrWhiteSpace(instanceOptions.LabelText))
				{
					instanceOptions.LabelText = modelFieldLabel;
				}

				//Implementing Inherit label mode
				ViewBag.LabelMode = instanceOptions.LabelMode;
				ViewBag.Mode = instanceOptions.Mode;

				if (instanceOptions.LabelMode == LabelRenderMode.Undefined && baseOptions.LabelMode != LabelRenderMode.Undefined)
					ViewBag.LabelMode = baseOptions.LabelMode;

				if (instanceOptions.Mode == FieldRenderMode.Undefined && baseOptions.Mode != FieldRenderMode.Undefined)
					ViewBag.Mode = baseOptions.Mode;


				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion

				if (context.Mode != ComponentMode.Options && context.Mode != ComponentMode.Help)
				{
					model.Value = context.DataModel.GetPropertyValueByDataSource(instanceOptions.Value);
				}

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
						ViewBag.ExceptionMessage = "Unknown component mode";
						ViewBag.Errors = new List<ValidationError>();
						return await Task.FromResult<IViewComponentResult>(View("Error"));
				}
			}
			catch (ValidationException ex)
			{
				ViewBag.ExceptionMessage = ex.Message;
				ViewBag.Errors = new List<ValidationError>();
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
			catch (Exception ex)
			{
				ViewBag.ExceptionMessage = ex.Message;
				ViewBag.Errors = new List<ValidationError>();
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
		}
	}
}
