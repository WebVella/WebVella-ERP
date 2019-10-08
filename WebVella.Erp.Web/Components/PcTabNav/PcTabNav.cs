using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web.Components
{
	[PageComponent(Label = "Tab Navigation", Library = "WebVella", Description = "Up to 7 tabs for your content", Version = "0.0.1", IconClass = "fas fa-folder")]
	public class PcTabNav : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcTabNav([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcTabNavOptions
		{
			[JsonProperty(PropertyName = "is_visible")]
			public string IsVisible { get; set; } = "";

			[JsonProperty(PropertyName = "visible_tabs")]
			public int VisibleTabs { get; set; } = 2;

			[JsonProperty(PropertyName = "render_type")]
			public TabNavRenderType RenderType { get; set; } = TabNavRenderType.PILLS;

			[JsonProperty(PropertyName = "css_class")]
			public string CssClass { get; set; } = "";

			[JsonProperty(PropertyName = "body_css_class")]
			public string BodyCssClass { get; set; } = "";

			#region << Tab1 >>
			[JsonProperty(PropertyName = "tab1_id")]
			public string Tab1Id { get; set; } = "tab1";

			[JsonProperty(PropertyName = "tab1_label")]
			public string Tab1Label { get; set; } = "Tab 1";
			#endregion

			#region << Tab2 >>
			[JsonProperty(PropertyName = "tab2_id")]
			public string Tab2Id { get; set; } = "tab2";

			[JsonProperty(PropertyName = "tab2_label")]
			public string Tab2Label { get; set; } = "Tab 2";
			#endregion

			#region << Tab3 >>
			[JsonProperty(PropertyName = "tab3_id")]
			public string Tab3Id { get; set; } = "tab3";

			[JsonProperty(PropertyName = "tab3_label")]
			public string Tab3Label { get; set; } = "Tab 3";
			#endregion

			#region << Tab4 >>
			[JsonProperty(PropertyName = "tab4_id")]
			public string Tab4Id { get; set; } = "tab4";

			[JsonProperty(PropertyName = "tab4_label")]
			public string Tab4Label { get; set; } = "Tab 4";
			#endregion

			#region << Tab5 >>
			[JsonProperty(PropertyName = "tab5_id")]
			public string Tab5Id { get; set; } = "tab5";

			[JsonProperty(PropertyName = "tab5_label")]
			public string Tab5Label { get; set; } = "Tab 5";
			#endregion

			#region << Tab6 >>
			[JsonProperty(PropertyName = "tab6_id")]
			public string Tab6Id { get; set; } = "tab6";

			[JsonProperty(PropertyName = "tab6_label")]
			public string Tab6Label { get; set; } = "Tab 6";
			#endregion

			#region << Tab7 >>
			[JsonProperty(PropertyName = "tab7_id")]
			public string Tab7Id { get; set; } = "tab7";

			[JsonProperty(PropertyName = "tab7_label")]
			public string Tab7Label { get; set; } = "Tab 7";
			#endregion



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

				var instanceOptions = new PcTabNavOptions();
				if (context.Options != null)
				{
					instanceOptions = JsonConvert.DeserializeObject<PcTabNavOptions>(context.Options.ToString());
				}

				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion

				ViewBag.Options = instanceOptions;
				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;
				ViewBag.ComponentContext = context;

                if (context.Mode != ComponentMode.Options && context.Mode != ComponentMode.Help)
                {
                    var isVisible = true;
                    var isVisibleDS = context.DataModel.GetPropertyValueByDataSource(instanceOptions.IsVisible);
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
                    if (!isVisible && context.Mode == ComponentMode.Display)
                        return await Task.FromResult<IViewComponentResult>(Content(""));
                }

                if (context.Mode == ComponentMode.Options)
					ViewBag.RenderTypeOptions = ModelExtensions.GetEnumAsSelectOptions<TabNavRenderType>();

				switch (context.Mode)
				{
					case ComponentMode.Display:
						if(instanceOptions.RenderType == TabNavRenderType.TABS)
							return await Task.FromResult<IViewComponentResult>(View("Display-Tabs"));
						else
							return await Task.FromResult<IViewComponentResult>(View("Display-Pills"));
					case ComponentMode.Design:
						if (instanceOptions.RenderType == TabNavRenderType.TABS)
							return await Task.FromResult<IViewComponentResult>(View("Design-Tabs"));
						else
							return await Task.FromResult<IViewComponentResult>(View("Design-Pills"));
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
