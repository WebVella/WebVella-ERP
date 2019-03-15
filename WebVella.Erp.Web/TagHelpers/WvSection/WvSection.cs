using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.TagHelpers
{

    //[OutputElementHint("div")]
    public class WvSection : TagHelper
    {

        [HtmlAttributeName("is-visible")]
        public bool isVisible { get; set; } = true;

        [HtmlAttributeName("id")]
        public string Id { get; set; } = "";

        [HtmlAttributeName("class")]
        public string Class { get; set; } = "";

        [HtmlAttributeName("body-class")]
        public string BodyClass { get; set; } = "";

        [HtmlAttributeName("is-card")]
        public bool IsCard { get; set; } = false;

        [HtmlAttributeName("is-collapsable")]
        public bool IsCollapsable { get; set; } = false;

        [HtmlAttributeName("is-collapsed")]
        public bool IsCollapsed { get; set; } = true;

        [HtmlAttributeName("title")]
        public string Title { get; set; } = "";

        [HtmlAttributeName("title-tag")]
        public string TitleTag { get; set; } = "h3";

        [HtmlAttributeName("label-mode")]
        public LabelRenderMode LabelMode { get; set; } = LabelRenderMode.Undefined; //To be inherited

        [HtmlAttributeName("field-mode")]
        public FieldRenderMode FieldMode { get; set; } = FieldRenderMode.Undefined; //To be inherited

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!isVisible)
            {
                output.SuppressOutput();
                return Task.CompletedTask;
            }

            if (String.IsNullOrWhiteSpace(Id))
            {
                Id = Guid.NewGuid().ToString();
            }

            #region << Init >>

            if (LabelMode == LabelRenderMode.Undefined)
            {
                //Check if it is defined in form group
                if (context.Items.ContainsKey(typeof(LabelRenderMode)))
                {
                    LabelMode = (LabelRenderMode)context.Items[typeof(LabelRenderMode)];
                }
                else
                {
                    LabelMode = LabelRenderMode.Stacked;
                }
            }

            if (FieldMode == FieldRenderMode.Undefined)
            {
                //Check if it is defined in form group
                if (context.Items.ContainsKey(typeof(FieldRenderMode)))
                {
                    FieldMode = (FieldRenderMode)context.Items[typeof(FieldRenderMode)];
                }
                else
                {
                    FieldMode = FieldRenderMode.Form;
                }
            }
            #endregion

            if (!IsCard)
            {
                output.TagName = "div";
                output.AddCssClass("lns");
                output.AddCssClass(Class);
                output.Attributes.Add("id", Id);

                if (!String.IsNullOrWhiteSpace(Title))
                {
                    var titleEl = new TagBuilder(TitleTag);
                    titleEl.AddCssClass("lns-header");

                    if (!IsCollapsable)
                    {
                        output.AddCssClass("lns-nc");
                        titleEl.InnerHtml.AppendHtml("<span class=\"title\"><i class=\"icon fas fa-fw fa-circle\"></i>" + Title + "</span>");
                    }
                    else
                    {
                        output.AddCssClass("lns-c");
                        var collapseLink = new TagBuilder("a");
                        collapseLink.Attributes.Add("data-toggle", "collapse");
                        collapseLink.Attributes.Add("href", "#collapse-" + Id);
                        var iconEl = new TagBuilder("span");
                        if (IsCollapsed)
                        {
                            iconEl.AddCssClass("icon fas fa-fw fa-caret-right");
                            collapseLink.AddCssClass("collapsed");
                            collapseLink.Attributes.Add("aria-expanded", "false");
                        }
                        else
                        {
                            iconEl.AddCssClass("icon fas fa-fw fa-caret-down");
                            collapseLink.Attributes.Add("aria-expanded", "true");
                        }
                        collapseLink.InnerHtml.AppendHtml(iconEl);
                        collapseLink.InnerHtml.AppendHtml(Title);
                        titleEl.InnerHtml.AppendHtml(collapseLink);
                    }
                    output.PreContent.AppendHtml(titleEl);
                }

                if (IsCollapsable)
                {
                    var collapseWrapper = new TagBuilder("div");
                    collapseWrapper.AddCssClass("collapse multi-collapse");
                    collapseWrapper.AddCssClass(BodyClass);
                    if (!IsCollapsed)
                    {
                        collapseWrapper.AddCssClass("show");
                    }
                    collapseWrapper.Attributes.Add("id", "collapse-" + Id);
                    output.PreContent.AppendHtml(collapseWrapper.RenderStartTag());
                    output.PostContent.AppendHtml(collapseWrapper.RenderEndTag());
                }
            }
            else
            {
                //Collapsable card not implemented - check bootstrap for future implementation guidence
                output.TagName = "div";
                output.AddCssClass("card lns");
                output.AddCssClass(Class);
                output.Attributes.Add("id", Id);

                if (!String.IsNullOrWhiteSpace(Title))
                {
                    if (!IsCollapsable)
                    {
                        output.AddCssClass("lns-nc");
                        var cardHeader = new TagBuilder("div");
                        cardHeader.AddCssClass("card-header");

                        //cardHeader.InnerHtml.AppendHtml("<div class='float-right'>test</div>");

                        var titleEl = new TagBuilder(TitleTag);
                        titleEl.AddCssClass("lns-header m-0");
                        titleEl.InnerHtml.AppendHtml(Title);
                        cardHeader.InnerHtml.AppendHtml(titleEl);
                        output.PreContent.AppendHtml(cardHeader);
                    }
                    else
                    {
                        output.AddCssClass("lns-c");
                        var cardHeader = new TagBuilder("a");
                        cardHeader.AddCssClass("card-header pl-1 lns-header");
                        cardHeader.Attributes.Add("data-toggle", "collapse");
                        cardHeader.Attributes.Add("href", "#collapse-" + Id);
                        var iconEl = new TagBuilder("span");
                        if (IsCollapsed)
                        {
                            iconEl.AddCssClass("icon fa fa-fw fa-caret-right");
                            cardHeader.AddCssClass("collapsed");
                            cardHeader.Attributes.Add("aria-expanded", "false");
                        }
                        else
                        {
                            iconEl.AddCssClass("icon fa fa-fw fa-caret-down");
                            cardHeader.Attributes.Add("aria-expanded", "true");
                        }
                        cardHeader.InnerHtml.AppendHtml(iconEl);


                        var titleEl = new TagBuilder(TitleTag);
                        titleEl.AddCssClass("m-0 d-inline-block");
                        titleEl.InnerHtml.AppendHtml(Title);
                        cardHeader.InnerHtml.AppendHtml(titleEl);
                        output.PreContent.AppendHtml(cardHeader);
                    }
                }

                var cardBodyEl = new TagBuilder("div");
                cardBodyEl.AddCssClass("card-body");
                cardBodyEl.AddCssClass(BodyClass);

                var collapseWrapper = new TagBuilder("div");
                if (IsCollapsable)
                {
                    collapseWrapper.AddCssClass("collapse multi-collapse");
                    if (!IsCollapsed)
                    {
                        collapseWrapper.AddCssClass("show");
                    }
                    collapseWrapper.Attributes.Add("id", "collapse-" + Id);

                    output.PreContent.AppendHtml(collapseWrapper.RenderStartTag());
                }

                output.PreContent.AppendHtml(cardBodyEl.RenderStartTag());
                output.PostContent.AppendHtml(cardBodyEl.RenderEndTag());

                if (IsCollapsable)
                {
                    output.PostContent.AppendHtml(collapseWrapper.RenderEndTag());
                }
            }


            var jsCompressor = new JavaScriptCompressor();
            #region << Init Scripts >>
            var tagHelperInitialized = false;
            var fileName = "service.js";
            if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvSection) + fileName))
            {
                var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvSection) + fileName];
                tagHelperInitialized = tagHelperContext.Initialized;
            }
            if (!tagHelperInitialized)
            {
                var scriptContent = FileService.GetEmbeddedTextResource(fileName, "WebVella.Erp.Web.TagHelpers.WvSection");
                var scriptEl = new TagBuilder("script");
                scriptEl.Attributes.Add("type", "text/javascript");
                scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
                //scriptEl.InnerHtml.AppendHtml(scriptContent);
                output.PostContent.AppendHtml(scriptEl);

                ViewContext.HttpContext.Items[typeof(WvSection) + fileName] = new WvTagHelperContext()
                {
                    Initialized = true
                };

            }
            #endregion

            #region << Add Inline Init Script for this instance >>
            var initScript = new TagBuilder("script");
            initScript.Attributes.Add("type", "text/javascript");
            var scriptTemplate = @"
            		$(function(){
            			WvSectionInit(""{{SectionId}}"");
            		});";
            scriptTemplate = scriptTemplate.Replace("{{SectionId}}", Id.ToString());

            initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

            output.PostContent.AppendHtml(initScript);
            #endregion



            context.Items[typeof(LabelRenderMode)] = LabelMode;
            context.Items[typeof(FieldRenderMode)] = FieldMode;

            return Task.CompletedTask;
        }
    }
}
