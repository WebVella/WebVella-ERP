using HtmlAgilityPack;
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
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.TagHelpers
{
	[HtmlTargetElement("wv-field-datetime")]
	[RestrictChildren("wv-field-prepend", "wv-field-append")]
	public class WvFieldDateTime : WvFieldBase
	{
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
				var inputReadOnlyValue = "";
				var inputEditValue = "";
				if (Value != null)
				{
					inputReadOnlyValue = (Value as DateTime? ?? DateTime.Now).ConvertToAppDate().ClearKind().ToString("dd MMM yyyy HH:mm", Culture);
					inputEditValue = (Value as DateTime? ?? DateTime.Now).ConvertToAppDate().ClearKind().ToString("yyyy-MM-ddTHH:mm:ss");//.ToString("yyyy-MM-ddTHH:mm:ssZ");
				}


				var inputGroupEl = new TagBuilder("div");
				inputGroupEl.AddCssClass("input-group");
				//Prepend
				if (PrependHtml.Count > 0)
				{
					var prependEl = new TagBuilder("span");
					prependEl.AddCssClass($"input-group-prepend {(ValidationErrors.Count > 0 ? "is-invalid" : "")}");
					foreach (var htmlString in PrependHtml)
					{
						prependEl.InnerHtml.AppendHtml(htmlString);
					}
					inputGroupEl.InnerHtml.AppendHtml(prependEl);
				}
				//Control
				var inputEl = new TagBuilder("input");
				var inputElCssClassList = new List<string>();
				inputElCssClassList.Add("form-control erp-datetime");

				inputEl.Attributes.Add("type", "text");
				inputEl.Attributes.Add("id", $"input-{FieldId}");
				inputEl.Attributes.Add("name", Name);

				if (Access == FieldAccess.Full || Access == FieldAccess.FullAndCreate)
				{
					if (Required)
					{
						inputEl.Attributes.Add("required", null);
					}
					if (!String.IsNullOrWhiteSpace(Placeholder))
					{
						inputEl.Attributes.Add("placeholder", Placeholder);
					}

					inputEl.Attributes.Add("value", inputEditValue);
				}
				else if (Access == FieldAccess.ReadOnly)
				{
					inputEl.Attributes.Add("value", inputReadOnlyValue);
					inputEl.Attributes.Add("readonly", null);
				}

				if (ValidationErrors.Count > 0)
				{
					inputElCssClassList.Add("is-invalid");
				}

				inputEl.Attributes.Add("class", String.Join(' ', inputElCssClassList));

				inputGroupEl.InnerHtml.AppendHtml(inputEl);
				//Append
				if (AppendHtml.Count > 0)
				{
					var appendEl = new TagBuilder("span");
					appendEl.AddCssClass($"input-group-append {(ValidationErrors.Count > 0 ? "is-invalid" : "")}");

					foreach (var htmlString in AppendHtml)
					{
						appendEl.InnerHtml.AppendHtml(htmlString);
					}
					inputGroupEl.InnerHtml.AppendHtml(appendEl);
				}

				output.Content.AppendHtml(inputGroupEl);

				if (Access == FieldAccess.Full || Access == FieldAccess.FullAndCreate)
				{
					var jsCompressor = new JavaScriptCompressor();

					#region << Init Scripts >>
					var tagHelperInitialized = false;
					if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldDateTime) + "-form"))
					{
						var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldDateTime) + "-form"];
						tagHelperInitialized = tagHelperContext.Initialized;
					}
					if (!tagHelperInitialized)
					{
						var scriptContent = FileService.GetEmbeddedTextResource("form.js", "WebVella.Erp.Web.TagHelpers.WvFieldDateTime");
						var scriptEl = new TagBuilder("script");
						scriptEl.Attributes.Add("type", "text/javascript");
						scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
						output.PostContent.AppendHtml(scriptEl);

						ViewContext.HttpContext.Items[typeof(WvFieldDateTime) + "-form"] = new WvTagHelperContext()
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
							InitFlatPickrDateTime(""{{FieldId}}"");
						});";
					scriptTemplate = scriptTemplate.Replace("{{FieldId}}", (FieldId ?? null).ToString());

					initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

					output.PostContent.AppendHtml(initScript);
					#endregion
				}
			}
			else if (Mode == FieldRenderMode.Display)
			{
				if (Value != null)
				{
					var inputReadOnlyValue = (Value as DateTime? ?? DateTime.Now).ConvertToAppDate().ClearKind().ToString("dd MMM yyyy HH:mm", Culture);

					var divEl = new TagBuilder("div");
					divEl.Attributes.Add("id", $"input-{FieldId}");
					divEl.AddCssClass("form-control-plaintext erp-datetime");
					divEl.InnerHtml.Append(inputReadOnlyValue);
					output.Content.AppendHtml(divEl);
				}
				else
				{
					output.Content.AppendHtml(EmptyValEl);
				}

			}
			else if (Mode == FieldRenderMode.Simple)
			{
				if (Value != null)
				{
					output.SuppressOutput();
					var inputReadOnlyValue = (Value as DateTime? ?? DateTime.Now).ConvertToAppDate().ClearKind().ToString("dd MMM yyyy HH:mm", Culture);
					output.Content.AppendHtml(inputReadOnlyValue);
				}
				else
				{
                    output.SuppressOutput();
                    output.Content.AppendHtml("");
				}
				return;
			}
			else if (Mode == FieldRenderMode.InlineEdit)
			{
				if (Access == FieldAccess.Full || Access == FieldAccess.FullAndCreate)
				{

					var editInpuValue = "";
					var readOnlyInputValue = "";
					if (Value != null)
					{
						readOnlyInputValue = (Value as DateTime? ?? DateTime.Now).ConvertToAppDate().ClearKind().ToString("dd MMM yyyy HH:mm", Culture);
						editInpuValue = (Value as DateTime? ?? DateTime.Now).ConvertToAppDate().ClearKind().ToString("yyyy-MM-ddTHH:mm:ss"); //.ToString("yyyy-MM-ddTHH:mm:ssZ"); 
					}

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
							viewInputPrepend.AddCssClass("input-group-prepend");
							foreach (var htmlString in PrependHtml)
							{
								viewInputPrepend.InnerHtml.AppendHtml(htmlString);
							}
							viewWrapperEl.InnerHtml.AppendHtml(viewInputPrepend);
						}
						//Control
						var viewFormControlEl = new TagBuilder("div");
						viewFormControlEl.AddCssClass("form-control erp-datetime");
						viewFormControlEl.InnerHtml.Append(readOnlyInputValue);
						viewWrapperEl.InnerHtml.AppendHtml(viewFormControlEl);
						//Append
						var viewInputActionEl = new TagBuilder("span");
						viewInputActionEl.AddCssClass("input-group-append");
						foreach (var htmlString in AppendHtml)
						{
							viewInputActionEl.InnerHtml.AppendHtml(htmlString);
						}
						viewInputActionEl.InnerHtml.AppendHtml("<button type=\"button\" class='btn btn-white action' title='edit'><i class='fa fa-fw fa-pencil-alt'></i></button>");
						viewWrapperEl.InnerHtml.AppendHtml(viewInputActionEl);
						output.Content.AppendHtml(viewWrapperEl);
					}
					#endregion

					#region << Edit Wrapper>>
					{
						var editWrapperEl = new TagBuilder("div");
						editWrapperEl.Attributes.Add("id", $"edit-{FieldId}");
						editWrapperEl.Attributes.Add("style", $"display:none;");
						editWrapperEl.Attributes.Add("data-default-date", editInpuValue); //There is a problem with double initializing flatpickr which this solves
						editWrapperEl.AddCssClass("edit-wrapper");

						var editInputGroupEl = new TagBuilder("div");
						editInputGroupEl.AddCssClass("input-group");

						//Prepend
						if (PrependHtml.Count > 0)
						{
							var editInputPrepend = new TagBuilder("span");
							editInputPrepend.AddCssClass("input-group-prepend");
							foreach (var htmlString in PrependHtml)
							{
								editInputPrepend.InnerHtml.AppendHtml(htmlString);
							}
							editInputGroupEl.InnerHtml.AppendHtml(editInputPrepend);
						}
						//Control
						var editInputEl = new TagBuilder("input");
						editInputEl.AddCssClass("form-control erp-datetime");
						editInputEl.Attributes.Add("type", "text");
						if (Required)
						{
							editInputEl.Attributes.Add("required", null);
						}
						if (!String.IsNullOrWhiteSpace(Placeholder))
						{
							editInputEl.Attributes.Add("placeholder", Placeholder);
						}

						editInputEl.Attributes.Add("value", editInpuValue);
						editInputGroupEl.InnerHtml.AppendHtml(editInputEl);

						//Append
						var editInputGroupAppendEl = new TagBuilder("span");
						editInputGroupAppendEl.AddCssClass("input-group-append");

						foreach (var htmlString in AppendHtml)
						{
							editInputGroupAppendEl.InnerHtml.AppendHtml(htmlString);
						}
						editInputGroupAppendEl.InnerHtml.AppendHtml("<button type=\"button\" class='btn btn-white save' title='save'><i class='fa fa-fw fa-check go-green'></i></button>");
						editInputGroupAppendEl.InnerHtml.AppendHtml("<button type=\"button\" class='btn btn-white cancel' title='cancel'><i class='fa fa-fw fa-times go-gray'></i></button>");
						editInputGroupEl.InnerHtml.AppendHtml(editInputGroupAppendEl);

						editWrapperEl.InnerHtml.AppendHtml(editInputGroupEl);

						output.Content.AppendHtml(editWrapperEl);
					}
					#endregion

					var jsCompressor = new JavaScriptCompressor();

					#region << Init Scripts >>
					var tagHelperInitialized = false;
					if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldDateTime) + "-inline-edit"))
					{
						var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldDateTime) + "-inline-edit"];
						tagHelperInitialized = tagHelperContext.Initialized;
					}
					if (!tagHelperInitialized)
					{
						var scriptContent = FileService.GetEmbeddedTextResource("inline-edit.js", "WebVella.Erp.Web.TagHelpers.WvFieldDateTime");
						var scriptEl = new TagBuilder("script");
						scriptEl.Attributes.Add("type", "text/javascript");
						scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
						output.PostContent.AppendHtml(scriptEl);

						ViewContext.HttpContext.Items[typeof(WvFieldDateTime) + "-inline-edit"] = new WvTagHelperContext()
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
							DateTimeInlineEditInit(""{{FieldId}}"",""{{Name}}"",""{{EntityName}}"",""{{RecordId}}"",{{ConfigJson}});
						});";
					scriptTemplate = scriptTemplate.Replace("{{FieldId}}", (FieldId ?? null).ToString());
					scriptTemplate = scriptTemplate.Replace("{{Name}}", Name);
					scriptTemplate = scriptTemplate.Replace("{{EntityName}}", EntityName);
					scriptTemplate = scriptTemplate.Replace("{{RecordId}}", (RecordId ?? null).ToString());

					var fieldConfig = new WvFieldDateTimeConfig()
					{
						ApiUrl = ApiUrl,
						CanAddValues = Access == FieldAccess.FullAndCreate ? true : false
					};

					scriptTemplate = scriptTemplate.Replace("{{ConfigJson}}", JsonConvert.SerializeObject(fieldConfig));

					initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

					output.PostContent.AppendHtml(initScript);
					#endregion
				}
				else if (Access == FieldAccess.ReadOnly)
				{
					var readOnlyInputValue = "";
					if (Value != null)
					{
						readOnlyInputValue = (Value as DateTime? ?? DateTime.Now).ConvertToAppDate().ClearKind().ToString("dd MMM yyyy HH:mm", Culture);
					}

					var divEl = new TagBuilder("div");
					divEl.AddCssClass("input-group");
					//Prepend
					if (PrependHtml.Count > 0)
					{
						var viewInputPrepend = new TagBuilder("span");
						viewInputPrepend.AddCssClass("input-group-prepend");
						foreach (var htmlString in PrependHtml)
						{
							viewInputPrepend.InnerHtml.AppendHtml(htmlString);
						}
						divEl.InnerHtml.AppendHtml(viewInputPrepend);
					}
					//Control
					var inputEl = new TagBuilder("input");
					inputEl.AddCssClass("form-control erp-datetime");
					inputEl.Attributes.Add("type", "text");
					inputEl.Attributes.Add("value", readOnlyInputValue);
					inputEl.Attributes.Add("readonly", null);
					divEl.InnerHtml.AppendHtml(inputEl);
					//Append
					var appendActionSpan = new TagBuilder("span");
					appendActionSpan.AddCssClass("input-group-append");
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
