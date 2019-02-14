using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.TagHelpers
{

	//[OutputElementHint("div")]
	[HtmlTargetElement("wv-modal")]
	public class WvModal : TagHelper
	{
		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		[HtmlAttributeName("is-visible")]
		public bool isVisible { get; set; } = true;

		[HtmlAttributeName("id")]
		public string Id { get; set; } = "";

		[HtmlAttributeName("title")]
		public string Title { get; set; } = "";

		[HtmlAttributeName("position")]
		public ModalPosition Position { get; set; } = ModalPosition.Top;

		[HtmlAttributeName("size")]
		public ModalSize Size { get; set; } = ModalSize.Normal;

		[HtmlAttributeName("backdrop")]
		public string Backdrop { get; set; } = "true";

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{

			if (!isVisible)
			{
				output.SuppressOutput();
				return Task.CompletedTask;
			}

			output.TagName = "div";
			output.AddCssClass("modal");
			output.Attributes.Add("tabindex","-1");
			output.Attributes.Add("role", "modal");

			if (!String.IsNullOrWhiteSpace(Id))
			{
				output.Attributes.Add("id", Id);
			}
			else {
				output.Attributes.Add("id", "modal-" + Guid.NewGuid());
			}

			if (!String.IsNullOrWhiteSpace(Backdrop) && Backdrop != "true")
			{
				output.Attributes.Add("data-backdrop", Backdrop);
			}


			var modalDialog = new TagBuilder("div");
			modalDialog.AddCssClass("modal-dialog");
			modalDialog.AddCssClass(Position.GetLabel());
			modalDialog.AddCssClass(Size.GetLabel());
			modalDialog.Attributes.Add("role", "document");

			var modalContent = new TagBuilder("div");
			modalContent.AddCssClass("modal-content");

			var modalHeader = new TagBuilder("div");
			modalHeader.AddCssClass("modal-header");
			var modalHeaderTitle = new TagBuilder("h5");
			modalHeaderTitle.AddCssClass("modal-title");
			modalHeaderTitle.InnerHtml.AppendHtml(Title);
			modalHeader.InnerHtml.AppendHtml(modalHeaderTitle);


			output.PreContent.AppendHtml(modalDialog.RenderStartTag());
			output.PreContent.AppendHtml(modalContent.RenderStartTag());

			if (!String.IsNullOrWhiteSpace(Title)){
				output.PreContent.AppendHtml(modalHeader);
			}


			output.PostContent.AppendHtml(modalContent.RenderEndTag());
			output.PostContent.AppendHtml(modalDialog.RenderEndTag());


			var jsCompressor = new JavaScriptCompressor();

			#region << Init Scripts >>
			var tagHelperInitialized = false;
			var scriptFileName = "modal.js";
			if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvModal) + scriptFileName))
			{
				var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvModal) + scriptFileName];
				tagHelperInitialized = tagHelperContext.Initialized;
			}
			if (!tagHelperInitialized)
			{
				var scriptContent = FileService.GetEmbeddedTextResource(scriptFileName, "WebVella.Erp.Web.TagHelpers.WvModal");
				var scriptEl = new TagBuilder("script");
				scriptEl.Attributes.Add("type", "text/javascript");
				scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
				//scriptEl.InnerHtml.AppendHtml(scriptContent);
				output.PostElement.AppendHtml(scriptEl);

				ViewContext.HttpContext.Items[typeof(WvModal) + scriptFileName] = new WvTagHelperContext()
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
							WebVellaErpWebComponentsPcModal_Init(""{{ElementId}}"");
						});";
			scriptTemplate = scriptTemplate.Replace("{{ElementId}}", Id);
			initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

			output.PostContent.AppendHtml(initScript);
			#endregion

			return Task.CompletedTask;
		}
	}
}
