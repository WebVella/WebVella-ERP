using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;
using WebVella.Erp.Exceptions;

namespace WebVella.Erp.Web.TagHelpers
{
	public abstract class WvFieldBase : TagHelper
	{
		[HtmlAttributeName("is-visible")]
		public bool isVisible { get; set; } = true;

		//Label
		[HtmlAttributeName("label-mode")]
		public LabelRenderMode LabelMode { get; set; } = LabelRenderMode.Undefined;

		[HtmlAttributeName("label-text")]
		public string LabelText { get; set; } = "";

		[HtmlAttributeName("label-warning-text")]
		public string LabelWarningText { get; set; } = "";

		[HtmlAttributeName("label-error-text")]
		public string LabelErrorText { get; set; } = "";

		[HtmlAttributeName("label-help-text")]
		public string LabelHelpText { get; set; } = "";

		//Field
		[HtmlAttributeName("field-id")]
		public Guid? FieldId { get; set; } = null;

		[HtmlAttributeName("id")]
		public string Id { get; set; } = "";

		[HtmlAttributeName("name")]
		public string Name { get; set; } = "";

		[HtmlAttributeName("class")]
		public string Class { get; set; } = "";

		[HtmlAttributeName("value")]
		public dynamic Value { get; set; } = null;

		[HtmlAttributeName("default-value")]
		public dynamic DefaultValue { get; set; } = null;

		[HtmlAttributeName("mode")]
		public FieldRenderMode Mode { get; set; } = FieldRenderMode.Undefined;

		[HtmlAttributeName("access")]
		public FieldAccess Access { get; set; } = FieldAccess.Full;

		[HtmlAttributeName("init-errors")]
		public List<string> InitErrors { get; set; } = new List<string>();

		[HtmlAttributeName("validation-errors")]
		public List<ValidationError> ValidationErrors { get; set; } = new List<ValidationError>();

		[HtmlAttributeName("record-id")]
		public Guid? RecordId { get; set; } = null;

		[HtmlAttributeName("entity-name")]
		public string EntityName { get; set; } = "";

		[HtmlAttributeName("api-url")]
		public string ApiUrl { get; set; } = "";

		[HtmlAttributeName("access-denied-message")]
		public string AccessDeniedMessage { get; set; } = "access denied";

		[HtmlAttributeName("empty-value-message")]
		public string EmptyValueMessage { get; set; } = "no data";

		[HtmlAttributeName("required")]
		public bool Required { get; set; } = false;

		[HtmlAttributeName("placeholder")]
		public string Placeholder { get; set; } = "";

		[HtmlAttributeName("description")]
		public string Description { get; set; } = "";

		[HtmlAttributeName("locale")]
		public string Locale { get; set; } = "";

		[HtmlAttributeName("autocomplete")]
		public bool? AutoComplete { get; set; } = null;

		public List<string> PrependHtml { get; set; } = new List<string>();

		public List<string> AppendHtml { get; set; } = new List<string>();

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		protected CultureInfo Culture { get; set; } = new CultureInfo("en-US");

		protected TagBuilder SubInputEl { get; set; } = null;

		protected TagBuilder EmptyValEl { get; set; } = null;

		private List<string> CssClassList { get; set; } = new List<string>();

