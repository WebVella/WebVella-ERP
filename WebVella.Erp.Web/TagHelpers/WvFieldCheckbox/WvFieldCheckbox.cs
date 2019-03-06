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

	[HtmlTargetElement("wv-field-checkbox")]
	public class WvFieldCheckbox : WvFieldBase
	{
		[HtmlAttributeName("text-true")]
		public string TextTrue { get; set; } = "";

		[HtmlAttributeName("text-false")]
		public string TextFalse { get; set; } = "";

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

			if (String.IsNullOrWhiteSpace(TextTrue)){
				TextTrue = "selected";
			}

			if (String.IsNullOrWhiteSpace(TextFalse))
			{
				TextFalse = "not selected";
			}

			#endregion


			#region << Render >>
			if (Mode == FieldRenderMode.Form)
			{
				var wrapper1 = new TagBuilder("div");
				wrapper1.AddCssClass("form-control-plaintext erp-checkbox");
				var wrapper2 = new TagBuilder("div");
				wrapper2.AddCssClass("form-check");
				var labelWrapper = new TagBuilder("label");
				labelWrapper.AddCssClass("form-check-label");

				var inputHidden = new TagBuilder("input");
				inputHidden.Attributes.Add("data-source-id", $"input-{FieldId}");
				inputHidden.Attributes.Add("type", "hidden");
				inputHidden.Attributes.Add("name", Name);
				inputHidden.Attributes.Add("value", (Value ?? "").ToString().ToLowerInvariant());
				labelWrapper.InnerHtml.AppendHtml(inputHidden);

				var inputFake = new TagBuilder("input");
				inputFake.Attributes.Add("id", $"input-{FieldId}");
				inputFake.Attributes.Add("type", "checkbox");
				inputFake.Attributes.Add("value", "true");
				inputFake.Attributes.Add("data-field-name", Name);
				if (Access == FieldAccess.ReadOnly)
				{
					inputFake.Attributes.Add("readonly", null);
				}

				inputFake.AddCssClass($"form-check-input {(ValidationErrors.Count > 0 ? "is-invalid" : "")}");

				if (Value != null && Value)
				{
					inputFake.Attributes.Add("checked", "checked");
				}
				labelWrapper.InnerHtml.AppendHtml(inputFake);

				labelWrapper.InnerHtml.AppendHtml(TextTrue);
				wrapper2.InnerHtml.AppendHtml(labelWrapper);
				wrapper1.InnerHtml.AppendHtml(wrapper2);

				output.Content.AppendHtml(wrapper1);


				var jsCompressor = new JavaScriptCompressor();
				#region << Init Scripts >>
				var tagHelperInitialized = false;
				if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldCheckbox) + "-form"))
				{
					var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldCheckbox) + "-form"];
					tagHelperInitialized = tagHelperContext.Initialized;
				}
				if (!tagHelperInitialized)
				{
					var scriptContent = FileService.GetEmbeddedTextResource("form.js", "WebVella.Erp.Web.TagHelpers.WvFieldCheckbox");
					var scriptEl = new TagBuilder("script");
					scriptEl.Attributes.Add("type", "text/javascript");
					scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
					output.PostContent.AppendHtml(scriptEl);

					ViewContext.HttpContext.Items[typeof(WvFieldCheckbox) + "-form"] = new WvTagHelperContext()
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
							CheckboxFormInit(""{{FieldId}}"");
						});";
				scriptTemplate = scriptTemplate.Replace("{{FieldId}}", (FieldId ?? null).ToString());

				initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

				output.PostContent.AppendHtml(initScript);
				#endregion

			}
			else if (Mode == FieldRenderMode.Display)
			{
				var wrapper = new TagBuilder("div");
				wrapper.Attributes.Add("id", $"input-{FieldId}");
				wrapper.AddCssClass("form-control-plaintext erp-checkbox");
				if (!String.IsNullOrWhiteSpace(Name))
				{
					wrapper.Attributes.Add("data-field-name", Name);
				}
				wrapper.Attributes.Add("data-field-value", (Value ?? "").ToString().ToLowerInvariant());

				if (Value != null)
				{
					var iconEl = new TagBuilder("span");
					iconEl.AddCssClass($"go-gray fa fa-{(Value ? "check" : "times")}");
					wrapper.InnerHtml.AppendHtml(iconEl);
					wrapper.InnerHtml.AppendHtml(Value ? TextTrue : TextFalse);
				}
				output.Content.AppendHtml(wrapper);

			}
			else if (Mode == FieldRenderMode.Simple)
			{
				if (Value != null)
				{
                    output.SuppressOutput();
                    var iconEl = new TagBuilder("span");
					iconEl.AddCssClass($"{(Value ? "fa fa-check" : "go-gray")}");
					iconEl.InnerHtml.Append($"{(Value ? "" : "-")}");
					output.Content.AppendHtml(iconEl);
				}
			}
			else if (Mode == FieldRenderMode.InlineEdit)
			{
				if (Access == FieldAccess.Full || Access == FieldAccess.FullAndCreate)
				{
					//View
					{
						var viewWrapper = new TagBuilder("div");
						viewWrapper.Attributes.Add("id", $"view-{FieldId}");
						viewWrapper.AddCssClass("input-group view-wrapper");

						var prependWrapper = new TagBuilder("span");
						prependWrapper.AddCssClass("input-group-prepend");

						var prependEl = new TagBuilder("span");
						prependEl.AddCssClass("input-group-text");

						var prependIcon = new TagBuilder("span");
						if (Value != null)
						{
							prependIcon.AddCssClass($"fa fa-fw fa-{(Value ? "check" : "times")}");
						}
						prependEl.InnerHtml.AppendHtml(prependIcon);
						prependWrapper.InnerHtml.AppendHtml(prependEl);
						viewWrapper.InnerHtml.AppendHtml(prependWrapper);

						var viewFormControlEl = new TagBuilder("div");
						viewFormControlEl.AddCssClass($"form-control erp-checkbox {(ValidationErrors.Count > 0 ? "is-invalid" : "")}");
						viewFormControlEl.Attributes.Add("title", "double click to edit");
						if (Value != null)
						{
							viewFormControlEl.InnerHtml.AppendHtml(Value ? TextTrue : TextFalse);
						}
						viewWrapper.InnerHtml.AppendHtml(viewFormControlEl);


						var appendEl = new TagBuilder("span");
						appendEl.AddCssClass("input-group-append");
						var appendButton = new TagBuilder("button");
						appendButton.Attributes.Add("type", "button");
						appendButton.Attributes.Add("title", "edit");
						appendButton.AddCssClass("btn btn-secondary action");
						var appendIcon = new TagBuilder("span");
						appendIcon.AddCssClass("fa fa-fw fa-pencil-alt");
						appendButton.InnerHtml.AppendHtml(appendIcon);
						appendEl.InnerHtml.AppendHtml(appendButton);
						viewWrapper.InnerHtml.AppendHtml(appendEl);

						output.Content.AppendHtml(viewWrapper);
					}

					//Edit
					{
						var editWrapper = new TagBuilder("div");
						editWrapper.Attributes.Add("id", $"edit-{FieldId}");
						editWrapper.AddCssClass("edit-wrapper");
						editWrapper.Attributes.Add("style", "display:none;");
						var inputGroupEl = new TagBuilder("div");
						inputGroupEl.AddCssClass("input-group erp-checkbox");
						var editFormControlEl = new TagBuilder("div");
						editFormControlEl.AddCssClass("form-control erp-checkbox");
						var editFormCheckEl = new TagBuilder("div");
						editFormCheckEl.AddCssClass("form-check");
						var editFormLabelEl = new TagBuilder("label");
						editFormLabelEl.AddCssClass("form-check-label");
						var editInputEl = new TagBuilder("input");
						editInputEl.Attributes.Add("type", "checkbox");
						editInputEl.AddCssClass("form-check-input");
						if (Value!= null && Value)
						{
							editInputEl.Attributes.Add("checked", "checked");
						}
						editInputEl.Attributes.Add("value", "true");
						editFormLabelEl.InnerHtml.AppendHtml(editInputEl);
						if (Value != null)
						{
							editFormLabelEl.InnerHtml.AppendHtml(Value ? TextTrue : TextFalse);
						}
						editFormCheckEl.InnerHtml.AppendHtml(editFormLabelEl);
						editFormControlEl.InnerHtml.AppendHtml(editFormCheckEl);
						inputGroupEl.InnerHtml.AppendHtml(editFormControlEl);

						var editInputAppend = new TagBuilder("span");
						editInputAppend.AddCssClass("input-group-append");
						var editSaveBtn = new TagBuilder("button");
						editSaveBtn.Attributes.Add("type", "submit");
						editSaveBtn.AddCssClass("btn btn-secondary save");
						editSaveBtn.Attributes.Add("title", "Save");
						var editSaveIcon = new TagBuilder("span");
						editSaveIcon.AddCssClass("fa fa-fw fa-check go-green");
						editSaveBtn.InnerHtml.AppendHtml(editSaveIcon);
						editInputAppend.InnerHtml.AppendHtml(editSaveBtn);

						var editCancelBtn = new TagBuilder("button");
						editCancelBtn.Attributes.Add("type", "submit");
						editCancelBtn.AddCssClass("btn btn-secondary cancel");
						editCancelBtn.Attributes.Add("title", "Cancel");
						var editCancelIcon = new TagBuilder("span");
						editCancelIcon.AddCssClass("fa fa-fw fa-times go-gray");
						editCancelBtn.InnerHtml.AppendHtml(editCancelIcon);
						editInputAppend.InnerHtml.AppendHtml(editCancelBtn);

						inputGroupEl.InnerHtml.AppendHtml(editInputAppend);
						editWrapper.InnerHtml.AppendHtml(inputGroupEl);
						output.Content.AppendHtml(editWrapper);



						var jsCompressor = new JavaScriptCompressor();
						#region << Init Scripts >>
						var tagHelperInitialized = false;
						if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldCheckbox) + "-inline-edit"))
						{
							var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldCheckbox) + "-inline-edit"];
							tagHelperInitialized = tagHelperContext.Initialized;
						}
						if (!tagHelperInitialized)
						{
							var scriptContent = FileService.GetEmbeddedTextResource("inline-edit.js", "WebVella.Erp.Web.TagHelpers.WvFieldCheckbox");
							var scriptEl = new TagBuilder("script");
							scriptEl.Attributes.Add("type", "text/javascript");
							scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
							output.PostContent.AppendHtml(scriptEl);

							ViewContext.HttpContext.Items[typeof(WvFieldCheckbox) + "-inline-edit"] = new WvTagHelperContext()
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
							CheckboxInlineEditInit(""{{FieldId}}"",""{{Name}}"",""{{EntityName}}"",""{{RecordId}}"",{{ConfigJson}});
						});";
						scriptTemplate = scriptTemplate.Replace("{{FieldId}}", (FieldId ?? null).ToString());
						scriptTemplate = scriptTemplate.Replace("{{Name}}", Name);
						scriptTemplate = scriptTemplate.Replace("{{EntityName}}", EntityName);
						scriptTemplate = scriptTemplate.Replace("{{RecordId}}", (RecordId ?? null).ToString());

						var fieldConfig = new WvFieldCheckboxConfig()
						{
							ApiUrl = ApiUrl,
							CanAddValues = Access == FieldAccess.FullAndCreate ? true : false,
							TrueLabel = TextTrue,
							FalseLabel = TextFalse
						};

						scriptTemplate = scriptTemplate.Replace("{{ConfigJson}}", JsonConvert.SerializeObject(fieldConfig));

						initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

						output.PostContent.AppendHtml(initScript);
						#endregion
					}
				}
				else if (Access == FieldAccess.ReadOnly)
				{
					var divEl = new TagBuilder("div");
					divEl.AddCssClass("input-group");

					var prependWrapper = new TagBuilder("span");
					prependWrapper.AddCssClass("input-group-prepend");

					var prependEl = new TagBuilder("span");
					prependEl.AddCssClass("input-group-text");

					var prependIcon = new TagBuilder("span");
					prependIcon.AddCssClass($"fa fa-fw fa-{(Value ? "check" : "times")}");
					prependEl.InnerHtml.AppendHtml(prependIcon);
					prependWrapper.InnerHtml.AppendHtml(prependEl);
					divEl.InnerHtml.AppendHtml(prependWrapper);



					var inputEl = new TagBuilder("input");
					inputEl.AddCssClass("form-control erp-checkbox");
					inputEl.Attributes.Add("type", "text");
					inputEl.Attributes.Add("value", Value ? TextTrue : TextFalse);
					inputEl.Attributes.Add("readonly", null);

					var appendActionSpan = new TagBuilder("span");
					appendActionSpan.AddCssClass("input-group-append");
					appendActionSpan.AddCssClass("action");

					var appendTextSpan = new TagBuilder("span");
					appendTextSpan.AddCssClass("input-group-text");

					var appendIconSpan = new TagBuilder("span");
					appendIconSpan.AddCssClass("fa fa-fw fa-lock");

					appendTextSpan.InnerHtml.AppendHtml(appendIconSpan);

					appendActionSpan.InnerHtml.AppendHtml(appendTextSpan);

					divEl.InnerHtml.AppendHtml(inputEl);
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

			return Task.CompletedTask;
		}

	}
}
