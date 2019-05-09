using HtmlAgilityPack;
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

namespace WebVella.Erp.Web.TagHelpers
{
	[HtmlTargetElement("wv-field-html")]
	[RestrictChildren("wv-field-prepend", "wv-field-append")]
	public class WvFieldHtml : WvFieldBase
	{
		[HtmlAttributeName("upload-mode")]
		public HtmlUploadMode UploadMode { get; set; } = HtmlUploadMode.None;

		[HtmlAttributeName("toolbar-mode")]
		public HtmlToolbarMode ToolbarMode { get; set; } = HtmlToolbarMode.Basic;

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (!isVisible)
			{
				output.SuppressOutput();
				return;
			}
			#region << Init >>
			var initSuccess = InitField(context, output);

			if (!initSuccess)
			{
				return;
			}

			#region << Init Prepend and Append >>
			var content = await output.GetChildContentAsync();
			var htmlDoc = new HtmlDocument();
			htmlDoc.LoadHtml(content.GetContent());
			var prependTaghelper = htmlDoc.DocumentNode.Descendants("wv-field-prepend");
			var appendTagHelper = htmlDoc.DocumentNode.Descendants("wv-field-append");

			foreach (var node in prependTaghelper)
			{
				PrependHtml.Add(node.InnerHtml.ToString());
			}

			foreach (var node in appendTagHelper)
			{
				AppendHtml.Add(node.InnerHtml.ToString());
			}

			#endregion

			#endregion



