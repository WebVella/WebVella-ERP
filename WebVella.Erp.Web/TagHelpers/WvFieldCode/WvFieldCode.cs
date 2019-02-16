using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.TagHelpers
{
	[HtmlTargetElement("wv-field-code")]
	public class WvFieldCode : WvFieldBase
	{
		[HtmlAttributeName("height")]
		public string Height { get; set; } = "160px";

		[HtmlAttributeName("language")]
		public string Language { get; set; } = "razor";

		[HtmlAttributeName("theme")]
		public string Theme { get; set; } = "cobalt";

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (!isVisible)
			{
				output.SuppressOutput();
				return Task.CompletedTask;
			}

			#region << Init >>
			var initSuccess = InitField(context, output);

			if (!initSuccess)
			{
				return Task.CompletedTask;
			}

			#endregion

			#region << Render >>

			var editorWrapperEl = new TagBuilder("div");
			var wrapperCssClassList = new List<string>();
			wrapperCssClassList.Add("form-control-plaintext erp-code");
			if (ValidationErrors.Count > 0)
			{
				wrapperCssClassList.Add("is-invalid");
			}
			editorWrapperEl.Attributes.Add("class", String.Join(' ', wrapperCssClassList));

			var editorWrapper = new TagBuilder("div");
			editorWrapper.Attributes.Add("id", $"ace-{FieldId}");
			editorWrapper.Attributes.Add("style", $"height:{Height}");
			editorWrapperEl.InnerHtml.AppendHtml(editorWrapper);

			var hiddenInput = new TagBuilder("input");
			hiddenInput.Attributes.Add("type", "hidden");
			hiddenInput.Attributes.Add("id", $"input-{FieldId}");
			hiddenInput.Attributes.Add("name", Name);
			hiddenInput.Attributes.Add("value", (Value ?? "").ToString());
			editorWrapperEl.InnerHtml.AppendHtml(hiddenInput);

			output.Content.AppendHtml(editorWrapperEl);

			var jsCompressor = new JavaScriptCompressor();

			#region << Add Ace lib >>
			{
				var tagHelperInitialized = false;
				var fileName = "ace.js";
				if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldCode) + fileName))
				{
					var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldCode) + fileName];
					tagHelperInitialized = tagHelperContext.Initialized;
				}
				if (!tagHelperInitialized)
				{
					var scriptEl = new TagBuilder("script");
					scriptEl.Attributes.Add("type", "text/javascript");
					scriptEl.Attributes.Add("src", "/lib/ace/ace.js");
					output.PostContent.AppendHtml(scriptEl);

					ViewContext.HttpContext.Items[typeof(WvFieldCode) + fileName] = new WvTagHelperContext()
					{
						Initialized = true
					};

				}
			}
			#endregion

			#region << Init Ace script >>
			{
				var tagHelperInitialized = false;
				var fileName = "form.js";
				if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldCode) + fileName))
				{
					var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldCode) + fileName];
					tagHelperInitialized = tagHelperContext.Initialized;
				}
				if (!tagHelperInitialized)
				{
					var scriptContent = FileService.GetEmbeddedTextResource(fileName, "WebVella.Erp.Web.TagHelpers.WvFieldCode");
					var scriptEl = new TagBuilder("script");
					scriptEl.Attributes.Add("type", "text/javascript");
					scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
					//scriptEl.InnerHtml.AppendHtml(scriptContent);
					output.PostContent.AppendHtml(scriptEl);

					ViewContext.HttpContext.Items[typeof(WvFieldCode) + fileName] = new WvTagHelperContext()
					{
						Initialized = true
					};

				}
			}
			#endregion

			#region << Add Inline Init Script for this instance >>
			var initScript = new TagBuilder("script");
			initScript.Attributes.Add("type", "text/javascript");
			var scriptTemplate = @"
					$(function(){
						CodeFormInit(""{{FieldId}}"",""{{Name}}"",""{{EntityName}}"",""{{RecordId}}"",{{ConfigJson}});
					});";

			scriptTemplate = scriptTemplate.Replace("{{FieldId}}", (FieldId ?? null).ToString());
			scriptTemplate = scriptTemplate.Replace("{{Name}}", Name);
			scriptTemplate = scriptTemplate.Replace("{{EntityName}}", EntityName);
			scriptTemplate = scriptTemplate.Replace("{{RecordId}}", (RecordId ?? null).ToString());

			var fieldConfig = new WvFieldCodeConfig()
			{
				ApiUrl = ApiUrl,
				CanAddValues = Access == FieldAccess.FullAndCreate ? true : false,
				Language = Language,
				Theme = Theme,
				ReadOnly = Mode == FieldRenderMode.Display
			};
			scriptTemplate = scriptTemplate.Replace("{{ConfigJson}}", JsonConvert.SerializeObject(fieldConfig));

			initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

			output.PostContent.AppendHtml(initScript);
			#endregion


			#endregion



			//Finally
			if (SubInputEl != null)
			{
				output.PostContent.AppendHtml(SubInputEl);
			}

			return Task.CompletedTask;
		}
	}
}
