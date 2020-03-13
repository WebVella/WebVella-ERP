using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;
using WebVella.TagHelpers.TagHelpers;
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.TagHelpers
{

	[HtmlTargetElement("wv-field-datasource")]
	public class WvFieldDatasource : WvFieldBase
	{
		protected DataSourceVariable DataSourceVariable { get; set; } = null; // none, datasource, embedded_code

		[HtmlAttributeName("page-id")]
		public Guid? PageId { get; set; } = null;

		private List<string> PropertyNameLibrary { get; set; } = new List<string>();

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

			#region << DataSource Variable >>
			if (((Value ?? "").ToString()).StartsWith("{"))
			{
				try
				{
					DataSourceVariable = JsonConvert.DeserializeObject<DataSourceVariable>((Value ?? "").ToString(),new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error});
				}
				catch
				{
				}
			}
			#endregion

			#region << PropertyNameLibrary>>
			if (PageId != null)
			{
				var dictionaryKey = PageId + "property-name-library";
				if (ViewContext.HttpContext.Items.ContainsKey(dictionaryKey))
				{
					PropertyNameLibrary = (List<string>)ViewContext.HttpContext.Items[dictionaryKey];
				}
				else
				{
					PropertyNameLibrary = PageUtils.PropertyNamesList(PageId ?? Guid.Empty);
					ViewContext.HttpContext.Items[dictionaryKey] = PropertyNameLibrary;
				}
			}
			#endregion

			#endregion

			#region << InputGroup textbox >>
			{
				var inputGroupEl = new TagBuilder("div");
				inputGroupEl.AddCssClass("input-group");

				if (DataSourceVariable != null)
				{
					inputGroupEl.AddCssClass("d-none");
				}

				#region << Form Control >>
				var inputEl = new TagBuilder("input");
				var inputElCssClassList = new List<string>();
				inputElCssClassList.Add("form-control erp-datasource");
				inputEl.Attributes.Add("type", "text");
				inputEl.Attributes.Add("value", (Value ?? "").ToString());

				inputEl.Attributes.Add("id", $"input-{FieldId}");
				inputEl.Attributes.Add("name", Name);
				if (Required)
				{
					inputEl.Attributes.Add("required", null);
				}
				if (!String.IsNullOrWhiteSpace(Placeholder))
				{
					inputEl.Attributes.Add("placeholder", Placeholder);
				}


				if (ValidationErrors.Count > 0)
				{
					inputElCssClassList.Add("is-invalid");
				}

				inputEl.Attributes.Add("class", String.Join(' ', inputElCssClassList));

				inputGroupEl.InnerHtml.AppendHtml(inputEl);

				#endregion

				#region << Append >>
				var igAppendEl = new TagBuilder("div");
				igAppendEl.AddCssClass("input-group-append");

				var igLinkBtnEl = new TagBuilder("button");
				igLinkBtnEl.Attributes.Add("type", "button");
				igLinkBtnEl.Attributes.Add("id", $"input-{FieldId}-create-ds-link");
				igLinkBtnEl.Attributes.Add("title", "Link option to dynamic value");
				igLinkBtnEl.AddCssClass("btn btn-white");
				var igLinkBtnIconEl = new TagBuilder("i");
				igLinkBtnIconEl.AddCssClass("fa fa-cloud-download-alt fa-fw");
				igLinkBtnEl.InnerHtml.AppendHtml(igLinkBtnIconEl);

				igAppendEl.InnerHtml.AppendHtml(igLinkBtnEl);

				inputGroupEl.InnerHtml.AppendHtml(igAppendEl);
				#endregion

				output.Content.AppendHtml(inputGroupEl);
			}
			#endregion

			#region << Datasource Selected >>
			{
				var dsInputGroupEl = new TagBuilder("div");
				dsInputGroupEl.AddCssClass("input-group");
				if (DataSourceVariable == null)
				{
					dsInputGroupEl.AddCssClass("d-none");
				}

				#region << Form Control >>
				var divEl = new TagBuilder("div");
				divEl.AddCssClass("form-control disabled erp-text erp-linked p-0");
				divEl.Attributes.Add("id", $"input-{FieldId}-datasource");
				if (DataSourceVariable == null || DataSourceVariable.Type == DataSourceVariableType.DATASOURCE)
				{
					divEl.AddCssClass("datasource");
				}
				else if (DataSourceVariable != null && DataSourceVariable.Type == DataSourceVariableType.CODE)
				{
					divEl.AddCssClass("code");
				}
				else if (DataSourceVariable != null && DataSourceVariable.Type == DataSourceVariableType.SNIPPET)
				{
					divEl.AddCssClass("snippet");
				}
				else
				{
					divEl.AddCssClass("html");
				}
				var select2ContainerEl = new TagBuilder("span");
				select2ContainerEl.AddCssClass("select2 select2-container select2-container--bootstrap4 d-block disabled");

				var select2SelectionEl = new TagBuilder("span");
				select2SelectionEl.AddCssClass("selection");

				var select2SelectionInnerEl = new TagBuilder("span");
				select2SelectionInnerEl.AddCssClass("select2-selection select2-selection--multiple");

				var select2SelectionUlEl = new TagBuilder("ul");
				select2SelectionUlEl.AddCssClass("select2-selection__rendered");
				var optionEl = new TagBuilder("li");
				optionEl.AddCssClass("select2-selection__choice");
				if (DataSourceVariable != null && (DataSourceVariable.Type == DataSourceVariableType.DATASOURCE || DataSourceVariable.Type == DataSourceVariableType.SNIPPET))
				{
					optionEl.Attributes.Add("title", DataSourceVariable.String);
				}

				var optionIcon = new TagBuilder("i");

				if (DataSourceVariable == null || DataSourceVariable.Type == DataSourceVariableType.DATASOURCE)
				{
					optionIcon.AddCssClass("fa fa-fw fa-link mr-1");
					optionEl.InnerHtml.AppendHtml(optionIcon);
					var textSpan = new TagBuilder("span");
					textSpan.InnerHtml.AppendHtml(DataSourceVariable != null ? DataSourceVariable.String : "");
					optionEl.InnerHtml.AppendHtml(textSpan);
				}
				else if (DataSourceVariable != null && DataSourceVariable.Type == DataSourceVariableType.CODE)
				{
					optionIcon.AddCssClass("fa fa-fw fa-code mr-1");
					optionEl.InnerHtml.AppendHtml(optionIcon);
					var textSpan = new TagBuilder("span");
					textSpan.InnerHtml.AppendHtml("c# code");
					optionEl.InnerHtml.AppendHtml(textSpan);
				}
				else if (DataSourceVariable != null && DataSourceVariable.Type == DataSourceVariableType.SNIPPET)
				{
					optionIcon.AddCssClass("fa fa-fw fa-cog mr-1");
					optionEl.InnerHtml.AppendHtml(optionIcon);
					var textSpan = new TagBuilder("span");
					textSpan.InnerHtml.AppendHtml(DataSourceVariable.String);
					optionEl.InnerHtml.AppendHtml(textSpan);
				}
				else
				{
					optionIcon.AddCssClass("fa fa-fw fa-code mr-1");
					optionEl.InnerHtml.AppendHtml(optionIcon);
					var textSpan = new TagBuilder("span");
					textSpan.InnerHtml.AppendHtml("html");
					optionEl.InnerHtml.AppendHtml(textSpan);
				}

				select2SelectionUlEl.InnerHtml.AppendHtml(optionEl);
				select2SelectionInnerEl.InnerHtml.AppendHtml(select2SelectionUlEl);

				select2SelectionEl.InnerHtml.AppendHtml(select2SelectionInnerEl);
				select2ContainerEl.InnerHtml.AppendHtml(select2SelectionEl);
				divEl.InnerHtml.AppendHtml(select2ContainerEl);
				dsInputGroupEl.InnerHtml.AppendHtml(divEl);
				#endregion


				#region << Append >>
				var dsAppendEl = new TagBuilder("div");
				dsAppendEl.AddCssClass("input-group-append");

				var dsRemoveBtnEl = new TagBuilder("button");
				dsRemoveBtnEl.Attributes.Add("type", "button");
				dsRemoveBtnEl.Attributes.Add("id", $"input-{FieldId}-remove-ds-link");
				dsRemoveBtnEl.AddCssClass("btn btn-white");
				var dsRemoveBtnIconEl = new TagBuilder("i");
				dsRemoveBtnIconEl.AddCssClass("fa fa-times go-red  fa-fw");
				dsRemoveBtnEl.InnerHtml.AppendHtml(dsRemoveBtnIconEl);
				dsAppendEl.InnerHtml.AppendHtml(dsRemoveBtnEl);

				var dsEditBtnEl = new TagBuilder("button");
				dsEditBtnEl.Attributes.Add("type", "button");
				dsEditBtnEl.Attributes.Add("id", $"input-{FieldId}-edit-ds-link");
				dsEditBtnEl.AddCssClass("btn btn-white");
				var dsEditBtnIconEl = new TagBuilder("i");
				dsEditBtnIconEl.AddCssClass("fa fa-pencil-alt go-orange  fa-fw");
				dsEditBtnEl.InnerHtml.AppendHtml(dsEditBtnIconEl);
				dsAppendEl.InnerHtml.AppendHtml(dsEditBtnEl);

				dsInputGroupEl.InnerHtml.AppendHtml(dsAppendEl);
				output.Content.AppendHtml(dsInputGroupEl);
				#endregion

			}
			#endregion


			#region << Modal >>
			{
				var editModalEl = new TagBuilder("div");
				editModalEl.AddCssClass("modal");
				editModalEl.Attributes.Add("id", $"modal-{FieldId}-datasource");
				editModalEl.Attributes.Add("style", "padding-right: 15px;");
				editModalEl.Attributes.Add("tabindex", "-1");
				var editModalDialog = new TagBuilder("div");
				editModalDialog.AddCssClass("modal-dialog modal-xl");
				var editModalContent = new TagBuilder("div");
				editModalContent.AddCssClass("modal-content");

				#region << Modal Header >>
				var editModalHeader = new TagBuilder("div");
				editModalHeader.AddCssClass("modal-header");
				var editModalHeaderTitle = new TagBuilder("h5");
				editModalHeaderTitle.AddCssClass("modal-title");
				editModalHeaderTitle.InnerHtml.Append("Link option to dynamic value");
				editModalHeader.InnerHtml.AppendHtml(editModalHeaderTitle);
				var editModalHeaderButton = new TagBuilder("button");
				editModalHeaderButton.Attributes.Add("type", "button");
				editModalHeaderButton.AddCssClass("close");
				editModalHeaderButton.Attributes.Add("data-dismiss", "modal");
				editModalHeaderButton.InnerHtml.AppendHtml(new TagBuilder("span").InnerHtml.AppendHtml("&times;"));
				editModalHeader.InnerHtml.AppendHtml(editModalHeaderButton);
				editModalContent.InnerHtml.AppendHtml(editModalHeader);
				#endregion

				#region << Modal Body >>
				var editModalBody = new TagBuilder("div");
				editModalBody.AddCssClass("modal-body");

				#region << Type >>
				{
					var fieldGroupEl = new TagBuilder("div");
					fieldGroupEl.AddCssClass("form-group wv-field");
					var fieldLabel = new TagBuilder("label");
					fieldLabel.AddCssClass("control-label label-stacked");
					fieldLabel.InnerHtml.AppendHtml("Type");
					fieldGroupEl.InnerHtml.AppendHtml(fieldLabel);

					var fieldFormControl = new TagBuilder("div");
					fieldFormControl.AddCssClass("form-control-plaintext erp-radio-list");

					#region << Datasource chkb >>
					{
						var formCheckEl = new TagBuilder("div");
						formCheckEl.AddCssClass("form-check form-check-inline");
						var formCheckInput = new TagBuilder("input");
						formCheckInput.AddCssClass("form-check-input ds-type-radio");
						formCheckInput.Attributes.Add("type", "radio");
						formCheckInput.Attributes.Add("id", $"modal-{FieldId}-checkbox-datasource");
						formCheckInput.Attributes.Add("value", "0");
						if (DataSourceVariable == null || DataSourceVariable.Type == DataSourceVariableType.DATASOURCE)
						{
							formCheckInput.Attributes.Add("checked", "checked");
						}
						formCheckEl.InnerHtml.AppendHtml(formCheckInput);
						var formCheckLabel = new TagBuilder("label");
						formCheckLabel.AddCssClass("form-check-label");
						formCheckLabel.Attributes.Add("for", $"modal-{FieldId}-checkbox-datasource");
						formCheckLabel.InnerHtml.AppendHtml("datasource");
						formCheckEl.InnerHtml.AppendHtml(formCheckLabel);
						fieldFormControl.InnerHtml.AppendHtml(formCheckEl);
					}
					#endregion

					#region << Code chkb >>
					{
						var formCheckEl = new TagBuilder("div");
						formCheckEl.AddCssClass("form-check form-check-inline");
						var formCheckInput = new TagBuilder("input");
						formCheckInput.AddCssClass("form-check-input ds-type-radio");
						formCheckInput.Attributes.Add("type", "radio");
						formCheckInput.Attributes.Add("id", $"modal-{FieldId}-checkbox-code");
						formCheckInput.Attributes.Add("value", "1");
						if (DataSourceVariable != null && DataSourceVariable.Type == DataSourceVariableType.CODE)
						{
							formCheckInput.Attributes.Add("checked", "checked");
						}
						formCheckEl.InnerHtml.AppendHtml(formCheckInput);
						var formCheckLabel = new TagBuilder("label");
						formCheckLabel.AddCssClass("form-check-label");
						formCheckLabel.Attributes.Add("for", $"modal-{FieldId}-checkbox-code");
						formCheckLabel.InnerHtml.AppendHtml("code");
						formCheckEl.InnerHtml.AppendHtml(formCheckLabel);
						fieldFormControl.InnerHtml.AppendHtml(formCheckEl);
					}
					#endregion

					#region << Html chkb >>
					{
						var formCheckEl = new TagBuilder("div");
						formCheckEl.AddCssClass("form-check form-check-inline");
						var formCheckInput = new TagBuilder("input");
						formCheckInput.AddCssClass("form-check-input ds-type-radio");
						formCheckInput.Attributes.Add("type", "radio");
						formCheckInput.Attributes.Add("id", $"modal-{FieldId}-checkbox-html");
						formCheckInput.Attributes.Add("value", "2");
						if (DataSourceVariable != null && DataSourceVariable.Type == DataSourceVariableType.HTML)
						{
							formCheckInput.Attributes.Add("checked", "checked");
						}
						formCheckEl.InnerHtml.AppendHtml(formCheckInput);
						var formCheckLabel = new TagBuilder("label");
						formCheckLabel.AddCssClass("form-check-label");
						formCheckLabel.Attributes.Add("for", $"modal-{FieldId}-checkbox-html");
						formCheckLabel.InnerHtml.AppendHtml("html");
						formCheckEl.InnerHtml.AppendHtml(formCheckLabel);
						fieldFormControl.InnerHtml.AppendHtml(formCheckEl);
					}
					#endregion

					#region << Snippet chkb >>
					{
						var formCheckEl = new TagBuilder("div");
						formCheckEl.AddCssClass("form-check form-check-inline");
						var formCheckInput = new TagBuilder("input");
						formCheckInput.AddCssClass("form-check-input ds-type-radio");
						formCheckInput.Attributes.Add("type", "radio");
						formCheckInput.Attributes.Add("id", $"modal-{FieldId}-checkbox-snippet");
						formCheckInput.Attributes.Add("value", "3");
						if (DataSourceVariable != null && DataSourceVariable.Type == DataSourceVariableType.SNIPPET)
						{
							formCheckInput.Attributes.Add("checked", "checked");
						}
						formCheckEl.InnerHtml.AppendHtml(formCheckInput);
						var formCheckLabel = new TagBuilder("label");
						formCheckLabel.AddCssClass("form-check-label");
						formCheckLabel.Attributes.Add("for", $"modal-{FieldId}-checkbox-snippet");
						formCheckLabel.InnerHtml.AppendHtml("resource");
						formCheckEl.InnerHtml.AppendHtml(formCheckLabel);
						fieldFormControl.InnerHtml.AppendHtml(formCheckEl);
					}
					#endregion

					fieldGroupEl.InnerHtml.AppendHtml(fieldFormControl);
					editModalBody.InnerHtml.AppendHtml(fieldGroupEl);
				}
				#endregion

				#region << Datasource >>
				{
					var fieldGroupEl = new TagBuilder("div");
					fieldGroupEl.AddCssClass("form-group wv-field");
					if (DataSourceVariable != null && DataSourceVariable.Type != DataSourceVariableType.DATASOURCE)
					{
						fieldGroupEl.AddCssClass("d-none");
					}
					fieldGroupEl.Attributes.Add("id", $"modal-{FieldId}-datasource-group");
					var fieldLabel = new TagBuilder("label");
					fieldLabel.AddCssClass("control-label label-stacked");
					fieldLabel.InnerHtml.AppendHtml("Value");
					fieldGroupEl.InnerHtml.AppendHtml(fieldLabel);

					var inputEl = new TagBuilder("input");
					inputEl.AddCssClass("form-control erp-text");
					inputEl.Attributes.Add("type", "text");

					if (DataSourceVariable != null && DataSourceVariable.Type == DataSourceVariableType.DATASOURCE)
					{
						inputEl.Attributes.Add("value", DataSourceVariable.String);
					}
					else
					{
						inputEl.Attributes.Add("value", "");
					}
					fieldGroupEl.InnerHtml.AppendHtml(inputEl);

					editModalBody.InnerHtml.AppendHtml(fieldGroupEl);
				}
				#endregion

				#region << Code >>
				{
					var fieldGroupEl = new TagBuilder("div");
					fieldGroupEl.AddCssClass("form-group wv-field");
					if (DataSourceVariable == null || DataSourceVariable.Type != DataSourceVariableType.CODE)
					{
						fieldGroupEl.AddCssClass("d-none");
					}
					fieldGroupEl.Attributes.Add("id", $"modal-{FieldId}-code-group");
					var fieldLabel = new TagBuilder("label");
					fieldLabel.AddCssClass("control-label label-stacked");
					fieldLabel.InnerHtml.AppendHtml("Value (templates: <a href='javascript:void(0)' class='simple-code ml-1'>simple</a>, <a href='javascript:void(0)' class='datasource-code ml-1'>data source get</a>, <a href='javascript:void(0)' class='datasource-selectoptions-code ml-1'>List&lt;EntityRecord&gt; to List&lt;SelectOptions&gt;</a>)");
					fieldGroupEl.InnerHtml.AppendHtml(fieldLabel);

					var editorWrapperEl = new TagBuilder("div");
					var wrapperCssClassList = new List<string>();
					wrapperCssClassList.Add("form-control-plaintext erp-code");
					if (ValidationErrors.Count > 0)
					{
						wrapperCssClassList.Add("is-invalid");
					}
					editorWrapperEl.Attributes.Add("class", String.Join(' ', wrapperCssClassList));
					var editorWrapper = new TagBuilder("div");
					editorWrapper.Attributes.Add("id", $"modal-{FieldId}-code-editor");
					editorWrapper.Attributes.Add("style", $"min-height:250px");
					editorWrapperEl.InnerHtml.AppendHtml(editorWrapper);
					fieldGroupEl.InnerHtml.AppendHtml(editorWrapperEl);

					var inputEl = new TagBuilder("input");
					inputEl.Attributes.Add("type", "hidden");
					inputEl.Attributes.Add("id", $"modal-{FieldId}-code-input");
					if (DataSourceVariable != null && DataSourceVariable.Type == DataSourceVariableType.CODE)
					{
						inputEl.Attributes.Add("value", DataSourceVariable.String);
					}
					else
					{
						var defaultCode = @"using System;
using System.Collections.Generic;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Api.Models;
using Newtonsoft.Json;

public class SampleCodeVariable : ICodeVariable
{
	public object Evaluate(BaseErpPageModel pageModel)
	{
		try{
			return DateTime.Now;
		}
		catch(Exception ex){
			return ""Error: "" + ex.Message;
		}
	}
}";
						inputEl.Attributes.Add("value", defaultCode);
					}
					fieldGroupEl.InnerHtml.AppendHtml(inputEl);
					editModalBody.InnerHtml.AppendHtml(fieldGroupEl);

				}
				#endregion


				#region << HTML >>
				{
					var fieldGroupEl = new TagBuilder("div");
					fieldGroupEl.AddCssClass("form-group wv-field");
					if (DataSourceVariable == null || DataSourceVariable.Type != DataSourceVariableType.HTML)
					{
						fieldGroupEl.AddCssClass("d-none");
					}
					fieldGroupEl.Attributes.Add("id", $"modal-{FieldId}-html-group");
					var fieldLabel = new TagBuilder("label");
					fieldLabel.AddCssClass("control-label label-stacked");
					fieldLabel.InnerHtml.AppendHtml("Value");
					fieldGroupEl.InnerHtml.AppendHtml(fieldLabel);

					var editorWrapperEl = new TagBuilder("div");
					var wrapperCssClassList = new List<string>();
					wrapperCssClassList.Add("form-control-plaintext erp-code");
					if (ValidationErrors.Count > 0)
					{
						wrapperCssClassList.Add("is-invalid");
					}
					editorWrapperEl.Attributes.Add("class", String.Join(' ', wrapperCssClassList));
					var editorWrapper = new TagBuilder("div");
					editorWrapper.Attributes.Add("id", $"modal-{FieldId}-html-editor");
					editorWrapper.Attributes.Add("style", $"min-height:250px");
					editorWrapperEl.InnerHtml.AppendHtml(editorWrapper);
					fieldGroupEl.InnerHtml.AppendHtml(editorWrapperEl);

					var inputEl = new TagBuilder("input");
					inputEl.Attributes.Add("type", "hidden");
					inputEl.Attributes.Add("id", $"modal-{FieldId}-html-input");
					if (DataSourceVariable != null && DataSourceVariable.Type == DataSourceVariableType.HTML)
					{
						inputEl.Attributes.Add("value", DataSourceVariable.String);
					}
					else
					{
						var defaultCode = @"<div>HTML code</div>";
						inputEl.Attributes.Add("value", defaultCode);
					}
					fieldGroupEl.InnerHtml.AppendHtml(inputEl);
					editModalBody.InnerHtml.AppendHtml(fieldGroupEl);

				}
				#endregion

				#region << File Snippet >>
				var snippetWrapEl = new TagBuilder("div");
				if (DataSourceVariable == null || DataSourceVariable.Type != DataSourceVariableType.SNIPPET)
				{
					snippetWrapEl.AddCssClass("d-none");
				}
				snippetWrapEl.Attributes.Add("id", $"modal-{FieldId}-snippet-group");

				{
					var fieldGroupEl = new TagBuilder("div");
					fieldGroupEl.AddCssClass("form-group wv-field");

					var fieldLabel = new TagBuilder("label");
					fieldLabel.AddCssClass("control-label label-stacked");
					fieldLabel.InnerHtml.AppendHtml("Embedded Resource Name");
					fieldGroupEl.InnerHtml.AppendHtml(fieldLabel);

					var inputEl = new TagBuilder("input");
					inputEl.AddCssClass("form-control erp-text");
					inputEl.Attributes.Add("type", "text");
					if (DataSourceVariable != null && DataSourceVariable.Type == DataSourceVariableType.SNIPPET)
					{
						inputEl.Attributes.Add("value", DataSourceVariable.String);
					}
					else
					{
						inputEl.Attributes.Add("value", "");
					}
					fieldGroupEl.InnerHtml.AppendHtml(inputEl);
					snippetWrapEl.InnerHtml.AppendHtml(fieldGroupEl);
				}
				{
					var fieldGroupEl = new TagBuilder("div");
					fieldGroupEl.AddCssClass("form-group wv-field");

					var fieldLabel = new TagBuilder("label");
					fieldLabel.AddCssClass("control-label label-stacked");
					fieldLabel.InnerHtml.AppendHtml("Resource Content");
					fieldGroupEl.InnerHtml.AppendHtml(fieldLabel);

					var editorWrapperEl = new TagBuilder("div");
					var wrapperCssClassList = new List<string>();
					wrapperCssClassList.Add("form-control-plaintext erp-code");
					if (ValidationErrors.Count > 0)
					{
						wrapperCssClassList.Add("is-invalid");
					}
					editorWrapperEl.Attributes.Add("class", String.Join(' ', wrapperCssClassList));
					var editorWrapper = new TagBuilder("div");
					editorWrapper.Attributes.Add("id", $"modal-{FieldId}-snippet-editor");
					editorWrapper.Attributes.Add("style", $"min-height:250px");
					editorWrapperEl.InnerHtml.AppendHtml(editorWrapper);
					fieldGroupEl.InnerHtml.AppendHtml(editorWrapperEl);

					var inputEl = new TagBuilder("input");
					inputEl.Attributes.Add("type", "hidden");
					inputEl.Attributes.Add("id", $"modal-{FieldId}-snippet-input");
					if (DataSourceVariable != null && 
						DataSourceVariable.Type == DataSourceVariableType.SNIPPET && !String.IsNullOrWhiteSpace(DataSourceVariable.String))
					{
						var snippet = SnippetService.GetSnippet(DataSourceVariable.String);
						inputEl.Attributes.Add("value", snippet.GetText());
					}
					fieldGroupEl.InnerHtml.AppendHtml(inputEl);
					snippetWrapEl.InnerHtml.AppendHtml(fieldGroupEl);
				}

				editModalBody.InnerHtml.AppendHtml(snippetWrapEl);
				#endregion

				#region << Default >>
				{
					var fieldGroupEl = new TagBuilder("div");
					fieldGroupEl.AddCssClass("form-group wv-field");
					fieldGroupEl.Attributes.Add("id", $"modal-{FieldId}-default-group");
					var fieldLabel = new TagBuilder("label");
					fieldLabel.AddCssClass("control-label label-stacked");
					fieldLabel.InnerHtml.AppendHtml("Default Value");
					fieldGroupEl.InnerHtml.AppendHtml(fieldLabel);

					var inputEl = new TagBuilder("input");
					inputEl.AddCssClass("form-control erp-text");
					inputEl.Attributes.Add("type", "text");
					if (DataSourceVariable != null)
					{
						inputEl.Attributes.Add("value", DataSourceVariable.Default);
					}
					else
					{
						inputEl.Attributes.Add("value", "");
					}
					fieldGroupEl.InnerHtml.AppendHtml(inputEl);
					editModalBody.InnerHtml.AppendHtml(fieldGroupEl);
				}
				#endregion


				editModalContent.InnerHtml.AppendHtml(editModalBody);
				#endregion

				#region << modal footer >>
				var editModalFooter = new TagBuilder("div");
				editModalFooter.AddCssClass("modal-footer");

				var editModalFooterTest = new TagBuilder("button");
				editModalFooterTest.Attributes.Add("type", "button");
				editModalFooterTest.AddCssClass("btn btn-secondary test btn-sm d-none");
				editModalFooterTest.Attributes.Add("style", "position: absolute;left: 10px;");
				editModalFooterTest.InnerHtml.AppendHtml("<i class='fa fa-cog'></i> test code");
				editModalFooter.InnerHtml.AppendHtml(editModalFooterTest);

				var editModalFooterSave = new TagBuilder("button");
				editModalFooterSave.Attributes.Add("type", "button");
				editModalFooterSave.AddCssClass("btn btn-green btn-sm submit");
				var editModalFooterSaveIcon = new TagBuilder("span");
				editModalFooterSaveIcon.AddCssClass("fa fa-link mr-1");
				editModalFooterSave.InnerHtml.AppendHtml(editModalFooterSaveIcon);
				editModalFooterSave.InnerHtml.AppendHtml("create link");
				editModalFooter.InnerHtml.AppendHtml(editModalFooterSave);
				var editModalFooterCancel = new TagBuilder("button");
				editModalFooterCancel.Attributes.Add("type", "button");
				editModalFooterCancel.AddCssClass("btn btn-secondary cancel btn-sm");
				editModalFooterCancel.InnerHtml.Append("cancel");
				editModalFooter.InnerHtml.AppendHtml(editModalFooterCancel);
				editModalContent.InnerHtml.AppendHtml(editModalFooter);
				#endregion


				editModalDialog.InnerHtml.AppendHtml(editModalContent);
				editModalEl.InnerHtml.AppendHtml(editModalDialog);

				output.Content.AppendHtml(editModalEl);
			}
			#endregion

			var jsCompressor = new JavaScriptCompressor();

			#region << Init Select2 >>
			{
				var wvLibraryInitialized = false;
				var libraryItemsKey = "WebVella-" + "select2";
				if (ViewContext.HttpContext.Items.ContainsKey(libraryItemsKey))
				{
					var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[libraryItemsKey];
					wvLibraryInitialized = tagHelperContext.Initialized;
				}

				if (!wvLibraryInitialized)
				{
					{
						var libCssEl = new TagBuilder("link");
						libCssEl.Attributes.Add("href", "/_content/WebVella.TagHelpers/lib/select2/css/select2.min.css");
						libCssEl.Attributes.Add("type", "text/css");
						libCssEl.Attributes.Add("rel", "stylesheet");
						output.PostContent.AppendHtml(libCssEl);
						output.PostContent.AppendHtml(Environment.NewLine + "\t");
					}
					{
						var libCssEl = new TagBuilder("link");
						libCssEl.Attributes.Add("href", "/_content/WebVella.TagHelpers/lib/select2-bootstrap-theme/select2-bootstrap4.css");
						libCssEl.Attributes.Add("type", "text/css");
						libCssEl.Attributes.Add("rel", "stylesheet");
						output.PostContent.AppendHtml(libCssEl);
						output.PostContent.AppendHtml(Environment.NewLine + "\t");
					}

					var libJsEl = new TagBuilder("script");
					libJsEl.Attributes.Add("type", "text/javascript");
					libJsEl.Attributes.Add("src", "/_content/WebVella.TagHelpers/lib/select2/js/select2.min.js");
					output.PostContent.AppendHtml(libJsEl);
					output.PostContent.AppendHtml(Environment.NewLine+"\t");

					ViewContext.HttpContext.Items[libraryItemsKey] = new WvTagHelperContext()
					{
						Initialized = true
					};
				}
			}
			#endregion

			#region << Add Ace lib >>
			{
				var aceLibInitialized = false;
				var fileName = "ace.js";
				if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldDatasource) + fileName))
				{
					var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldDatasource) + fileName];
					aceLibInitialized = tagHelperContext.Initialized;
				}
				if (!aceLibInitialized)
				{
					var scriptEl = new TagBuilder("script");
					scriptEl.Attributes.Add("type", "text/javascript");
					scriptEl.Attributes.Add("src", "/_content/WebVella.TagHelpers/lib/ace/ace.js");
					output.Content.AppendHtml(scriptEl);

					ViewContext.HttpContext.Items[typeof(WvFieldDatasource) + fileName] = new WvTagHelperContext()
					{
						Initialized = true
					};

				}
			}
			#endregion

			#region << Add Typeahead lib >>
			{
				var aceLibInitialized = false;
				var fileName = "bootstrap3-typeahead.js";
				if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldDatasource) + fileName))
				{
					var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldDatasource) + fileName];
					aceLibInitialized = tagHelperContext.Initialized;
				}
				if (!aceLibInitialized)
				{
					var scriptEl = new TagBuilder("script");
					scriptEl.Attributes.Add("type", "text/javascript");
					scriptEl.Attributes.Add("src", "/_content/WebVella.TagHelpers/lib/bootstrap-3-typeahead/bootstrap3-typeahead.min.js");
					output.Content.AppendHtml(scriptEl);

					ViewContext.HttpContext.Items[typeof(WvFieldDatasource) + fileName] = new WvTagHelperContext()
					{
						Initialized = true
					};

				}
			}
			#endregion


			#region << Init Scripts >>
			{
				var tagHelperInitialized = false;
				if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFieldDatasource) + "-form"))
				{
					var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFieldDatasource) + "-form"];
					tagHelperInitialized = tagHelperContext.Initialized;
				}
				if (!tagHelperInitialized)
				{
					var scriptContent = FileService.GetEmbeddedTextResource("form.js", "WebVella.Erp.Web.TagHelpers.WvFieldDatasource");
					var scriptEl = new TagBuilder("script");
					scriptEl.Attributes.Add("type", "text/javascript");
					//scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
					scriptEl.InnerHtml.AppendHtml(scriptContent);
					output.Content.AppendHtml(scriptEl);

					ViewContext.HttpContext.Items[typeof(WvFieldDatasource) + "-form"] = new WvTagHelperContext()
					{
						Initialized = true
					};

				}
			}
			#endregion

			#region << Add Inline Init Script for this instance >>
			{
				var initScript = new TagBuilder("script");
				initScript.Attributes.Add("type", "text/javascript");
				var scriptTemplate = @"
						$(function(){
							DataSourceFormEditInit(""{{FieldId}}"",{{PropertyNameCsv}});
						});";
				scriptTemplate = scriptTemplate.Replace("{{FieldId}}", (FieldId ?? null).ToString());
				scriptTemplate = scriptTemplate.Replace("{{PropertyNameCsv}}", JsonConvert.SerializeObject(PropertyNameLibrary));

				initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

				output.Content.AppendHtml(initScript);
			}
			#endregion



			//Finally
			if (SubInputEl != null)
			{
				output.Content.AppendHtml(SubInputEl);
			}

			return Task.CompletedTask;
		}


	}
}
