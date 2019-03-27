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
	[PageComponent(Label = "Field Data Csv", Library = "WebVella", Description = "field for submitting CSV data", Version = "0.0.1", IconClass = "fas fa-table")]
	public class PcFieldDataCsv : PcFieldBase
    {
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcFieldDataCsv([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcFieldDataCsvOptions : PcFieldBaseOptions
        {
            [JsonProperty(PropertyName = "height")]
            public string Height { get; set; } = "";

            [JsonProperty(PropertyName = "delimiter_value_ds")]
            public string DelimiterValueDs { get; set; } = "comma";

            [JsonProperty(PropertyName = "delimiter_field_name")]
            public string DelimiterFieldName { get; set; } = "";

            [JsonProperty(PropertyName = "has_header_value_ds")]
            public string HasHeaderValueDs { get; set; } = "true";

            [JsonProperty(PropertyName = "has_header_field_name")]
            public string HasHeaderFieldName { get; set; } = "";

            [JsonProperty(PropertyName = "lang_ds")]
            public string LangDs { get; set; } = "en";

            public static PcFieldDataCsvOptions CopyFromBaseOptions(PcFieldBaseOptions input)
            {
                return new PcFieldDataCsvOptions
                {
                    IsVisible = input.IsVisible,
                    LabelMode = input.LabelMode,
                    LabelText = input.LabelText,
                    Mode = input.Mode,
                    Name = input.Name,
                    Height = "",
                    DelimiterValueDs = "",
                    HasHeaderValueDs = "",
                    DelimiterFieldName = "",
                    HasHeaderFieldName = "",
                    LangDs = "en",
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

                var baseOptions = InitPcFieldBaseOptions(context);
                var options = PcFieldDataCsvOptions.CopyFromBaseOptions(baseOptions);
                if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcFieldDataCsvOptions>(context.Options.ToString());
				}

                var modelFieldLabel = "";
                var model = (PcFieldBaseModel)InitPcFieldBaseModel(context, options, label: out modelFieldLabel);
                if (String.IsNullOrWhiteSpace(options.LabelText))
                {
                    options.LabelText = modelFieldLabel;
                }

                ViewBag.LabelMode = options.LabelMode;
                ViewBag.Mode = options.Mode;

                if (options.LabelMode == LabelRenderMode.Undefined && baseOptions.LabelMode != LabelRenderMode.Undefined)
                    ViewBag.LabelMode = baseOptions.LabelMode;

                if (options.Mode == FieldRenderMode.Undefined && baseOptions.Mode != FieldRenderMode.Undefined)
                    ViewBag.Mode = baseOptions.Mode;

                var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion

				ViewBag.Options = options;
                ViewBag.Model = model;
                ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;
				ViewBag.ComponentContext = context;
				ViewBag.GeneralHelpSection = HelpJsApiGeneralSection;

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

                    model.Value = context.DataModel.GetPropertyValueByDataSource(options.Value);

                    var delimiter = context.DataModel.GetPropertyValueByDataSource(options.DelimiterValueDs) as string;
                    var lang = context.DataModel.GetPropertyValueByDataSource(options.LangDs) as string;
                    var hasHeader = context.DataModel.GetPropertyValueByDataSource(options.HasHeaderValueDs) as bool?;

                    ViewBag.Delimiter = ErpDataCsvDelimiterType.COMMA;
                    if (!String.IsNullOrWhiteSpace(delimiter) && delimiter == "tab") {
                        ViewBag.Delimiter = ErpDataCsvDelimiterType.TAB;
                    }
                    ViewBag.Lang = "en";
                    if (!String.IsNullOrWhiteSpace(lang))
                    {
                        ViewBag.Lang = lang;
                    }
                    ViewBag.HasHeader = true;
                    if (hasHeader != null)
                    {
                        ViewBag.HasHeader = hasHeader;
                    }
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
