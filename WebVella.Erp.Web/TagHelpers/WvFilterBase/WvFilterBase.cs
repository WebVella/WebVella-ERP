using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.TagHelpers
{
	public abstract class WvFilterBase : TagHelper
	{
		[HtmlAttributeName("is-visible")]
		public bool isVisible { get; set; } = true;

		[HtmlAttributeName("init-errors")]
		public List<string> InitErrors { get; set; } = new List<string>();

		[HtmlAttributeName("query-type")]
		public FilterType QueryType { get; set; } = FilterType.Undefined;  //will be overrided with Url Query name: q_fieldName_t

		[HtmlAttributeName("query-options")]
		public List<FilterType> QueryOptions { get; set; } = new List<FilterType>(); //if not set will be inited with default set

		[HtmlAttributeName("field-id")]
		public Guid FilterId { get; set; } = Guid.Empty;

		[HtmlAttributeName("prefix")]
		public string Prefix { get; set; } = "";

		[HtmlAttributeName("name")]
		public string Name { get; set; } = "";

		[HtmlAttributeName("label")]
		public string Label { get; set; } = "";

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		protected dynamic Value { get; set; } = null; //Url Query name: q_fieldName_v

		protected dynamic Value2 { get; set; } = null;  //Url Query name: q_fieldName_v2

		protected string UrlQueryOfType { get; set; } = "q__t";

		protected string UrlQueryOfValue { get; set; } = "q__v";

		protected string UrlQueryOfValue2 { get; set; } = "q__v2";

		protected TagBuilder FilterTypeSelect { get; set; } = null;

		protected TagBuilder ValueTextControl { get; set; } = null;

		protected TagBuilder ValueNumberControl { get; set; } = null;

		protected TagBuilder Value2NumberControl { get; set; } = null;

		protected TagBuilder AndDivider { get; set; } = null;

		public bool InitFilter(TagHelperContext context, TagHelperOutput output)
		{
			var isSuccess = true;

			#region << Init Props >>
			if (String.IsNullOrWhiteSpace(Name))
			{
				InitErrors.Add("Name attribute is required for wv-filter TagHelper!");
			}
			UrlQueryOfType = $"{Prefix}q_{Name}_t";
			UrlQueryOfValue = $"{Prefix}q_{Name}_v";
			UrlQueryOfValue2 = $"{Prefix}q_{Name}_v2";

			if (FilterId == Guid.Empty)
			{
				FilterId = Guid.NewGuid();
			}

			#endregion

			#region << Init Values >>

			#region << Preinit from URL - type,value,value2 >>
			var urlQueryDict = ViewContext.HttpContext.Request.Query;

			if (urlQueryDict.ContainsKey(UrlQueryOfType))
			{
				if (Enum.TryParse(urlQueryDict[UrlQueryOfType], out FilterType result))
				{
					QueryType = result;
				}
			}

			if (urlQueryDict.ContainsKey(UrlQueryOfValue))
			{
				Value = (string)urlQueryDict[UrlQueryOfValue];
			}

			if (urlQueryDict.ContainsKey(UrlQueryOfValue2))
			{
				Value2 = (string)urlQueryDict[UrlQueryOfValue2];
			}
			#endregion

			#region << Convert to proper type = value, value2 >>
			var tagName = context.TagName;
			dynamic valueResult = null;
			dynamic value2Result = null;
			var errorList = new List<string>();
			var error2List = new List<string>();
			var fieldType = FieldType.TextField;
			switch (context.TagName)
			{
				case "wv-filter-autonumber":
					fieldType = FieldType.AutoNumberField;
					break;
				case "wv-filter-checkbox":
					fieldType = FieldType.CheckboxField;
					break;
				case "wv-filter-currency":
					fieldType = FieldType.CurrencyField;
					break;
				case "wv-filter-date":
					fieldType = FieldType.DateField;
					break;
				case "wv-filter-datetime":
					fieldType = FieldType.DateTimeField;
					break;
				case "wv-filter-email":
					fieldType = FieldType.EmailField;
					break;
				case "wv-filter-file":
					fieldType = FieldType.FileField;
					break;
				case "wv-filter-guid":
					fieldType = FieldType.GuidField;
					break;
				case "wv-filter-html":
					fieldType = FieldType.HtmlField;
					break;
				case "wv-filter-image":
					fieldType = FieldType.ImageField;
					break;
				case "wv-filter-textarea":
					fieldType = FieldType.MultiLineTextField;
					break;
				case "wv-filter-multiselect":
					fieldType = FieldType.MultiSelectField;
					break;
				case "wv-filter-number":
					fieldType = FieldType.NumberField;
					break;
				case "wv-filter-percent":
					fieldType = FieldType.NumberField;
					break;
				case "wv-filter-phone":
					fieldType = FieldType.PhoneField;
					break;
				case "wv-filter-select":
					fieldType = FieldType.SelectField;
					break;
				case "wv-filter-url":
					fieldType = FieldType.UrlField;
					break;
				default:
					fieldType = FieldType.TextField;
					break;
			}

			DataUtils.ValidateValueToFieldType(fieldType, Value, out valueResult, out errorList);
			Value = valueResult;
			if (errorList.Count > 0)
			{
				Value = null;
			}

			DataUtils.ValidateValueToFieldType(fieldType, Value2, out value2Result, out error2List);
			Value2 = value2Result;
			if (error2List.Count > 0)
			{
				foreach (var error in error2List)
				{
					InitErrors.Add(error);
				}
			}
			if (QueryOptions == null)
				QueryOptions = new List<FilterType>();

			if (QueryOptions.Count == 0)
			{
				QueryOptions = DataUtils.GetFilterTypesForFieldType(fieldType);
			}

			if (QueryType == FilterType.Undefined)
			{
				//If options has EQ selected
				if (QueryOptions.Any(x => x == FilterType.EQ))
				{
					QueryType = FilterType.EQ;
				}
				//If not select the first in the options
				else if (QueryOptions.Any()){
					QueryType = QueryOptions.First();
				}
			}

			#endregion


			#region << Render >>
			output.TagName = "div";
			output.AddCssClass("wv-field form-group erp-filter");
			output.Attributes.Add("id", $"erp-filter-{FilterId}");
			output.Attributes.Add("data-name", $"{Name}");
			output.Attributes.Add("data-prefix", $"{Prefix}");
			output.Attributes.Add("data-filter-id", $"{FilterId}");

			var labelEl = new TagBuilder("label");
			labelEl.AddCssClass("control-label");
			if (!String.IsNullOrWhiteSpace(Label))
			{
				labelEl.InnerHtml.AppendHtml(Label);
			}
			else
			{
				labelEl.InnerHtml.AppendHtml(Name);
			}
			var clearLink = new TagBuilder("a");
			clearLink.AddCssClass("clear-filter action");
			clearLink.Attributes.Add("href", "javascript:void(0)");
			clearLink.InnerHtml.Append("clear");

			if ((Value == null  || Value.ToString() == "") && (Value2 == null || Value2.ToString() == "")) {
				clearLink.AddCssClass("d-none");
			}
			labelEl.InnerHtml.AppendHtml(clearLink);

			output.PreContent.AppendHtml(labelEl);



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

			#endregion


			#endregion

			#region << Query Type Select >>

			if (QueryOptions.Count > 1)
			{
				FilterTypeSelect = new TagBuilder("select");
				FilterTypeSelect.AddCssClass("form-control erp-filter-rule");
				FilterTypeSelect.Attributes.Add("name", $"{UrlQueryOfType}");
				foreach (var typeOption in QueryOptions)
				{
					var optionEl = new TagBuilder("option");
					optionEl.Attributes.Add("value", typeOption.ToString());
					optionEl.InnerHtml.Append(typeOption.GetLabel());
					if (QueryType == typeOption)
					{
						optionEl.Attributes.Add("selected", null);
					}
					FilterTypeSelect.InnerHtml.AppendHtml(optionEl);
				}
			}
			else
			{
				//If 1
				FilterTypeSelect = new TagBuilder("span");
				FilterTypeSelect.AddCssClass($"input-group-prepend erp-filter-rule");
				var prependText = new TagBuilder("span");
				prependText.AddCssClass("input-group-text");
				prependText.InnerHtml.AppendHtml(QueryOptions.First().GetLabel());
				FilterTypeSelect.InnerHtml.AppendHtml(prependText);

				var hiddenInput = new TagBuilder("input");
				hiddenInput.Attributes.Add("type", "hidden");
				hiddenInput.Attributes.Add("value", QueryOptions.First().ToString());
				hiddenInput.Attributes.Add("name", $"{UrlQueryOfType}");
				FilterTypeSelect.InnerHtml.AppendHtml(hiddenInput);
			}

			#endregion

			#region << ValueTextControl >>
			{
				ValueTextControl = new TagBuilder("input");
				ValueTextControl.AddCssClass("form-control value");
				if (QueryType != FilterType.BETWEEN && QueryType != FilterType.NOTBETWEEN)
				{
					ValueTextControl.AddCssClass("rounded-right");
				}
				ValueTextControl.Attributes.Add("value", (Value ?? "").ToString());
				ValueTextControl.Attributes.Add("type", "text");
				ValueTextControl.Attributes.Add("name", UrlQueryOfValue);
			}
			#endregion

			#region << ValueNumberControl >>
			{
				ValueNumberControl = new TagBuilder("input");
				ValueNumberControl.AddCssClass("form-control value");
				if (QueryType != FilterType.BETWEEN && QueryType != FilterType.NOTBETWEEN)
				{
					ValueNumberControl.AddCssClass("rounded-right");
				}
				ValueNumberControl.Attributes.Add("value", (Value ?? "").ToString());
				ValueNumberControl.Attributes.Add("type", "number");
				ValueNumberControl.Attributes.Add("name", UrlQueryOfValue);
			}
			#endregion

			#region << Value2NumberControl >>
			{
				Value2NumberControl = new TagBuilder("input");
				Value2NumberControl.Attributes.Add("value", (Value2 ?? "").ToString());
				Value2NumberControl.AddCssClass("form-control value2");
				Value2NumberControl.Attributes.Add("type", "number");
				if (QueryType == FilterType.BETWEEN || QueryType == FilterType.NOTBETWEEN)
				{
					Value2NumberControl.Attributes.Add("name", UrlQueryOfValue2);
				}
				else
				{
					Value2NumberControl.AddCssClass("d-none");
				}
			}
			#endregion

			#region << AndDivider >>
			{
				AndDivider = new TagBuilder("span");
				AndDivider.AddCssClass($"input-group-prepend input-group-append erp-filter-divider");
				if (QueryType != FilterType.BETWEEN && QueryType != FilterType.NOTBETWEEN)
				{
					AndDivider.AddCssClass("d-none");
				}
				var prependText = new TagBuilder("span");
				prependText.AddCssClass("input-group-text divider");
				prependText.InnerHtml.Append("&");
				AndDivider.InnerHtml.AppendHtml(prependText);
			}
			#endregion

			var jsCompressor = new JavaScriptCompressor();

			#region << Init Scripts >>
			var tagHelperInitialized = false;
			if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFilterBase) + "-base"))
			{
				var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFilterBase) + "-base"];
				tagHelperInitialized = tagHelperContext.Initialized;
			}
			if (!tagHelperInitialized)
			{
				var scriptContent = FileService.GetEmbeddedTextResource("base.js", "WebVella.Erp.Web.TagHelpers.WvFilterBase");
				var scriptEl = new TagBuilder("script");
				scriptEl.Attributes.Add("type", "text/javascript");
				scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
				output.PostContent.AppendHtml(scriptEl);

				ViewContext.HttpContext.Items[typeof(WvFilterBase) + "-base"] = new WvTagHelperContext()
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
							BaseFilterInit(""{{FilterId}}"",""{{Value2InputName}}"");
						});";
			scriptTemplate = scriptTemplate.Replace("{{FilterId}}", FilterId.ToString());
			scriptTemplate = scriptTemplate.Replace("{{Value2InputName}}", UrlQueryOfValue2.ToString());
			initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

			output.PostContent.AppendHtml(initScript);
			#endregion



			return isSuccess;
		}
	}
}
