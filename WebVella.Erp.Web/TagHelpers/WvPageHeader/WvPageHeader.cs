using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using Yahoo.Yui.Compressor;
using WebVella.Erp.Web.Utils;
using WebVella.Erp.Api.Models;
using System.Linq;
using System.Globalization;
using HtmlAgilityPack;

namespace WebVella.Erp.Web.TagHelpers
{
	[HtmlTargetElement("wv-page-header")]
	[RestrictChildren("wv-page-header-actions", "wv-page-header-toolbar", "wv-page-header-actions-aux")]
	public class WvPageHeader : TagHelper
	{

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("is-visible")]
		public bool isVisible { get; set; } = true;

		[HtmlAttributeName("color")]
		public string Color { get; set; } = "";

		[HtmlAttributeName("icon-color")]
		public string IconColor { get; set; } = "";

		[HtmlAttributeName("area-label")]
		public string AreaLabel { get; set; } = "";

		[HtmlAttributeName("area-sublabel")]
		public string AreaSubLabel { get; set; } = "";

		[HtmlAttributeName("title")]
		public string Title { get; set; } = "";

		[HtmlAttributeName("subtitle")]
		public string SubTitle { get; set; } = "";

		[HtmlAttributeName("description")]
		public string Description { get; set; } = "";

		[HtmlAttributeName("icon-class")]
		public string IconClass { get; set; } = "";

		[HtmlAttributeName("return-url")]
		public string ReturnUrl { get; set; } = "";

        [HtmlAttributeName("fix-on-scroll")]
        public bool FixOnScroll { get; set; } = false;

