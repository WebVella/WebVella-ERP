using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.TagHelpers
{
	[HtmlTargetElement("wv-form")]
	public class WvForm : TagHelper
	{
		public WvForm(IHtmlGenerator generator)
		{
			Generator = generator;
		}

		protected IHtmlGenerator Generator { get; }

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		[HtmlAttributeName("is-visible")]
		public bool isVisible { get; set; } = true;

		[HtmlAttributeName("id")]
		public string Id { get; set; } = "";

		[HtmlAttributeName("antiforgery")]
		public bool Antiforgery { get; set; } = true;

		[HtmlAttributeName("name")]
		public string Name { get; set; } = "form";

		[HtmlAttributeName("action")]
		public string Action { get; set; } = "";

		[HtmlAttributeName("enctype")]
		public string Enctype { get; set; } = "";
		//Specifies how the form-data should be encoded when submitting it to the server (only for method="post")
		//application/x-www-form-urlencoded
		//multipart/form-data
		//text/plain


		[HtmlAttributeName("target")]
		public string Target { get; set; } = "";
		//Specifies where to display the response that is received after submitting the form
		//_blank
		//_self
		//_parent
		//_top

		[HtmlAttributeName("method")]
		public string Method { get; set; } = "post";

		[HtmlAttributeName("novalidate")]
		public bool NoValidate { get; set; } = true;

		[HtmlAttributeName("autocomplete")]
		public bool AutoComplete { get; set; } = true;

		[HtmlAttributeName("accept-charset")]
		public string AcceptCharset { get; set; } = "";

		[HtmlAttributeName("validation")]
		public ValidationException Validation { get; set; } = null;

		[HtmlAttributeName("label-mode")]
		public LabelRenderMode LabelMode { get; set; } = LabelRenderMode.Undefined; //To be inherited

		[HtmlAttributeName("mode")]
		public FieldRenderMode Mode { get; set; } = FieldRenderMode.Undefined; //To be inherited

		[HtmlAttributeName("class")]
		public string Class { get; set; } = "";

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (!isVisible)
			{
				output.SuppressOutput();
				return Task.CompletedTask;
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

			if (Mode == FieldRenderMode.Undefined)
			{
				//Check if it is defined in form group
				if (context.Items.ContainsKey(typeof(FieldRenderMode)))
				{
					Mode = (FieldRenderMode)context.Items[typeof(FieldRenderMode)];
				}
				else
				{
					Mode = FieldRenderMode.Form;
				}
			}
			#endregion

			if (Mode == FieldRenderMode.Form)
			{
				output.TagName = "form";
				if (!String.IsNullOrWhiteSpace(Id))
				{
					output.Attributes.Add("id", Id);
				}

				if (!String.IsNullOrWhiteSpace(Class))
				{
					output.AddCssClass(Class);
				}

				if (!String.IsNullOrWhiteSpace(Name))
				{
					output.Attributes.Add("name", Name);
				}
				if (!String.IsNullOrWhiteSpace(Action))
				{
					output.Attributes.Add("action", Action);
				}

				if (!String.IsNullOrWhiteSpace(Enctype))
				{
					output.Attributes.Add("enctype", Enctype);
				}

				if (!String.IsNullOrWhiteSpace(Target))
				{
					output.Attributes.Add("target", Target);
				}
				switch (Method.ToLowerInvariant())
				{
					case "get":
						output.Attributes.Add("method", Method);
						break;
					default:
						output.Attributes.Add("method", "post");
						break;
				}

				if (NoValidate)
				{
					output.Attributes.Add("novalidate", "novalidate");
				}

				if (AutoComplete)
				{
					output.Attributes.Add("autocomplete", "on");
				}
				else
				{
					output.Attributes.Add("autocomplete", "off");
				}

				if (!String.IsNullOrWhiteSpace(AcceptCharset))
				{
					output.Attributes.Add("accept-charset", AcceptCharset);
				}

				if (Antiforgery && Method != "get")
				{
					var antiforgeryTag = Generator.GenerateAntiforgery(ViewContext);
					if (antiforgeryTag != null)
					{
						output.PreContent.AppendHtml(antiforgeryTag);
					}
				}

				if (Validation != null)
				{
					context.Items[typeof(ValidationException)] = Validation;
				}

				var jsCompressor = new JavaScriptCompressor();

				#region << Init Scripts >>
				var tagHelperInitialized = false;
				var scriptFileName = "form.js";
				if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvForm) + scriptFileName))
				{
					var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvForm) + scriptFileName];
					tagHelperInitialized = tagHelperContext.Initialized;
				}
				if (!tagHelperInitialized && !String.IsNullOrEmpty(Name))
				{
					var scriptContent = FileService.GetEmbeddedTextResource(scriptFileName, "WebVella.Erp.Web.TagHelpers.WvForm");
					scriptContent = scriptContent.Replace("{{FormName}}", Name);
					scriptContent = scriptContent.Replace("{{ElementId}}", Id);

					var scriptEl = new TagBuilder("script");
					scriptEl.Attributes.Add("type", "text/javascript");
					scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
					//scriptEl.InnerHtml.AppendHtml(scriptContent);
					output.PostElement.AppendHtml(scriptEl);

					ViewContext.HttpContext.Items[typeof(WvForm) + scriptFileName] = new WvTagHelperContext()
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
							WebVellaErpWebComponentsPcForm_Init(""{{ElementId}}"",""{{FormName}}"");
						});";
				scriptTemplate = scriptTemplate.Replace("{{ElementId}}", Id);
				scriptTemplate = scriptTemplate.Replace("{{FormName}}", Name);
				initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

				output.PostContent.AppendHtml(initScript);
				#endregion
			}
			else
			{
				output.TagName = "div";
				output.AddCssClass("form");
			}

			context.Items[typeof(LabelRenderMode)] = LabelMode;
			context.Items[typeof(FieldRenderMode)] = Mode;
			context.Items["FromAutocomplete"] = AutoComplete;
			return Task.CompletedTask;
		}
	}
}
