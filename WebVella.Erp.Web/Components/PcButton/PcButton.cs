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
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Web.Components
{
	[PageComponent(Label = "Button", Library = "WebVella", Description = "Renders a button", Version = "0.0.1", IconClass = "far fa-caret-square-right", IsInline = true)]
	public class PcButton : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcButton([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcButtonOptions
		{
			[JsonProperty(PropertyName = "is_visible")]
			public string IsVisible { get; set; } = "";

			[JsonProperty(PropertyName = "type")]
			public WvButtonType Type { get; set; } = WvButtonType.Button;

			[JsonProperty(PropertyName = "is_outline")]
			public bool isOutline { get; set; } = false;

			[JsonProperty(PropertyName = "is_block")]
			public bool isBlock { get; set; } = false;

			[JsonProperty(PropertyName = "is_active")]
			public bool isActive { get; set; } = false;

			[JsonProperty(PropertyName = "is_disabled")]
			public bool isDisabled { get; set; } = false;

			[JsonProperty(PropertyName = "color")]
			public WvColor Color { get; set; } = WvColor.White;

			[JsonProperty(PropertyName = "size")]
			public WvCssSize Size { get; set; } = WvCssSize.Inherit;

			[JsonProperty(PropertyName = "class")]
			public string Class { get; set; } = "";

			[JsonProperty(PropertyName = "id")]
			public string Id { get; set; } = "";

			[JsonProperty(PropertyName = "text")]
			public string Text { get; set; } = "button";

			[JsonProperty(PropertyName = "onclick")]
			public string OnClick { get; set; } = "";

			[JsonProperty(PropertyName = "href")]
			public string Href { get; set; } = "";

			[JsonProperty(PropertyName = "new_tab")]
			public bool NewTab { get; set; } = false;

			[JsonProperty(PropertyName = "icon_class")]
			public string IconClass { get; set; } = "";

            [JsonProperty(PropertyName = "icon_right")]
            public bool IconRight { get; set; } = false;

            [JsonProperty(PropertyName = "form")]
			public string Form { get; set; } = "";

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
					return await Task.FromResult<IViewComponentResult>(Content("Error: The page Id is required to be set as query parameter 'pid', when requesting this component"));
				}

				var options = new PcButtonOptions();
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcButtonOptions>(context.Options.ToString());
				}

				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion

                ViewBag.Options = options;
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

                    options.Text = context.DataModel.GetPropertyValueByDataSource(options.Text) as string;
                    options.Class = context.DataModel.GetPropertyValueByDataSource(options.Class) as string;
                    options.IconClass = context.DataModel.GetPropertyValueByDataSource(options.IconClass) as string;
					options.OnClick = context.DataModel.GetPropertyValueByDataSource(options.OnClick) as string;

					ViewBag.ProcessedHref = context.DataModel.GetPropertyValueByDataSource(options.Href);

                }
                #region << Select options >>
                ViewBag.CssSize = WebVella.TagHelpers.Utilities.ModelExtensions.GetEnumAsSelectOptions<WvCssSize>();

                ViewBag.ColorOptions = WebVella.TagHelpers.Utilities.ModelExtensions.GetEnumAsSelectOptions<WvColor>().OrderBy(x => x.Label).ToList();

                ViewBag.TypeOptions = WebVella.TagHelpers.Utilities.ModelExtensions.GetEnumAsSelectOptions<WvButtonType>();

                #endregion
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
