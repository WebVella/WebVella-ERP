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
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Web.Components
{
    [PageComponent(Label = "Form", Library = "WebVella", Description = "Render a form with a Validation token", Version = "0.0.1", IconClass = "fas fa-poll-h")]
    public class PcForm : PageComponent
    {
        protected ErpRequestContext ErpRequestContext { get; set; }

        public PcForm([FromServices]ErpRequestContext coreReqCtx)
        {
            ErpRequestContext = coreReqCtx;
        }

        public class PcFormOptions
        {
            [JsonProperty(PropertyName = "is_visible")]
            public string IsVisible { get; set; } = "";

            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; } = "";

            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; } = "form";

            [JsonProperty(PropertyName = "method")]
            public string Method { get; set; } = "post";

            [JsonProperty(PropertyName = "hook_key")]
            public string HookKey { get; set; } = "";

            [JsonProperty(PropertyName = "label_mode")]
            public WvLabelRenderMode LabelMode { get; set; } = WvLabelRenderMode.Stacked; //To be inherited

            [JsonProperty(PropertyName = "mode")]
            public WvFieldRenderMode Mode { get; set; } = WvFieldRenderMode.Form; //To be inherited

            [JsonProperty(PropertyName = "class")]
            public string Class { get; set; } = "";

            [JsonProperty(PropertyName = "show_validation")]
            public bool ShowValidation { get; set; } = true;

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

                var instanceOptions = new PcFormOptions();
                if (context.Options != null)
                {
                    instanceOptions = JsonConvert.DeserializeObject<PcFormOptions>(context.Options.ToString());
                    if (instanceOptions.LabelMode == WvLabelRenderMode.Undefined)
                        instanceOptions.LabelMode = WvLabelRenderMode.Stacked;
                    if (instanceOptions.Mode == WvFieldRenderMode.Undefined)
                        instanceOptions.Mode = WvFieldRenderMode.Form;
                }

                if (String.IsNullOrWhiteSpace(instanceOptions.Id))
                {
                    instanceOptions.Id = "wv-" + context.Node.Id.ToString();
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

                ViewBag.LabelRenderModeOptions = WebVella.TagHelpers.Utilities.ModelExtensions.GetEnumAsSelectOptions<WvLabelRenderMode>();

                ViewBag.FieldRenderModeOptions = WebVella.TagHelpers.Utilities.ModelExtensions.GetEnumAsSelectOptions<WvFieldRenderMode>();

                context.Items[typeof(WvLabelRenderMode)] = instanceOptions.LabelMode;
                context.Items[typeof(WvFieldRenderMode)] = instanceOptions.Mode;

                ViewBag.MethodOptions = new List<SelectOption>() {
                    new SelectOption("get","get"),
                    new SelectOption("post","post")
                    };

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

                    var validation = context.DataModel.GetProperty("Validation") as ValidationException ?? new ValidationException();

                    context.Items[typeof(ValidationException)] = validation;
                    ViewBag.Validation = validation;

                    ViewBag.Action = "";
                    if (!String.IsNullOrWhiteSpace(instanceOptions.HookKey))
                    {
                        var queryList = new List<SelectOption>();
                        foreach (var key in HttpContext.Request.Query.Keys)
                        {
                            if (key != "hookKey")
                            {
                                queryList.Add(new SelectOption(key, HttpContext.Request.Query[key].ToString()));
                            }
                        }
                        queryList.Add(new SelectOption("hookKey", instanceOptions.HookKey)); //override even if already present

                        ViewBag.Action = string.Format(HttpContext.Request.Path + "?{0}", string.Join("&", queryList.Select(kvp => string.Format("{0}={1}", kvp.Value, kvp.Label))));
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
