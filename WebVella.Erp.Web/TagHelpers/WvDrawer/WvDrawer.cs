using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Api;
using WebVella.Erp.Utilities;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.TagHelpers.Models;
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.TagHelpers
{
	[HtmlTargetElement("wv-drawer")]
	public class WvDrawer : TagHelper
	{
		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		[HtmlAttributeName("is-visible")]
		public bool isVisible { get; set; } = true;

		[HtmlAttributeName("id")]
		public string Id { get; set; } = "";

		[HtmlAttributeName("width")]
		public string Width { get; set; } = "";

		[HtmlAttributeName("title")]
		public string Title { get; set; } = "";

		[HtmlAttributeName("title-action-html")]
		public string TitleActionHtml { get; set; } = "";

		[HtmlAttributeName("class")]
		public string Class { get; set; } = "";

		[HtmlAttributeName("body-class")]
		public string BodyClass { get; set; } = "";

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (!isVisible)
			{
				output.SuppressOutput();
				return Task.CompletedTask;
			}

			#region << Render >>
			output.TagName = "div";
			output.AddCssClass("drawer");
			if (!String.IsNullOrWhiteSpace(Class))
			{
				output.AddCssClass(Class);
			}
			if (!String.IsNullOrWhiteSpace(Id))
			{
				output.Attributes.Add("id", Id);
			}
			if (!String.IsNullOrWhiteSpace(Width)) {
				output.Attributes.Add("style", $"width:{Width}; ");
			}
			if (!String.IsNullOrWhiteSpace(Title))
			{
				var dHeadEl = new TagBuilder("div");
				dHeadEl.AddCssClass("drawer-header");

				var btnCloseEl = new TagBuilder("button");
				btnCloseEl.Attributes.Add("type", "button");
				btnCloseEl.AddCssClass("drawer-close");
				if (String.IsNullOrWhiteSpace(Id))
				{
					btnCloseEl.Attributes.Add("onclick", "ErpEvent.DISPATCH('WebVella.Erp.Web.Components.WvDrawer','close')");
				}
				else {
					btnCloseEl.Attributes.Add("onclick", "ErpEvent.DISPATCH('WebVella.Erp.Web.Components.WvDrawer',{htmlId:'" + Id + "',action:'close',payload:null})");
				}


				var btnCloseIconEl = new TagBuilder("span");
				btnCloseIconEl.AddCssClass("fa fa-times fa-fw");
				btnCloseEl.InnerHtml.AppendHtml(btnCloseIconEl);

				dHeadEl.InnerHtml.AppendHtml(btnCloseEl);

				var headTitle = new TagBuilder("span");
				headTitle.AddCssClass("title");
				headTitle.InnerHtml.AppendHtml(Title);
				dHeadEl.InnerHtml.AppendHtml(headTitle);

				if (!String.IsNullOrWhiteSpace(TitleActionHtml)){
					var actionWrapEl = new TagBuilder("div");
					actionWrapEl.AddCssClass("drawer-header-action");
					actionWrapEl.InnerHtml.AppendHtml(TitleActionHtml);
					dHeadEl.InnerHtml.AppendHtml(actionWrapEl);
				}

				output.PreContent.AppendHtml(dHeadEl);

				var contentEl = new TagBuilder("div");
				contentEl.AddCssClass("drawer-content");
				if (!String.IsNullOrWhiteSpace(BodyClass))
				{
					contentEl.AddCssClass(BodyClass);
				}

				output.PreContent.AppendHtml(contentEl.RenderStartTag());

				output.PostContent.AppendHtml(contentEl.RenderEndTag());
			}
			#endregion

			var jsCompressor = new JavaScriptCompressor();

			#region << Init Scripts >>
			var tagHelperInitialized = false;
			var scriptFileName = "drawer.js";
			if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvDrawer) + scriptFileName))
			{
				var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvDrawer) + scriptFileName];
				tagHelperInitialized = tagHelperContext.Initialized;
			}
			if (!tagHelperInitialized)
			{
				var scriptContent = FileService.GetEmbeddedTextResource(scriptFileName, "WebVella.Erp.Web.TagHelpers.WvDrawer");
				var scriptEl = new TagBuilder("script");
				scriptEl.Attributes.Add("type", "text/javascript");
				//scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
				scriptEl.InnerHtml.AppendHtml(scriptContent);
				output.PostElement.AppendHtml(scriptEl);

				ViewContext.HttpContext.Items[typeof(WvDrawer) + scriptFileName] = new WvTagHelperContext()
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
							WebVellaErpWebComponentsPcDrawer_Init(""{{ElementId}}"");
						});";
			scriptTemplate = scriptTemplate.Replace("{{ElementId}}", Id);
			initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

			output.PostContent.AppendHtml(initScript);
			#endregion

			return Task.CompletedTask;
		}
	}
}