        [HtmlAttributeName("page-switch-items")]
		public List<PageSwitchItem> PageSwitchItems { get; set; } = new List<PageSwitchItem>();

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (!isVisible)
			{
				output.SuppressOutput();
			}
			else
			{
                var elementId = Guid.NewGuid();
				var content = await output.GetChildContentAsync();
				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(content.GetContent());

				var actionsContentHtml = "";
				var actionsAuxContentHtml = "";
				var toolbarContentHtml = "";
				var actionTagHelperList = htmlDoc.DocumentNode.Descendants("wv-page-header-actions");
				var actionAuxTagHelperList = htmlDoc.DocumentNode.Descendants("wv-page-header-actions-aux");
				var toolbarTagHelperList = htmlDoc.DocumentNode.Descendants("wv-page-header-toolbar");

				foreach (var node in actionTagHelperList)
				{
					actionsContentHtml += node.InnerHtml.ToString();
				}

				foreach (var node in actionAuxTagHelperList)
				{
					actionsAuxContentHtml += node.InnerHtml.ToString();
				}

				foreach (var node in toolbarTagHelperList)
				{
					toolbarContentHtml += node.InnerHtml.ToString();
				}


				output.TagName = "div";
				output.AddCssClass($"pc-page-header {(!String.IsNullOrWhiteSpace(toolbarContentHtml) ? "has-toolbar" : "")} {(!String.IsNullOrWhiteSpace(ReturnUrl) ? "has-btn-back" : "")} {(String.IsNullOrWhiteSpace(IconClass) ? "no-icon" : "")}");
                output.Attributes.Add("id", $"wv-{elementId}");

				if (!String.IsNullOrWhiteSpace(ReturnUrl))
				{
					var backBtnEl = new TagBuilder("a");
					backBtnEl.AddCssClass("btn btn-sm btn-outline-secondary btn-back");
					backBtnEl.Attributes.Add("href", ReturnUrl);
					var backBtnIconEl = new TagBuilder("span");
					backBtnIconEl.AddCssClass("fa fa-arrow-left");
					backBtnEl.InnerHtml.AppendHtml(backBtnIconEl);
					output.Content.AppendHtml(backBtnEl);
				}

				var rowEl = new TagBuilder("div");
				rowEl.AddCssClass("row m-0 no-gutters");

				var metaColEl = new TagBuilder("div");
				metaColEl.AddCssClass("col-md");
				var metaLabelAuxEl = new TagBuilder("div");
				metaLabelAuxEl.AddCssClass("meta-label-aux");
				if (!String.IsNullOrWhiteSpace(Color))
				{
					metaLabelAuxEl.Attributes.Add("style", $"color:{Color};");
				}

				var metaLabelAuxTextEl = new TagBuilder("span");
				metaLabelAuxTextEl.AddCssClass("text");
				metaLabelAuxTextEl.InnerHtml.AppendHtml(AreaLabel);
				metaLabelAuxEl.InnerHtml.AppendHtml(metaLabelAuxTextEl);

				if (!String.IsNullOrWhiteSpace(AreaSubLabel))
				{
					var metaSubLabelDividerAuxTextEl = new TagBuilder("span");
					metaSubLabelDividerAuxTextEl.AddCssClass("divider");
					metaSubLabelDividerAuxTextEl.InnerHtml.AppendHtml("/");
					metaLabelAuxEl.InnerHtml.AppendHtml(metaSubLabelDividerAuxTextEl);

					var metaSubLabelAuxTextEl = new TagBuilder("span");
					metaSubLabelAuxTextEl.AddCssClass("text");
					metaSubLabelAuxTextEl.InnerHtml.AppendHtml(AreaSubLabel);
					metaLabelAuxEl.InnerHtml.AppendHtml(metaSubLabelAuxTextEl);
				}

				metaColEl.InnerHtml.AppendHtml(metaLabelAuxEl);

				var metaLabelEl = new TagBuilder("div");
				metaLabelEl.AddCssClass("meta-label");

				if (!String.IsNullOrWhiteSpace(IconClass))
				{
					var metaLabelIconWrapperEl = new TagBuilder("span");
					metaLabelIconWrapperEl.AddCssClass("meta-icon");
					if (!String.IsNullOrWhiteSpace(Color))
					{
						metaLabelIconWrapperEl.Attributes.Add("style", $"background-color:{Color};");
					}
					var metaLabelIconEl = new TagBuilder("span");
					metaLabelIconEl.AddCssClass(IconClass);
					if (!String.IsNullOrWhiteSpace(IconColor))
					{
						metaLabelIconEl.Attributes.Add("style", $"color:{IconColor};");
					}
					metaLabelIconWrapperEl.InnerHtml.AppendHtml(metaLabelIconEl);
					metaLabelEl.InnerHtml.AppendHtml(metaLabelIconWrapperEl);
				}

                var metaLabelTextEl = new TagBuilder("span");
                metaLabelTextEl.AddCssClass("text");

                if (PageSwitchItems.Count > 1)
                {
                    //If only the current page there is no switch needed
                    var switchDropdownEl = new TagBuilder("div");
                    switchDropdownEl.AddCssClass("dropdown");
                    switchDropdownEl.AddCssClass("d-inline-block");

                    //link
                    var metaSubLabelTextEl = new TagBuilder("a");
                    metaSubLabelTextEl.AddCssClass("page-switch");
                    //metaSubLabelTextEl.AddCssClass("dropdown-toggle");
                    metaSubLabelTextEl.Attributes.Add("data-toggle", "dropdown");
                    metaSubLabelTextEl.Attributes.Add("href", "#");
                    //metaSubLabelTextEl.InnerHtml.AppendHtml("switch");
                    metaSubLabelTextEl.InnerHtml.AppendHtml("<i class='icon fas fa-ellipsis-v'></i>" + Title);
                    switchDropdownEl.InnerHtml.AppendHtml(metaSubLabelTextEl);

                    //Dropdown
                    var switchDDMenuEl = new TagBuilder("div");
                    switchDDMenuEl.AddCssClass("dropdown-menu");
                    foreach (var pageSwitchItem in PageSwitchItems)
                    {
                        var switchItemEl = new TagBuilder("a");
                        switchItemEl.AddCssClass("dropdown-item pl-2 pr-2");
                        if (pageSwitchItem.IsSelected)
                            switchItemEl.InnerHtml.AppendHtml("<i class=\"fas fa-fw fa-angle-right\"></i>");
                        else
                            switchItemEl.InnerHtml.AppendHtml("<i class=\"fa fa-fw\"></i>");

                        switchItemEl.Attributes.Add("href", pageSwitchItem.Url);
                        switchItemEl.InnerHtml.AppendHtml(pageSwitchItem.Label);
                        switchDDMenuEl.InnerHtml.AppendHtml(switchItemEl);
                    }
                    switchDropdownEl.InnerHtml.AppendHtml(switchDDMenuEl);
                    metaLabelTextEl.InnerHtml.AppendHtml(switchDropdownEl);

                    if (!String.IsNullOrWhiteSpace(SubTitle))
                    {
                        var divider = new TagBuilder("span");
                        divider.AddCssClass("fa fa-angle-right divider");
                        metaLabelTextEl.InnerHtml.AppendHtml(divider);

                        var metaSubLabelTextEl2 = new TagBuilder("span");
                        metaSubLabelTextEl2.AddCssClass("subtext");
                        metaSubLabelTextEl2.InnerHtml.AppendHtml(SubTitle);
                        metaLabelTextEl.InnerHtml.AppendHtml(metaSubLabelTextEl2);
                    }
                }
                else
                {
                    metaLabelTextEl.InnerHtml.AppendHtml(Title);

                    if (!String.IsNullOrWhiteSpace(SubTitle))
                    {
                        var divider = new TagBuilder("span");
                        divider.AddCssClass("fa fa-angle-right divider");
                        metaLabelTextEl.InnerHtml.AppendHtml(divider);

                        var metaSubLabelTextEl = new TagBuilder("span");
                        metaSubLabelTextEl.AddCssClass("subtext");
                        metaSubLabelTextEl.InnerHtml.AppendHtml(SubTitle);
                        metaLabelTextEl.InnerHtml.AppendHtml(metaSubLabelTextEl);
                    }
                }


				metaLabelEl.InnerHtml.AppendHtml(metaLabelTextEl);
				metaColEl.InnerHtml.AppendHtml(metaLabelEl);

				rowEl.InnerHtml.AppendHtml(metaColEl);

				//Actions
				if (!String.IsNullOrWhiteSpace(actionsContentHtml))
				{
					var actionColEl = new TagBuilder("div");
					actionColEl.AddCssClass("col-md-auto align-self-center");
					actionColEl.InnerHtml.AppendHtml(actionsContentHtml);

					rowEl.InnerHtml.AppendHtml(actionColEl);
				}




				output.Content.AppendHtml(rowEl);

				//Description

				if (!String.IsNullOrWhiteSpace(Description) || !String.IsNullOrWhiteSpace(actionsAuxContentHtml))
				{
					var metaDescriptionWrapperEl = new TagBuilder("div");
					metaDescriptionWrapperEl.AddCssClass("description-wrapper");
					var metaDescriptionRowEl = new TagBuilder("div");
					metaDescriptionRowEl.AddCssClass("row m-0 no-gutters");

					var metaDescriptionLeftColumn = new TagBuilder("div");
					metaDescriptionLeftColumn.AddCssClass("col-md align-self-center");

					var metaDescriptionRightColumn = new TagBuilder("div");
					metaDescriptionRightColumn.AddCssClass("col-md-auto align-self-center");


					if (!String.IsNullOrWhiteSpace(Description))
					{
						var metaDescriptionEl = new TagBuilder("div");
						metaDescriptionEl.AddCssClass("description");
						metaDescriptionEl.InnerHtml.AppendHtml(Description);
						metaDescriptionLeftColumn.InnerHtml.AppendHtml(metaDescriptionEl);
					}

					metaDescriptionRowEl.InnerHtml.AppendHtml(metaDescriptionLeftColumn);

					// Aux actions
					if (!String.IsNullOrWhiteSpace(actionsAuxContentHtml))
					{
						metaDescriptionRightColumn.InnerHtml.AppendHtml(actionsAuxContentHtml);
						metaDescriptionRowEl.InnerHtml.AppendHtml(metaDescriptionRightColumn);
					}

					metaDescriptionWrapperEl.InnerHtml.AppendHtml(metaDescriptionRowEl);
					output.Content.AppendHtml(metaDescriptionWrapperEl);
				}




				//Toolbar
				if (!String.IsNullOrWhiteSpace(toolbarContentHtml))
				{
					var wrapEl = new TagBuilder("div");
					wrapEl.AddCssClass("page-header-toolbar");
					wrapEl.InnerHtml.AppendHtml(toolbarContentHtml);
					output.Content.AppendHtml(wrapEl);
				}


                if (FixOnScroll)
                {
                    var jsCompressor = new JavaScriptCompressor();

                    #region << Init Scripts >>
                    var tagHelperInitialized = false;
                    var scriptFileName = "script.js";
                    if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvPageHeader) + scriptFileName))
                    {
                        var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvPageHeader) + scriptFileName];
                        tagHelperInitialized = tagHelperContext.Initialized;
                    }

                    if (!tagHelperInitialized)
                    {
                        var scriptContent = FileService.GetEmbeddedTextResource("script.js", "WebVella.Erp.Web.TagHelpers.WvPageHeader");
                        var scriptEl = new TagBuilder("script");
                        scriptEl.Attributes.Add("type", "text/javascript");
                        scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
                        //scriptEl.InnerHtml.AppendHtml(scriptContent);
                        output.PostContent.AppendHtml(scriptEl);

                        ViewContext.HttpContext.Items[typeof(WvPageHeader) + scriptFileName] = new WvTagHelperContext()
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
							WebVellaErpWebComponentsPcPageHeader_Init(""{{ElementId}}"");
						});";
                    scriptTemplate = scriptTemplate.Replace("{{ElementId}}", elementId.ToString());
                    initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

                    output.PostContent.AppendHtml(initScript);
                    #endregion
                }
            }
            //return Task.CompletedTask;
        }


	}
}
