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
	[PageComponent(Label = "Modal", Library = "WebVella", Description = "A modal", Version = "0.0.1", IconClass = "far fa-window-maximize")]
	public class PcModal : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcModal([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcModalOptions
		{
			[JsonProperty(PropertyName = "is_visible")]
			public string IsVisible { get; set; } = "";

            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; } = "";

			[JsonProperty(PropertyName = "title")]
			public string Title { get; set; } = "";

			[JsonProperty(PropertyName = "position")]
			public WvModalPosition Position { get; set; } = WvModalPosition.Top;

			[JsonProperty(PropertyName = "size")]
			public WvModalSize Size { get; set; } = WvModalSize.Normal;

			[JsonProperty(PropertyName = "backdrop")]
			public string Backdrop { get; set; } = "true";

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

				var instanceOptions = new PcModalOptions();
				if (context.Options != null)
				{
					instanceOptions = JsonConvert.DeserializeObject<PcModalOptions>(context.Options.ToString());
				}

				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion

				if(String.IsNullOrWhiteSpace(instanceOptions.Id))
					instanceOptions.Id = "wv-" + context.Node.Id;

				ViewBag.Options = instanceOptions;
				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;
				ViewBag.ComponentContext = context;
				ViewBag.GeneralHelpSection = HelpJsApiGeneralSection;

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
                    ViewBag.IsVisible = isVisible;
                }

                switch (context.Mode)
				{
					case ComponentMode.Display:
						ViewBag.ProcessedTitle = context.DataModel.GetPropertyValueByDataSource(instanceOptions.Title);
						return await Task.FromResult<IViewComponentResult>(View("Display"));
					case ComponentMode.Design:
						return await Task.FromResult<IViewComponentResult>(View("Design"));
					case ComponentMode.Options:
						ViewBag.PositionOptions = WebVella.TagHelpers.Utilities.ModelExtensions.GetEnumAsSelectOptions<WvModalPosition>();
						ViewBag.SizeOptions = WebVella.TagHelpers.Utilities.ModelExtensions.GetEnumAsSelectOptions<WvModalSize>();
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
