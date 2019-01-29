using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Web.Components
{
	[PageComponent(Label = "Field Number", Library = "WebVella", Description = "Only numbers are allowed. Leading zeros will be stripped.", Version = "0.0.1", IconClass = "fas fa-dice-six")]
	public class PcFieldNumber : PcFieldBase
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcFieldNumber([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcFieldNumberOptions : PcFieldBaseOptions
		{
			[JsonProperty(PropertyName = "min")]
			public decimal? Min { get; set; } = null;

			[JsonProperty(PropertyName = "max")]
			public decimal? Max { get; set; } = null;

			[JsonProperty(PropertyName = "step")]
			public decimal? Step { get; set; } = null;

			[JsonProperty(PropertyName = "decimal_digits")]
			public int DecimalDigits { get; set; } = 2;

			public static PcFieldNumberOptions CopyFromBaseOptions(PcFieldBaseOptions input)
			{
				return new PcFieldNumberOptions
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
				var options = PcFieldNumberOptions.CopyFromBaseOptions(baseOptions);
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcFieldNumberOptions>(context.Options.ToString());
					//Check for connection to entity field
					Entity mappedEntity = null;
					if (options.ConnectedEntityId != null)
					{
						mappedEntity = new EntityManager().ReadEntity(options.ConnectedEntityId.Value).Object;
					}
					else
					{
						var entity = context.DataModel.GetProperty("Entity");
						if (entity is Entity)
						{
							mappedEntity = (Entity)entity;
						}
					}

					if (mappedEntity != null)
					{
						var fieldName = options.Name;
						var entityField = mappedEntity.Fields.FirstOrDefault(x => x.Name == fieldName);
						if (entityField != null && entityField is NumberField)
						{
							var castedEntityField = ((NumberField)entityField);
							options.Min = castedEntityField.MinValue;
							options.Max = castedEntityField.MaxValue;
							options.DecimalDigits = (int)(castedEntityField.DecimalPlaces ?? 0);
						}
					}
				}
				var modelFieldLabel = "";
				var model = (PcFieldBaseModel)InitPcFieldBaseModel(context, options, label: out modelFieldLabel);
				if (String.IsNullOrWhiteSpace(options.LabelText))
				{
					options.LabelText = modelFieldLabel;
				}

				//Implementing Inherit label mode
				ViewBag.LabelMode = options.LabelMode;
				ViewBag.Mode = options.Mode;

				if (options.LabelMode == LabelRenderMode.Undefined && baseOptions.LabelMode != LabelRenderMode.Undefined)
					ViewBag.LabelMode = baseOptions.LabelMode;

				if (options.Mode == FieldRenderMode.Undefined && baseOptions.Mode != FieldRenderMode.Undefined)
					ViewBag.Mode = baseOptions.Mode;


				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion

				if (context.Mode != ComponentMode.Options && context.Mode != ComponentMode.Help)
				{
					model.Value = context.DataModel.GetPropertyValueByDataSource(options.Value);
				}


				ViewBag.Options = options;
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