			#region << Render >>
			if (Mode == FieldRenderMode.Form)
			{
				if (Access == FieldAccess.Full || Access == FieldAccess.FullAndCreate)
				{
					var inputEl = new TagBuilder("textarea");
					inputEl.Attributes.Add("id", $"input-{FieldId}");
					inputEl.Attributes.Add("name", Name);
					var inputElCssClassList = new List<string>();
					inputElCssClassList.Add("form-control erp-html");

					if (Required)
					{
						inputEl.Attributes.Add("required", null);
					}

					if (ValidationErrors.Count > 0)
					{
						inputElCssClassList.Add("is-invalid");
					}

					inputEl.InnerHtml.Append((Value ?? "").ToString());

					inputEl.Attributes.Add("class", String.Join(' ', inputElCssClassList));

					output.Content.AppendHtml(inputEl);

					var jsCompressor = new JavaScriptCompressor();
					#region << Init Scripts >>
					var tagHelperInitialized = false;
					if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldHtml) + "-form"))
					{
						var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldHtml) + "-form"];
						tagHelperInitialized = tagHelperContext.Initialized;
					}
					if (!tagHelperInitialized)
					{
						var scriptContent = FileService.GetEmbeddedTextResource("form.js", "WebVella.Erp.Web.TagHelpers.WvFieldHtml");
						var scriptEl = new TagBuilder("script");
						scriptEl.Attributes.Add("type", "text/javascript");
						scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
						//scriptEl.InnerHtml.AppendHtml(scriptContent);
						output.PostContent.AppendHtml(scriptEl);

						ViewContext.HttpContext.Items[typeof(WvFieldHtml) + "-form"] = new WvTagHelperContext()
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
							InitHtmlFieldCKEditor(""{{FieldId}}"",{{ConfigJson}});
						});";
					scriptTemplate = scriptTemplate.Replace("{{FieldId}}", (FieldId ?? null).ToString());

					var fieldConfig = new WvFieldHtmlConfig()
					{
						ApiUrl = ApiUrl,
						CanAddValues = Access == FieldAccess.FullAndCreate ? true : false,
						UploadMode = UploadMode,
						ToolbarMode = ToolbarMode
					};

					scriptTemplate = scriptTemplate.Replace("{{ConfigJson}}", JsonConvert.SerializeObject(fieldConfig));

					initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

					output.PostContent.AppendHtml(initScript);
					#endregion

				}
				else if (Access == FieldAccess.ReadOnly)
				{
					var inputGroupEl = new TagBuilder("div");
					inputGroupEl.AddCssClass("input-group");
					//Prepend
					if (PrependHtml.Count > 0)
					{
						var prependEl = new TagBuilder("span");
						prependEl.AddCssClass($"input-group-prepend erp-multilinetext {(ValidationErrors.Count > 0 ? "is-invalid" : "")}");
						foreach (var htmlString in PrependHtml)
						{
							prependEl.InnerHtml.AppendHtml(htmlString);
						}
						inputGroupEl.InnerHtml.AppendHtml(prependEl);
					}
					//Control
					var inputEl = new TagBuilder("div");
					inputEl.Attributes.Add("id", $"plaintext-{FieldId}");
					inputEl.AddCssClass("form-control erp-html disabled");
					inputEl.Attributes.Add("style", "height:auto;");
					inputEl.InnerHtml.AppendHtml((Value ?? "").ToString());
					inputGroupEl.InnerHtml.AppendHtml(inputEl);
					//Append
					if (AppendHtml.Count > 0)
					{
						var appendEl = new TagBuilder("span");
						appendEl.AddCssClass($"input-group-append erp-multilinetext {(ValidationErrors.Count > 0 ? "is-invalid" : "")}");

						foreach (var htmlString in AppendHtml)
						{
							appendEl.InnerHtml.AppendHtml(htmlString);
						}
						inputGroupEl.InnerHtml.AppendHtml(appendEl);
					}
					//Hidden input with the value
					var hiddenInput = new TagBuilder("input");
					hiddenInput.Attributes.Add("type", "hidden");
					hiddenInput.Attributes.Add("id", $"input-{FieldId}");
					hiddenInput.Attributes.Add("name", Name);
					hiddenInput.Attributes.Add("value", (Value ?? "").ToString());
					output.Content.AppendHtml(hiddenInput);

					output.Content.AppendHtml(inputGroupEl);
				}
			}
			else if (Mode == FieldRenderMode.Display)
			{

				if (!String.IsNullOrWhiteSpace(Value))
				{
					var divEl = new TagBuilder("div");
					divEl.Attributes.Add("id", $"input-{FieldId}");
					divEl.AddCssClass("form-control-plaintext erp-html");
					divEl.InnerHtml.AppendHtml((Value ?? "").ToString());
					output.Content.AppendHtml(divEl);
				}
				else
				{
					output.Content.AppendHtml(EmptyValEl);
				}
			}
			else if (Mode == FieldRenderMode.Simple)
			{
				output.SuppressOutput();
				output.Content.AppendHtml((Value ?? "").ToString());
				return;
			}
			else if (Mode == FieldRenderMode.InlineEdit)
			{
				if (Access == FieldAccess.Full || Access == FieldAccess.FullAndCreate)
				{
					#region << View Wrapper >>
					{
						var viewWrapperEl = new TagBuilder("div");
						viewWrapperEl.AddCssClass("input-group view-wrapper");
						viewWrapperEl.Attributes.Add("title", "double click to edit");
						viewWrapperEl.Attributes.Add("id", $"view-{FieldId}");
						//Prepend
						if (PrependHtml.Count > 0)
						{
							var viewInputPrepend = new TagBuilder("span");
							viewInputPrepend.AddCssClass("input-group-prepend erp-multilinetext");
							foreach (var htmlString in PrependHtml)
							{
								viewInputPrepend.InnerHtml.AppendHtml(htmlString);
							}
							viewWrapperEl.InnerHtml.AppendHtml(viewInputPrepend);
						}
						//Control

						var viewFormControlEl = new TagBuilder("div");
						viewFormControlEl.AddCssClass("form-control erp-html");
						viewFormControlEl.Attributes.Add("style","height:auto;");
						viewFormControlEl.InnerHtml.AppendHtml((Value ?? "").ToString());
						viewWrapperEl.InnerHtml.AppendHtml(viewFormControlEl);

						//Append
						var viewInputActionEl = new TagBuilder("span");
						viewInputActionEl.AddCssClass("input-group-append erp-multilinetext");
						foreach (var htmlString in AppendHtml)
						{
							viewInputActionEl.InnerHtml.AppendHtml(htmlString);
						}
						viewInputActionEl.InnerHtml.AppendHtml("<button type=\"button\" class='btn btn-white action' title='edit'><i class='fa fa-fw fa-pencil-alt'></i></button>");
						viewWrapperEl.InnerHtml.AppendHtml(viewInputActionEl);
						output.Content.AppendHtml(viewWrapperEl);
					}
					#endregion

					#region << Edit Modal >>
					{
						var editModalEl = new TagBuilder("div");
						editModalEl.AddCssClass("modal");
						editModalEl.Attributes.Add("id", $"edit-{FieldId}");
						editModalEl.Attributes.Add("tabindex", "-1");
						var editModalDialog = new TagBuilder("div");
						editModalDialog.AddCssClass("modal-dialog modal-lg");
						var editModalContent = new TagBuilder("div");
						editModalContent.AddCssClass("modal-content");

						var editModalHeader = new TagBuilder("div");
						editModalHeader.AddCssClass("modal-header");
						var editModalHeaderTitle = new TagBuilder("h5");
						editModalHeaderTitle.AddCssClass("modal-title");
						editModalHeaderTitle.InnerHtml.Append("edit field ");
						var editModalHeaderTitleSpan = new TagBuilder("span");
						editModalHeaderTitleSpan.AddCssClass("go-green");
						editModalHeaderTitleSpan.InnerHtml.Append(Name);
						editModalHeaderTitle.InnerHtml.AppendHtml(editModalHeaderTitleSpan);
						editModalHeader.InnerHtml.AppendHtml(editModalHeaderTitle);
						var editModalHeaderButton = new TagBuilder("button");
						editModalHeaderButton.Attributes.Add("type", "button");
						editModalHeaderButton.AddCssClass("close");
						editModalHeaderButton.Attributes.Add("data-dismiss", "modal");
						editModalHeaderButton.InnerHtml.AppendHtml(new TagBuilder("span").InnerHtml.AppendHtml("&times;"));
						editModalHeader.InnerHtml.AppendHtml(editModalHeaderButton);
						editModalContent.InnerHtml.AppendHtml(editModalHeader);

						var editModalBody = new TagBuilder("div");
						editModalBody.AddCssClass("modal-body");
						var editModalBodyTextArea = new TagBuilder("textarea");
						editModalBodyTextArea.Attributes.Add("id", $"input-{FieldId}");
						editModalBodyTextArea.AddCssClass("form-control erp-html");
						if (Required)
						{
							editModalBodyTextArea.Attributes.Add("required", null);
						}
						editModalBodyTextArea.InnerHtml.AppendHtml((Value ?? "").ToString());
						editModalBody.InnerHtml.AppendHtml(editModalBodyTextArea);
						editModalContent.InnerHtml.AppendHtml(editModalBody);

						var editModalFooter = new TagBuilder("div");
						editModalFooter.AddCssClass("modal-footer");
						var editModalFooterSave = new TagBuilder("button");
						editModalFooterSave.Attributes.Add("type", "button");
						editModalFooterSave.AddCssClass("btn btn-primary save btn-sm");
						var editModalFooterSaveIcon = new TagBuilder("span");
						editModalFooterSaveIcon.AddCssClass("fa fa-check");
						editModalFooterSave.InnerHtml.AppendHtml(editModalFooterSaveIcon);
						editModalFooterSave.InnerHtml.AppendHtml(" save");
						editModalFooter.InnerHtml.AppendHtml(editModalFooterSave);
						var editModalFooterCancel = new TagBuilder("button");
						editModalFooterCancel.Attributes.Add("type", "button");
						editModalFooterCancel.AddCssClass("btn btn-secondary cancel btn-sm");
						editModalFooterCancel.InnerHtml.Append("cancel");
						editModalFooter.InnerHtml.AppendHtml(editModalFooterCancel);
						editModalContent.InnerHtml.AppendHtml(editModalFooter);

						editModalDialog.InnerHtml.AppendHtml(editModalContent);
						editModalEl.InnerHtml.AppendHtml(editModalDialog);

						output.Content.AppendHtml(editModalEl);
					}
					#endregion
					var jsCompressor = new JavaScriptCompressor();

					#region << Init Scripts >>
					var tagHelperInitialized = false;
					if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldHtml) + "-inline-edit"))
					{
						var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldHtml) + "-inline-edit"];
						tagHelperInitialized = tagHelperContext.Initialized;
					}
					if (!tagHelperInitialized)
					{
						var scriptContent = FileService.GetEmbeddedTextResource("inline-edit.js", "WebVella.Erp.Web.TagHelpers.WvFieldHtml");
						var scriptEl = new TagBuilder("script");
						scriptEl.Attributes.Add("type", "text/javascript");
						scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
						output.PostContent.AppendHtml(scriptEl);

						ViewContext.HttpContext.Items[typeof(WvFieldHtml) + "-inline-edit"] = new WvTagHelperContext()
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
							HtmlInlineEditInit(""{{FieldId}}"",""{{Name}}"",""{{EntityName}}"",""{{RecordId}}"",{{ConfigJson}});
						});";
					scriptTemplate = scriptTemplate.Replace("{{FieldId}}", (FieldId ?? null).ToString());
					scriptTemplate = scriptTemplate.Replace("{{Name}}", Name);
					scriptTemplate = scriptTemplate.Replace("{{EntityName}}", EntityName);
					scriptTemplate = scriptTemplate.Replace("{{RecordId}}", (RecordId ?? null).ToString());

					var fieldConfig = new WvFieldHtmlConfig()
					{
						ApiUrl = ApiUrl,
						CanAddValues = Access == FieldAccess.FullAndCreate ? true : false,
						UploadMode = UploadMode,
						ToolbarMode = ToolbarMode
					};

					scriptTemplate = scriptTemplate.Replace("{{ConfigJson}}", JsonConvert.SerializeObject(fieldConfig));

					initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

					output.PostContent.AppendHtml(initScript);
					#endregion
				}
				else if (Access == FieldAccess.ReadOnly)
				{

					var divEl = new TagBuilder("div");
					divEl.AddCssClass("input-group");
					//Prepend
					if (PrependHtml.Count > 0)
					{
						var viewInputPrepend = new TagBuilder("span");
						viewInputPrepend.AddCssClass("input-group-prepend  erp-multilinetext");
						foreach (var htmlString in PrependHtml)
						{
							viewInputPrepend.InnerHtml.AppendHtml(htmlString);
						}
						divEl.InnerHtml.AppendHtml(viewInputPrepend);
					}
					//Control
					var inputEl = new TagBuilder("div");
					inputEl.AddCssClass("form-control erp-html disabled");
					inputEl.InnerHtml.AppendHtml((Value ?? "").ToString());
					divEl.InnerHtml.AppendHtml(inputEl);
					//Append
					var appendActionSpan = new TagBuilder("span");
					appendActionSpan.AddCssClass("input-group-append  erp-multilinetext");
					foreach (var htmlString in AppendHtml)
					{
						appendActionSpan.InnerHtml.AppendHtml(htmlString);
					}
					appendActionSpan.InnerHtml.AppendHtml("<button type=\"button\" disabled class='btn btn-white action' title='locked'><i class='fa fa-fw fa-lock'></i></button>");
					divEl.InnerHtml.AppendHtml(appendActionSpan);
					output.Content.AppendHtml(divEl);
				}
			}
			#endregion


			//Finally
			if (SubInputEl != null)
			{
				output.PostContent.AppendHtml(SubInputEl);
			}

			return;
		}

	}
}
