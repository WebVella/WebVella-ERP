using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.TagHelpers.Models;
using WebVella.TagHelpers.TagHelpers;
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.TagHelpers
{
	[HtmlTargetElement("wv-field-user-file-multiple")]
	public class WvFieldUserFileMultiple : WvFieldBase
	{
		[HtmlAttributeName("file-entity-name")]
		public string FileEntityName { get; set; } = "user_file";

		[HtmlAttributeName("path-field-name")]
		public string PathFieldName { get; set; } = "path";

		[HtmlAttributeName("size-field-name")]
		public string SizeFieldName { get; set; } = "size";

		[HtmlAttributeName("accept")]
		public string Accept { get; set; } = "";

		private List<EntityRecord> FileRecords { get; set; } = new List<EntityRecord>();

		private List<Guid> RelatedFileIds { get; set; } = new List<Guid>();

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

            if (Value is string && Value == "")
                Value = null;

			if (Value != null && !(Value is List<EntityRecord>)) {
				output.Content.AppendHtml("<div class='go-red'>'value' property should be 'List&lt;EntityRecord>'");
				return Task.CompletedTask;
			}

			if (Value != null && Value is List<EntityRecord>)
			{
				var valueRecords = (List<EntityRecord>)Value;
				foreach (var file in valueRecords)
				{
					if (!file.Properties.ContainsKey(PathFieldName))
					{
						output.Content.AppendHtml($"<div class='go-red'>{PathFieldName} property is missing in value (List<EntityRecord>)");
						return Task.CompletedTask;
					}

					var fileId = (Guid)file["id"];
					RelatedFileIds.Add(fileId);
					var fileRecord = new EntityRecord();
					fileRecord["id"] = fileId;
					var fileUrl = (string)file[PathFieldName];
					if (fileUrl.StartsWith("/fs"))
					{
						fileUrl = fileUrl.Substring(3);
					}
					fileRecord["path"] = fileUrl;
					fileRecord["icon_class"] = new RenderService().GetPathTypeIcon(fileUrl);
					fileRecord["name"] = Path.GetFileName(fileUrl);
					fileRecord["size"] = (decimal)0;
					if (!string.IsNullOrWhiteSpace(SizeFieldName) && file.Properties.ContainsKey(SizeFieldName))
						fileRecord["size"] = (decimal)file[SizeFieldName];

					FileRecords.Add(fileRecord);
				}

			}

			#endregion

			#region << Render >>
			if (Mode == WvFieldRenderMode.Form)
			{
				if (Access == WvFieldAccess.Full || Access == WvFieldAccess.FullAndCreate)
				{

					#region << Hidden input for posting >>
					output.Content.AppendHtml($"<input type='hidden' id='input-{FieldId}' name='{Name}' value='{String.Join(',', RelatedFileIds)}' data-entity-name='{FileEntityName}' data-path-field-name='{PathFieldName}'/>");
					#endregion

					#region << fake upload >>
					var inputGroupEl = new TagBuilder("div");
					inputGroupEl.AddCssClass("input-group erp-file-multiple-input");

					var fakeInputEl = new TagBuilder("div");
					var inputElCssClassList = new List<string>();
					inputElCssClassList.Add("form-control erp-file with-progress");
					fakeInputEl.Attributes.Add("id", $"fake-{FieldId}");

					if (ValidationErrors.Count > 0)
					{
						inputElCssClassList.Add("is-invalid");
					}
					fakeInputEl.Attributes.Add("class", String.Join(' ', inputElCssClassList));

					var fakeInputProgress = new TagBuilder("div");
					fakeInputProgress.AddCssClass("form-control-progress");
					fakeInputEl.InnerHtml.AppendHtml(fakeInputProgress);

					inputGroupEl.InnerHtml.AppendHtml(fakeInputEl);

					var appendEl = new TagBuilder("span");
					appendEl.AddCssClass("input-group-append action erp-file");
					var selectFileLink = new TagBuilder("button");
					selectFileLink.Attributes.Add("type", $"button");
					selectFileLink.AddCssClass("btn btn-white");
					selectFileLink.Attributes.Add("onclick", $"window.document.getElementById('file-{FieldId}').click();");
					selectFileLink.InnerHtml.AppendHtml("browse");
					appendEl.InnerHtml.AppendHtml(selectFileLink);
					inputGroupEl.InnerHtml.AppendHtml(appendEl);
					output.Content.AppendHtml(inputGroupEl);

					var realHiddenFileInput = new TagBuilder("input");
					realHiddenFileInput.Attributes.Add("id", $"file-{FieldId}");
					realHiddenFileInput.Attributes.Add("type", $"file");
					realHiddenFileInput.Attributes.Add("multiple", $"true");
					realHiddenFileInput.AddCssClass("d-none");
					realHiddenFileInput.Attributes.Add("value", $"");
					if (!String.IsNullOrWhiteSpace(Accept))
					{
						realHiddenFileInput.Attributes.Add("accept", $"{Accept}");
					}
					output.Content.AppendHtml(realHiddenFileInput);

					#endregion

					#region << Files list element >>

					var filesListEl = new TagBuilder("div");
					filesListEl.AddCssClass("form-control erp-file-multiple-list form");
					filesListEl.Attributes.Add("id", $"fake-list-{FieldId}");

					if (FileRecords.Count == 0)
						filesListEl.AddCssClass("d-none");


					//Generate the files list
					foreach (var file in FileRecords)
					{
						var fileRow = new TagBuilder("div");
						fileRow.AddCssClass("filerow");
						fileRow.Attributes.Add("data-file-id", ((Guid)file["id"]).ToString());
						//Append icon
						fileRow.InnerHtml.AppendHtml($"<div class='icon'><i class='fa {(string)file["icon_class"]}'></i></div>");

						//Append meta
						var rowMeta = new TagBuilder("div");
						rowMeta.AddCssClass("meta");

						//Append file 
						rowMeta.InnerHtml.AppendHtml($"<a class='link' href='/fs{(string)file["path"]}' target='_blank' title='/fs{(string)file["path"]}'>{(string)file["name"]}<em></em></a>");
						//Append size
						var sizeString = "";
						var sizeKBInt = (int)((decimal)file["size"]); //size is in KB
						if (sizeKBInt < 1024)
						{
							sizeString = sizeKBInt + " KB";
						}
						else if (sizeKBInt >= 1024 && sizeKBInt < Math.Pow(1024, 2))
						{
							sizeString = Math.Round((decimal)(sizeKBInt / 1024), 1) + " MB";
						}
						else {
							sizeString = Math.Round((decimal)(sizeKBInt / Math.Pow(1024, 2)), 1) + " GB";
						}

						rowMeta.InnerHtml.AppendHtml($"<div class='size'>{sizeString}</div>");

						fileRow.InnerHtml.AppendHtml(rowMeta);

						//Action
						var rowAction = new TagBuilder("div");
						rowAction.AddCssClass("action remove");
						rowAction.InnerHtml.AppendHtml($"<a class='link' href='#'><i class='fa fa-times-circle'></i></a>");
						//rowAction.InnerHtml.AppendHtml($"<span class='progress d-none'>0%</span>");
						//rowAction.InnerHtml.AppendHtml($"<span class='error go-red d-none'><i class='fas fa-exclamation-circle'></i></span>");

						fileRow.InnerHtml.AppendHtml(rowAction);
						filesListEl.InnerHtml.AppendHtml(fileRow);
					}

					output.Content.AppendHtml(filesListEl);

					#endregion


					var jsCompressor = new JavaScriptCompressor();

					#region << Init Scripts >>
					var tagHelperInitialized = false;
					var fileName = "form";
					if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldUserFileMultiple) + fileName))
					{
						var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldUserFileMultiple) + fileName];
						tagHelperInitialized = tagHelperContext.Initialized;
					}
					if (!tagHelperInitialized)
					{
						var scriptContent = FileService.GetEmbeddedTextResource("form.js", "WebVella.Erp.Web.TagHelpers.WvFieldUserFileMultiple");
						var scriptEl = new TagBuilder("script");
						scriptEl.Attributes.Add("type", "text/javascript");
                        //scriptEl.InnerHtml.AppendHtml(scriptContent);
                        scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
                        output.PostContent.AppendHtml(scriptEl);

						ViewContext.HttpContext.Items[typeof(WvFieldUserFileMultiple) + fileName] = new WvTagHelperContext()
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
							FieldUserMultiFileFormInit(""{{FieldId}}"",{{ConfigJson}});
						});";
					scriptTemplate = scriptTemplate.Replace("{{FieldId}}", (FieldId ?? null).ToString());

					var fieldConfig = new WvFieldFileConfig()
					{
						ApiUrl = ApiUrl,
						CanAddValues = Access == WvFieldAccess.FullAndCreate ? true : false,
						Accept = Accept
					};

					scriptTemplate = scriptTemplate.Replace("{{ConfigJson}}", JsonConvert.SerializeObject(fieldConfig));

					initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

					output.PostContent.AppendHtml(initScript);
					#endregion

				}
				else if (Access == WvFieldAccess.ReadOnly)
				{
					//if (!String.IsNullOrWhiteSpace(Value))
					//{
					//	var inputGroupEl = new TagBuilder("div");
					//	inputGroupEl.AddCssClass("input-group");
					//	var prependEl = new TagBuilder("span");
					//	prependEl.AddCssClass($"input-group-prepend icon-addon {(String.IsNullOrWhiteSpace(Value) ? "d-none" : "")} {(ValidationErrors.Count > 0 ? "is-invalid" : "")}");
					//	prependEl.Attributes.Add("title", $"/fs{Value}");
					//	var prependText = new TagBuilder("span");
					//	prependText.AddCssClass("input-group-text");
					//	var prependIcon = new TagBuilder("span");
					//	prependIcon.AddCssClass($"fa fa-fw type-icon {PathTypeIcon}");
					//	prependText.InnerHtml.AppendHtml(prependIcon);
					//	prependEl.InnerHtml.AppendHtml(prependText);
					//	inputGroupEl.InnerHtml.AppendHtml(prependEl);

					//	var inputEl = new TagBuilder("div");
					//	inputEl.AddCssClass("form-control erp-file disabled");
					//	var inputElLink = new TagBuilder("a");
					//	inputElLink.Attributes.Add("href", $"/fs{Value}");
					//	inputElLink.Attributes.Add("target", "_blank");
					//	inputElLink.Attributes.Add("title", $"/fs{Value}");
					//	inputElLink.InnerHtml.Append(FileName);
					//	inputEl.InnerHtml.AppendHtml(inputElLink);
					//	inputGroupEl.InnerHtml.AppendHtml(inputEl);
					//	output.Content.AppendHtml(inputGroupEl);

					//	//Hidden input with the value
					//	var hiddenInput = new TagBuilder("input");
					//	hiddenInput.Attributes.Add("type", "hidden");
					//	hiddenInput.Attributes.Add("id", $"input-{FieldId}");
					//	hiddenInput.Attributes.Add("name", Name);
					//	hiddenInput.Attributes.Add("value", (Value ?? "").ToString());
					//	output.Content.AppendHtml(hiddenInput);
					//}
					//else {
					//	var inputEl = new TagBuilder("input");
					//	inputEl.Attributes.Add("readonly", null);
					//	inputEl.AddCssClass("form-control erp-file");
					//	inputEl.Attributes.Add("value","");
					//	inputEl.Attributes.Add("name", Name);
					//	output.Content.AppendHtml(inputEl);
					//}
				}
			}
			else if (Mode == WvFieldRenderMode.Display)
			{
                output.Content.AppendHtml("Not implemented yet");
                //if (!String.IsNullOrWhiteSpace(Value))
                //{
                //	var divEl = new TagBuilder("div");
                //	divEl.Attributes.Add("id", $"input-{FieldId}");
                //	divEl.AddCssClass("form-control-plaintext erp-file");
                //	var iconEl = new TagBuilder("span");
                //	iconEl.AddCssClass($"fa fa-fw {PathTypeIcon}");
                //	divEl.InnerHtml.AppendHtml(iconEl);
                //	var linkEl = new TagBuilder("a");
                //	linkEl.Attributes.Add("href", $"/fs{Value}");
                //	linkEl.Attributes.Add("target", $"_blank");
                //	linkEl.InnerHtml.Append(FileName);
                //	divEl.InnerHtml.AppendHtml(linkEl);
                //	output.Content.AppendHtml(divEl);
                //}
                //else
                //{
                //	output.Content.AppendHtml(EmptyValEl);
                //}
            }
			else if (Mode == WvFieldRenderMode.Simple)
			{
                output.Content.AppendHtml("Not implemented yet");
                //output.SuppressOutput();
                //var linkEl = new TagBuilder("a");
                //linkEl.Attributes.Add("href", $"/fs{Value}");
                //linkEl.Attributes.Add("target", $"_blank");
                //linkEl.InnerHtml.Append(FileName);
                //output.Content.AppendHtml(linkEl);
                //return Task.CompletedTask;
            }
			else if (Mode == WvFieldRenderMode.InlineEdit)
			{
                output.Content.AppendHtml("Not implemented yet");
				//if (Access == FieldAccess.Full || Access == FieldAccess.FullAndCreate)
				//{
				//	#region << View Wrapper >>
				//	{
				//		var viewWrapperEl = new TagBuilder("div");
				//		viewWrapperEl.AddCssClass("input-group view-wrapper");
				//		viewWrapperEl.Attributes.Add("title", "double click to edit");
				//		viewWrapperEl.Attributes.Add("id", $"view-{FieldId}");

				//		var viewInputPrepend = new TagBuilder("span");
				//		viewInputPrepend.AddCssClass($"input-group-prepend icon-addon {(String.IsNullOrWhiteSpace(Value) ? "d-none" : "")}");
				//		viewInputPrepend.Attributes.Add("title", $"/fs{Value}");
				//		var viewInputPrependText = new TagBuilder("span");
				//		viewInputPrependText.AddCssClass("input-group-text");
				//		var prependIcon = new TagBuilder("span");
				//		prependIcon.AddCssClass($"fa fa-fw type-icon {PathTypeIcon}");
				//		viewInputPrependText.InnerHtml.AppendHtml(prependIcon);
				//		viewInputPrepend.InnerHtml.AppendHtml(viewInputPrependText);
				//		viewWrapperEl.InnerHtml.AppendHtml(viewInputPrepend);

				//		var viewFormControlEl = new TagBuilder("div");
				//		viewFormControlEl.AddCssClass("form-control erp-file");

				//		var viewFormControlLinkEl = new TagBuilder("a");
				//		viewFormControlLinkEl.Attributes.Add("href", $"/fs{Value}");
				//		viewFormControlLinkEl.Attributes.Add("target", "_blank");
				//		viewFormControlLinkEl.Attributes.Add("title", $"/fs{Value}");
				//		viewFormControlLinkEl.InnerHtml.Append(FileName);
				//		viewFormControlEl.InnerHtml.AppendHtml(viewFormControlLinkEl);

				//		viewWrapperEl.InnerHtml.AppendHtml(viewFormControlEl);

				//		var viewInputActionEl = new TagBuilder("span");
				//		viewInputActionEl.AddCssClass("input-group-append action");
				//		viewInputActionEl.Attributes.Add("title", "edit");

				//		var viewInputActionLinkEl = new TagBuilder("button");
				//		viewInputActionLinkEl.Attributes.Add("type", "button");
				//		viewInputActionLinkEl.AddCssClass("btn btn-white");

				//		var viewInputActionIconEl = new TagBuilder("span");
				//		viewInputActionIconEl.AddCssClass("fa fa-fw fa-pencil-alt");
				//		viewInputActionLinkEl.InnerHtml.AppendHtml(viewInputActionIconEl);
				//		viewInputActionEl.InnerHtml.AppendHtml(viewInputActionLinkEl);
				//		viewWrapperEl.InnerHtml.AppendHtml(viewInputActionEl);

				//		output.Content.AppendHtml(viewWrapperEl);
				//	}
				//	#endregion

				//	#region << Edit Wrapper>>
				//	{
				//		var editWrapperEl = new TagBuilder("div");
				//		editWrapperEl.Attributes.Add("id", $"edit-{FieldId}");
				//		editWrapperEl.Attributes.Add("style", $"display:none;");
				//		editWrapperEl.AddCssClass("edit-wrapper");

				//		var editInputGroupEl = new TagBuilder("div");
				//		editInputGroupEl.AddCssClass("input-group");

				//		var editWrapperPrependEl = new TagBuilder("span");
				//		editWrapperPrependEl.AddCssClass($"input-group-prepend icon-addon {(String.IsNullOrWhiteSpace(Value) ? "d-none" : "")} {(ValidationErrors.Count > 0 ? "is-invalid" : "")}");
				//		editWrapperPrependEl.Attributes.Add("title", $"/fs{Value}");
				//		var editWrapperPrependText = new TagBuilder("span");
				//		editWrapperPrependText.AddCssClass("input-group-text");
				//		var editWrapperPrependIcon = new TagBuilder("span");
				//		editWrapperPrependIcon.AddCssClass($"fa fa-fw type-icon {PathTypeIcon}");
				//		editWrapperPrependText.InnerHtml.AppendHtml(editWrapperPrependIcon);
				//		editWrapperPrependEl.InnerHtml.AppendHtml(editWrapperPrependText);
				//		editInputGroupEl.InnerHtml.AppendHtml(editWrapperPrependEl);


				//		var fakeInputEl = new TagBuilder("div");
				//		var inputElCssClassList = new List<string>();
				//		inputElCssClassList.Add("form-control erp-file with-progress ");

				//		fakeInputEl.Attributes.Add("id", $"fake-{FieldId}");

				//		if (ValidationErrors.Count > 0)
				//		{
				//			inputElCssClassList.Add("is-invalid");
				//		}

				//		fakeInputEl.Attributes.Add("class", String.Join(' ', inputElCssClassList));

				//		var fakeInputFileLinkEl = new TagBuilder("a");
				//		fakeInputFileLinkEl.Attributes.Add("href", $"/fs{Value}");
				//		fakeInputFileLinkEl.Attributes.Add("target", "_blank");
				//		fakeInputFileLinkEl.Attributes.Add("title", $"/fs{Value}");
				//		fakeInputFileLinkEl.InnerHtml.Append(FileName);
				//		fakeInputEl.InnerHtml.AppendHtml(fakeInputFileLinkEl);
				//		var fakeInputProgress = new TagBuilder("div");
				//		fakeInputProgress.AddCssClass("form-control-progress");
				//		fakeInputEl.InnerHtml.AppendHtml(fakeInputProgress);
				//		editInputGroupEl.InnerHtml.AppendHtml(fakeInputEl);


				//		var editInputGroupAppendEl = new TagBuilder("span");
				//		editInputGroupAppendEl.AddCssClass("input-group-append");

				//		if (!Required)
				//		{
				//			var appendDeleteLink = new TagBuilder("button");
				//			appendDeleteLink.Attributes.Add("type", $"button");
				//			appendDeleteLink.Attributes.Add("id", $"remove-{FieldId}");
				//			appendDeleteLink.AddCssClass($"btn btn-white remove {(String.IsNullOrWhiteSpace(Value) ? "d-none" : "")}");
				//			appendDeleteLink.Attributes.Add("title", "select as undefined");
				//			var appendDeleteLinkIcon = new TagBuilder("span");
				//			appendDeleteLinkIcon.AddCssClass("fa fa-fw fa-trash go-red");
				//			appendDeleteLink.InnerHtml.AppendHtml(appendDeleteLinkIcon);
				//			editInputGroupAppendEl.InnerHtml.AppendHtml(appendDeleteLink);
				//		}

				//		var selectFileLink = new TagBuilder("button");
				//		selectFileLink.Attributes.Add("type", $"button");
				//		selectFileLink.AddCssClass("btn btn-white");
				//		selectFileLink.Attributes.Add("onclick", $"window.document.getElementById('file-{FieldId}').click();");
				//		selectFileLink.InnerHtml.AppendHtml("select");
				//		editInputGroupAppendEl.InnerHtml.AppendHtml(selectFileLink);


				//		var editSaveBtnEl = new TagBuilder("button");
				//		editSaveBtnEl.Attributes.Add("type", "submit");
				//		editSaveBtnEl.AddCssClass("btn btn-white save");
				//		editSaveBtnEl.Attributes.Add("title", "save");

				//		var editSaveIconEl = new TagBuilder("span");
				//		editSaveIconEl.AddCssClass("fa fa-fw fa-check go-green");
				//		editSaveBtnEl.InnerHtml.AppendHtml(editSaveIconEl);
				//		editInputGroupAppendEl.InnerHtml.AppendHtml(editSaveBtnEl);

				//		var editCancelBtnEl = new TagBuilder("button");
				//		editCancelBtnEl.Attributes.Add("type", "submit");
				//		editCancelBtnEl.AddCssClass("btn btn-white cancel");
				//		editCancelBtnEl.Attributes.Add("title", "cancel");

				//		var editCancelIconEl = new TagBuilder("span");
				//		editCancelIconEl.AddCssClass("fa fa-fw fa-times go-gray");
				//		editCancelBtnEl.InnerHtml.AppendHtml(editCancelIconEl);
				//		editInputGroupAppendEl.InnerHtml.AppendHtml(editCancelBtnEl);

				//		editInputGroupEl.InnerHtml.AppendHtml(editInputGroupAppendEl);
				//		editWrapperEl.InnerHtml.AppendHtml(editInputGroupEl);

				//		output.Content.AppendHtml(editWrapperEl);

				//		var realHiddenFileInput = new TagBuilder("input");
				//		realHiddenFileInput.Attributes.Add("id", $"file-{FieldId}");
				//		realHiddenFileInput.Attributes.Add("type", $"file");
				//		realHiddenFileInput.AddCssClass("d-none");
				//		realHiddenFileInput.Attributes.Add("value", $"");
				//		if (!String.IsNullOrWhiteSpace(Accept))
				//		{
				//			realHiddenFileInput.Attributes.Add("accept", $"{Accept}");
				//		}
				//		output.Content.AppendHtml(realHiddenFileInput);

				//		var realSubmitInput = new TagBuilder("input");
				//		realSubmitInput.Attributes.Add("id", $"input-{FieldId}");
				//		realSubmitInput.Attributes.Add("type", $"hidden");
				//		realSubmitInput.Attributes.Add("value", $"{Value}");
				//		realSubmitInput.Attributes.Add("data-newfilepath", $"{Value}");
				//		realSubmitInput.Attributes.Add("data-filename", $"{FileName}");
				//		realSubmitInput.Attributes.Add("data-newfilename", $"{FileName}");
				//		output.Content.AppendHtml(realSubmitInput);

				//	}
				//	#endregion

				//	var jsCompressor = new JavaScriptCompressor();

				//	#region << Init Scripts >>
				//	var tagHelperInitialized = false;
				//	if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldFile) + "-inline-edit"))
				//	{
				//		var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldFile) + "-inline-edit"];
				//		tagHelperInitialized = tagHelperContext.Initialized;
				//	}
				//	if (!tagHelperInitialized)
				//	{
				//		var scriptContent = FileService.GetEmbeddedTextResource("inline-edit.js", "WebVella.Erp.Web.TagHelpers.WvFieldFile");
				//		var scriptEl = new TagBuilder("script");
				//		scriptEl.Attributes.Add("type", "text/javascript");
				//		scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
				//		output.PostContent.AppendHtml(scriptEl);

				//		ViewContext.HttpContext.Items[typeof(WvFieldFile) + "-inline-edit"] = new WvTagHelperContext()
				//		{
				//			Initialized = true
				//		};

				//	}
				//	#endregion

				//	#region << Add Inline Init Script for this instance >>
				//	var initScript = new TagBuilder("script");
				//	initScript.Attributes.Add("type", "text/javascript");
				//	var scriptTemplate = @"
				//		$(function(){
				//			FileInlineEditInit(""{{FieldId}}"",""{{Name}}"",""{{EntityName}}"",""{{RecordId}}"",{{ConfigJson}});
				//		});";
				//	scriptTemplate = scriptTemplate.Replace("{{FieldId}}", (FieldId ?? null).ToString());
				//	scriptTemplate = scriptTemplate.Replace("{{Name}}", Name);
				//	scriptTemplate = scriptTemplate.Replace("{{EntityName}}", EntityName);
				//	scriptTemplate = scriptTemplate.Replace("{{RecordId}}", (RecordId ?? null).ToString());

				//	var fieldConfig = new WvFieldFileConfig()
				//	{
				//		ApiUrl = ApiUrl,
				//		CanAddValues = Access == FieldAccess.FullAndCreate ? true : false,
				//		Accept = Accept
				//	};

				//	scriptTemplate = scriptTemplate.Replace("{{ConfigJson}}", JsonConvert.SerializeObject(fieldConfig));

				//	initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

				//	output.PostContent.AppendHtml(initScript);
				//	#endregion
				//}
				//else if (Access == FieldAccess.ReadOnly)
				//{

				//	var divEl = new TagBuilder("div");
				//	divEl.AddCssClass("input-group");

				//	var prependEl = new TagBuilder("span");
				//	prependEl.AddCssClass($"input-group-prepend icon-addon {(String.IsNullOrWhiteSpace(Value) ? "d-none" : "")} {(ValidationErrors.Count > 0 ? "is-invalid" : "")}");
				//	prependEl.Attributes.Add("title", $"/fs{Value}");
				//	var prependText = new TagBuilder("span");
				//	prependText.AddCssClass("input-group-text");
				//	var prependIcon = new TagBuilder("span");
				//	prependIcon.AddCssClass($"fa fa-fw type-icon {PathTypeIcon}");
				//	prependText.InnerHtml.AppendHtml(prependIcon);
				//	prependEl.InnerHtml.AppendHtml(prependText);
				//	divEl.InnerHtml.AppendHtml(prependEl);

				//	var inputEl = new TagBuilder("div");
				//	inputEl.AddCssClass("form-control erp-file disabled");
				//	var inputElLink = new TagBuilder("a");
				//	inputElLink.Attributes.Add("href", $"/fs{Value}");
				//	inputElLink.Attributes.Add("target", "_blank");
				//	inputElLink.Attributes.Add("title", $"/fs{Value}");
				//	inputElLink.InnerHtml.Append(FileName);
				//	inputEl.InnerHtml.AppendHtml(inputElLink);
				//	divEl.InnerHtml.AppendHtml(inputEl);

				//	var appendActionSpan = new TagBuilder("span");
				//	appendActionSpan.AddCssClass("input-group-append");
				//	appendActionSpan.AddCssClass("action");

				//	var appendTextSpan = new TagBuilder("span");
				//	appendTextSpan.AddCssClass("input-group-text");

				//	var appendIconSpan = new TagBuilder("span");
				//	appendIconSpan.AddCssClass("fa fa-fw fa-lock");
				//	appendTextSpan.InnerHtml.AppendHtml(appendIconSpan);
				//	appendActionSpan.InnerHtml.AppendHtml(appendTextSpan);

				//	divEl.InnerHtml.AppendHtml(appendActionSpan);
				//	output.Content.AppendHtml(divEl);
				//}
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