		public bool InitField(TagHelperContext context, TagHelperOutput output)
		{
			var isSuccess = true;

			#region << Init >>
			if (FieldId == null)
			{
				FieldId = Guid.NewGuid();
			}

			if (String.IsNullOrWhiteSpace(ApiUrl) && Mode == FieldRenderMode.InlineEdit && (RecordId == null || String.IsNullOrWhiteSpace(EntityName)))
			{
				InitErrors.Add("In 'inlineEdit' when 'api-url' is not defined, entityName and recordId are required!");
			}

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

			if (!String.IsNullOrWhiteSpace(Locale))
			{
				Culture = new CultureInfo(Locale);
			}

			if (String.IsNullOrWhiteSpace(Name) && (Mode == FieldRenderMode.Form || Mode == FieldRenderMode.InlineEdit) 
				&& context.TagName != "wv-field-plaintext")
			{
				InitErrors.Add("In InlineEdit or Form the attribute 'name' is required");
			}
			if (ValidationErrors.Count > 0)
			{
				ValidationErrors = ValidationErrors.FindAll(x => x.PropertyName == Name.ToLowerInvariant()).ToList();
			}

			if (ValidationErrors.Count == 0)
			{
				//Check if it is defined in form group
				if (context.Items.ContainsKey(typeof(ValidationException)))
				{
					var validation = (ValidationException)context.Items[typeof(ValidationException)];
					if (validation != null & validation.Errors.Count > 0)
					{
						ValidationErrors = validation.Errors.FindAll(x => x.PropertyName == Name.ToLowerInvariant());
					}
				}
			}

			if (AutoComplete == null) {
				if (context.Items.ContainsKey("FromAutocomplete")) {
					AutoComplete = (bool)context.Items["FromAutocomplete"];
				}
			}

			#region << Init Value >>
			var tagName = context.TagName;
			var errorList = new List<string>();
			FieldType? fieldType = FieldType.TextField;
			switch (context.TagName)
			{
				case "wv-field-autonumber":
					fieldType = FieldType.AutoNumberField;
					break;
				case "wv-field-checkbox":
					fieldType = FieldType.CheckboxField;
					break;
				case "wv-field-checkbox-grid":
					{
						fieldType = FieldType.TextField;
					}
					break;
				case "wv-field-currency":
					fieldType = FieldType.CurrencyField;
					break;
				case "wv-field-date":
					fieldType = FieldType.DateField;
					break;
				case "wv-field-datetime":
				case "wv-field-time":
					fieldType = FieldType.DateTimeField;
					break;
				case "wv-field-email":
					fieldType = FieldType.EmailField;
					break;
				case "wv-field-file":
					fieldType = FieldType.FileField;
					break;
				case "wv-field-html":
					fieldType = FieldType.HtmlField;
					break;
				case "wv-field-image":
					fieldType = FieldType.ImageField;
					break;
				case "wv-field-textarea":
                case "wv-field-data-csv":
                    fieldType = FieldType.MultiLineTextField;
					break;
				case "wv-field-multiselect":
				case "wv-field-checkbox-list":
					fieldType = FieldType.MultiSelectField;
                    if (Value == null)
                    {
                        Value = new List<string>();
                    }
                    else if (Value is List<string>)
                    {

                    }
                    else if (Value is List<SelectOption>)
                    {
                        var newListString = new List<string>();
                        foreach (var option in (List<SelectOption>)Value)
                        {
                            newListString.Add(option.Value);
                        }
                        Value = newListString;
                    }
                    else if (Value is string) {
                        var stringValue = Value.ToString();
  
                        Value = new List<string>() { stringValue };
                    }
                    else
                    {
                        throw new Exception("Expected multiselect value is List<string> or List<SelectOption>");
                    }
					break;
				case "wv-field-number":
					fieldType = FieldType.NumberField;
					break;
				case "wv-field-password":
					fieldType = FieldType.PasswordField;
					break;
				case "wv-field-percent":
					fieldType = FieldType.PercentField;
					break;
				case "wv-field-phone":
					fieldType = FieldType.PhoneField;
					break;
				case "wv-field-guid":
					fieldType = FieldType.GuidField;
					break;
				case "wv-field-select":
					fieldType = FieldType.SelectField;
					break;
				case "wv-field-text":
					fieldType = FieldType.TextField;
					break;
				case "wv-field-url":
					fieldType = FieldType.UrlField;
					break;
				default:
					fieldType = null;
					break;
			}
			dynamic valueResult = null;
			DataUtils.ValidateValueToFieldType(fieldType, Value, out valueResult, out errorList);
			Value = valueResult;
			dynamic defaultValueResult = null;
			DataUtils.ValidateValueToFieldType(fieldType, DefaultValue, out defaultValueResult, out errorList);
			DefaultValue = defaultValueResult;

			if (errorList.Count > 0)
			{
				foreach (var error in errorList)
				{
					InitErrors.Add(error);
				}
			}
			#endregion

			#endregion

			#region << Render Field Group / Output tag >>
			output.TagName = "div";
			if (!String.IsNullOrWhiteSpace(Class)) {
				CssClassList.Add(Class);
			}
			CssClassList.Add("form-group");
			CssClassList.Add("erp-field");
			if (LabelMode == LabelRenderMode.Horizontal)
			{
				CssClassList.Add("label-horizontal");
			}
			if (!String.IsNullOrWhiteSpace(Id))
			{
				output.Attributes.Add("id",Id);
			}


			switch (LabelMode)
			{
				case LabelRenderMode.Hidden:
					CssClassList.Add("label-hidden");
					break;
				case LabelRenderMode.Horizontal:
					CssClassList.Add("row no-gutters");
					break;
			}

			switch (Mode)
			{
				case FieldRenderMode.Display:
					CssClassList.Add("display");
					break;
				case FieldRenderMode.Form:
					CssClassList.Add("form");
					break;
				case FieldRenderMode.InlineEdit:
					CssClassList.Add("inline-edit");
					break;
				case FieldRenderMode.Simple:
					CssClassList.Add("list");
					break;
			}

			output.Attributes.SetAttribute("class", String.Join(' ', CssClassList));

			#endregion

			#region << Render Label >>
			if (LabelMode != LabelRenderMode.Hidden)
			{
				var labelEl = new TagBuilder("label");
				//Set label attributes
				if (FieldId != null)
				{
					labelEl.Attributes.Add("for", "input-" + FieldId);
				}
				if (LabelMode == LabelRenderMode.Horizontal)
				{
					labelEl.Attributes.Add("class", "col-12 col-sm-auto col-form-label label-horizontal pr-0 pr-sm-2");
				}
				else
				{
					labelEl.Attributes.Add("class", "control-label label-stacked");
				}

				//Set Required 
				if (Required &&  Mode == FieldRenderMode.Form)
				{
					var requiredEl = new TagBuilder("abbr");
					requiredEl.MergeAttribute("class", "go-red");
					requiredEl.MergeAttribute("title", "required");
					requiredEl.InnerHtml.Append("*");
					labelEl.InnerHtml.AppendHtml(requiredEl);
				}

				//Set Label text
				if (LabelMode != LabelRenderMode.Horizontal)
				{
					labelEl.InnerHtml.AppendHtml(LabelText);
				}
				else {
					labelEl.InnerHtml.AppendHtml(LabelText + ":");
				}

					//Set Help
				if (!String.IsNullOrWhiteSpace(LabelHelpText))
				{
					var helpEl = new TagBuilder("i");
					helpEl.MergeAttribute("class", "fa fa-fw fa-question-circle field-help");
					helpEl.MergeAttribute("data-toggle", "tooltip");
					helpEl.MergeAttribute("data-placement", "top");
					helpEl.MergeAttribute("data-html", "true");
					helpEl.MergeAttribute("id", "help-message-" + FieldId);
					helpEl.MergeAttribute("title", LabelHelpText);
					labelEl.InnerHtml.AppendHtml(helpEl);
					var scriptEl = new TagBuilder("script");
					var script = $"$(function () {{$('#help-message-{FieldId}').tooltip();}})";
					scriptEl.InnerHtml.AppendHtml(script);
					labelEl.InnerHtml.AppendHtml(scriptEl);
				};

				//Set Warning
				if (!String.IsNullOrWhiteSpace(LabelWarningText))
				{
					var helpEl = new TagBuilder("i");
					helpEl.MergeAttribute("class", "fa fa-fw fa-exclamation-triangle field-warning-message");
					helpEl.MergeAttribute("data-toggle", "tooltip");
					helpEl.MergeAttribute("data-placement", "top");
					helpEl.MergeAttribute("data-html", "true");
					helpEl.MergeAttribute("id", "warning-message-" + FieldId);
					helpEl.MergeAttribute("data-delay", "{ \"show\": 100, \"hide\": 3000 }");
					helpEl.MergeAttribute("title", LabelWarningText);
					labelEl.InnerHtml.AppendHtml(helpEl);
					var scriptEl = new TagBuilder("script");
					var script = $"$(function () {{$('#warning-message-{FieldId}').tooltip();}})";
					scriptEl.InnerHtml.AppendHtml(script);
					labelEl.InnerHtml.AppendHtml(scriptEl);
				};

				//Set Error
				if (!String.IsNullOrWhiteSpace(LabelErrorText))
				{
					var helpEl = new TagBuilder("i");
					helpEl.MergeAttribute("class", "fa fa-fw fa-exclamation-circle field-error-message");
					helpEl.MergeAttribute("data-toggle", "tooltip");
					helpEl.MergeAttribute("data-placement", "top");
					helpEl.MergeAttribute("data-html", "true");
					helpEl.MergeAttribute("id", "error-message-" + FieldId);
					helpEl.MergeAttribute("data-delay", "{ \"show\": 100, \"hide\": 3000 }");
					helpEl.MergeAttribute("title", LabelErrorText);
					labelEl.InnerHtml.AppendHtml(helpEl);
					var scriptEl = new TagBuilder("script");
					var script = $"$(function () {{$('#error-message-{FieldId}').tooltip();}})";
					scriptEl.InnerHtml.AppendHtml(script);
					labelEl.InnerHtml.AppendHtml(scriptEl);
				};

				output.PreContent.AppendHtml(labelEl);
			}

			#endregion


			#region << Field Outer Wrapper tag - StartTag >>
			var fieldWrapper = new TagBuilder("div");
			fieldWrapper.AddCssClass("col");

			if (LabelMode == LabelRenderMode.Horizontal)
			{
				output.PreContent.AppendHtml(fieldWrapper.RenderStartTag());
			}
			#endregion

			#region << if Init Errors >>
			if (InitErrors.Count > 0)
			{
				var errorListEl = new TagBuilder("ul");
				errorListEl.AddCssClass("erp-error-list list-unstyled");
				foreach (var error in InitErrors)
				{
					var errorEl = new TagBuilder("li");
					errorEl.AddCssClass("go-red");

					var iconEl = new TagBuilder("span");
					iconEl.AddCssClass("fa fa-fw fa-exclamation");

					errorEl.InnerHtml.AppendHtml(iconEl);
					errorEl.InnerHtml.Append($"Error: {error}");

					errorListEl.InnerHtml.AppendHtml(errorEl);
				}
				output.PostContent.AppendHtml(errorListEl);
				return false;
			}
			#endregion

			#region << if Forbidden >>
			if (Access == FieldAccess.Forbidden)
			{
				if (Mode != FieldRenderMode.Simple)
				{
					var forbiddenEl = new TagBuilder("div");
					forbiddenEl.AddCssClass("form-control-plaintext");

					var innerSpan = new TagBuilder("span");
					innerSpan.AddCssClass("go-gray");

					var innerIcon = new TagBuilder("span");
					innerIcon.AddCssClass("fa fa-lock mr-1");

					var innerEm = new TagBuilder("em");
					innerEm.InnerHtml.Append(AccessDeniedMessage);

					innerSpan.InnerHtml.AppendHtml(innerIcon);
					innerSpan.InnerHtml.AppendHtml(innerEm);
					forbiddenEl.InnerHtml.AppendHtml(innerSpan);

					output.PostContent.AppendHtml(forbiddenEl);
					return false;
				}
				else
				{
					var innerSpan = new TagBuilder("span");
					innerSpan.AddCssClass("go-gray");

					var innerIcon = new TagBuilder("span");
					innerIcon.AddCssClass("fa fa-lock mr-1");

					var innerEm = new TagBuilder("em");
					innerEm.InnerHtml.Append(AccessDeniedMessage);
					innerSpan.InnerHtml.AppendHtml(innerIcon);
					innerSpan.InnerHtml.AppendHtml(innerEm);

					output.SuppressOutput();
					output.PostContent.AppendHtml(innerSpan);

					return false;
				}
			}
			#endregion

			#region << Field Outer Wrapper tag - End Tag >>
			if (LabelMode == LabelRenderMode.Horizontal)
			{
				output.PostContent.AppendHtml(fieldWrapper.RenderEndTag());
			}
			#endregion

			#region << Init SubInputEl >>

			if (ValidationErrors.Count == 1)
			{
				SubInputEl = new TagBuilder("div");
				SubInputEl.AddCssClass("invalid-feedback d-block");
				SubInputEl.InnerHtml.AppendHtml(ValidationErrors.First().Message);
			}
			else if (ValidationErrors.Count > 1)
			{
				SubInputEl = new TagBuilder("div");
				SubInputEl.AddCssClass("invalid-feedback d-block");
				var errorListEl = new TagBuilder("ul");
				foreach (var error in ValidationErrors)
				{
					var errorEl = new TagBuilder("li");
					errorEl.InnerHtml.AppendHtml(error.Message);
					errorListEl.InnerHtml.AppendHtml(errorEl);
				}
				SubInputEl.InnerHtml.AppendHtml(errorListEl);
			}
			else if (!String.IsNullOrWhiteSpace(Description))
			{
				SubInputEl = new TagBuilder("div");
				SubInputEl.AddCssClass("field-description form-text text-muted");
				SubInputEl.InnerHtml.AppendHtml(Description);
			}
			#endregion

			#region << Init  EmptyValEl>>
			{
				EmptyValEl = new TagBuilder("div");
				EmptyValEl.AddCssClass("form-control-plaintext");

				var innerSpan = new TagBuilder("span");
				innerSpan.AddCssClass("go-gray");

				var innerEm = new TagBuilder("em");
				innerEm.InnerHtml.Append(EmptyValueMessage);

				innerSpan.InnerHtml.AppendHtml(innerEm);
				EmptyValEl.InnerHtml.AppendHtml(innerSpan);
			}
			#endregion

			return isSuccess;
		}

	}
}
