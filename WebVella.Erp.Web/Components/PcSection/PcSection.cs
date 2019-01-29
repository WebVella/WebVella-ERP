using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web.Components
{
	[PageComponent(Label = "Section", Library = "WebVella", Description = "A foldable section", Version = "0.0.1", IconClass = "far fa-object-group")]
	public class PcSection : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcSection([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcSectionOptions
		{
			[JsonProperty(PropertyName = "title")]
			public string Title { get; set; } = "";

			[JsonProperty(PropertyName = "title_tag")]
			public string TitleTag { get; set; } = "h3";

			[JsonProperty(PropertyName = "class")]
			public string Class { get; set; } = "";

			[JsonProperty(PropertyName = "body_class")]
			public string BodyClass { get; set; } = "";

			[JsonProperty(PropertyName = "is_card")]
			public bool IsCard { get; set; } = false;

			[JsonProperty(PropertyName = "is_collapsable")]
			public bool IsCollapsable { get; set; } = false;

			[JsonProperty(PropertyName = "is_collapsed")]
			public bool IsCollapsed { get; set; } = false;

			[JsonProperty(PropertyName = "label_mode")]
			public LabelRenderMode LabelMode { get; set; } = LabelRenderMode.Undefined; //To be inherited

			[JsonProperty(PropertyName = "field_mode")]
			public FieldRenderMode FieldMode { get; set; } = FieldRenderMode.Undefined; //To be inherited
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
				if (pageFromModel is ErpPage)
				{
					currentPage = (ErpPage)pageFromModel;
				}
				else
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: PageModel does not have Page property or it is not from ErpPage Type"));
				}

				if (currentPage == null)
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: The page Id is required to be set as query param 'pid', when requesting this component"));
				}

				var instanceOptions = new PcSectionOptions();
				if (context.Options != null)
				{
					instanceOptions = JsonConvert.DeserializeObject<PcSectionOptions>(context.Options.ToString());
				}

				//Check if it is defined in form group
				if (instanceOptions.LabelMode == LabelRenderMode.Undefined)
				{
					if (context.Items.ContainsKey(typeof(LabelRenderMode)))
					{
						instanceOptions.LabelMode = (LabelRenderMode)context.Items[typeof(LabelRenderMode)];
					}
					else
					{
						instanceOptions.LabelMode = LabelRenderMode.Stacked;
					}
				}

				//Check if it is defined in form group
				if (instanceOptions.FieldMode == FieldRenderMode.Undefined)
				{
					if (context.Items.ContainsKey(typeof(FieldRenderMode)))
					{
						instanceOptions.FieldMode = (FieldRenderMode)context.Items[typeof(FieldRenderMode)];
					}
					else
					{
						instanceOptions.FieldMode = FieldRenderMode.Form;
					}
				}
				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion


				ViewBag.Options = instanceOptions;
				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;
				ViewBag.ComponentContext = context;
				ViewBag.GeneralHelpSection = HelpJsApiGeneralSection;

				if (context.Mode == ComponentMode.Display || context.Mode == ComponentMode.Design)
				{
					ViewBag.ProcessedTitle = context.DataModel.GetPropertyValueByDataSource(instanceOptions.Title);
				}

				context.Items[typeof(LabelRenderMode)] = instanceOptions.LabelMode;
				context.Items[typeof(FieldRenderMode)] = instanceOptions.FieldMode;

				switch (context.Mode)
				{
					case ComponentMode.Display:
						return await Task.FromResult<IViewComponentResult>(View("Display"));
					case ComponentMode.Design:
						return await Task.FromResult<IViewComponentResult>(View("Design"));
					case ComponentMode.Options:
						ViewBag.LabelRenderModeOptions = ModelExtensions.GetEnumAsSelectOptions<LabelRenderMode>();
						ViewBag.FieldRenderModeOptions = ModelExtensions.GetEnumAsSelectOptions<FieldRenderMode>();
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
