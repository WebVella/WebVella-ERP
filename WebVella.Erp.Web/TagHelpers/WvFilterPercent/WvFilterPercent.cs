using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.TagHelpers.Models;
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.TagHelpers
{
	[HtmlTargetElement("wv-filter-percent")]
	public class WvFilterPercent : WvFilterBase
	{
		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (!isVisible)
			{
				output.SuppressOutput();
				return Task.CompletedTask;
			}
			#region << Init >>
			var initSuccess = InitFilter(context, output);

			if (!initSuccess)
			{
				return Task.CompletedTask;
			}

			var inputGroupEl = new TagBuilder("div");
			inputGroupEl.AddCssClass("input-group");

			inputGroupEl.InnerHtml.AppendHtml(FilterTypeSelect);

			#region << Value Control >>
			{
				var fakeValueString = "";
				if (Value != null)
				{
					fakeValueString = (((decimal)Value) * 100).ToString();
				}
				var valueFakeInputControl = new TagBuilder("input");
				valueFakeInputControl.Attributes.Add("id", $"erp-filter-fake-value-{FilterId}");
				valueFakeInputControl.AddCssClass("form-control fake-value");
				if (QueryType != FilterType.BETWEEN && QueryType != FilterType.NOTBETWEEN)
				{
					valueFakeInputControl.AddCssClass("rounded-right");
				}
				valueFakeInputControl.Attributes.Add("value", fakeValueString);
				valueFakeInputControl.Attributes.Add("type", "number");
				inputGroupEl.InnerHtml.AppendHtml(valueFakeInputControl);

				var valueHiddenInputControl = new TagBuilder("input");
				valueHiddenInputControl.AddCssClass("form-control value");
				valueHiddenInputControl.Attributes.Add("value", (Value ?? "").ToString());
				valueHiddenInputControl.Attributes.Add("id", $"erp-filter-input-value-{FilterId}");
				valueHiddenInputControl.Attributes.Add("type", "hidden");
				valueHiddenInputControl.Attributes.Add("name", UrlQueryOfValue);
				inputGroupEl.InnerHtml.AppendHtml(valueHiddenInputControl);

			}
			#endregion

			inputGroupEl.InnerHtml.AppendHtml(AndDivider);

			#region << Value2 Control >>
			{
				var fakeValue2String = "";
				if (Value2 != null)
				{
					fakeValue2String = (((decimal)Value) * 100).ToString();
				}
				var value2FakeInputControl = new TagBuilder("input");
				value2FakeInputControl.Attributes.Add("value", fakeValue2String);
				value2FakeInputControl.Attributes.Add("id", $"erp-filter-fake-value2-{FilterId}");
				value2FakeInputControl.AddCssClass("form-control fake-value2");
				value2FakeInputControl.Attributes.Add("type", "number");
				if (QueryType != FilterType.BETWEEN && QueryType != FilterType.NOTBETWEEN)
				{
					value2FakeInputControl.AddCssClass("d-none");
				}
				inputGroupEl.InnerHtml.AppendHtml(value2FakeInputControl);

				var value2HiddenInputControl = new TagBuilder("input");
				value2HiddenInputControl.AddCssClass("form-control value2");
				value2HiddenInputControl.Attributes.Add("value", (Value2 ?? "").ToString());
				value2HiddenInputControl.Attributes.Add("id", $"erp-filter-input-value2-{FilterId}");
				value2HiddenInputControl.Attributes.Add("type", "hidden");
				if (QueryType == FilterType.BETWEEN || QueryType == FilterType.NOTBETWEEN)
				{
					value2HiddenInputControl.Attributes.Add("name", UrlQueryOfValue2);
				}
				inputGroupEl.InnerHtml.AppendHtml(value2HiddenInputControl);
			}
			#endregion

			output.Content.AppendHtml(inputGroupEl);

			var jsCompressor = new JavaScriptCompressor();

			#region << Init Scripts >>
			var tagHelperInitialized = false;
			if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvFilterPercent) + "-percent"))
			{
				var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvFilterPercent) + "-percent"];
				tagHelperInitialized = tagHelperContext.Initialized;
			}
			if (!tagHelperInitialized)
			{
				var scriptContent = FileService.GetEmbeddedTextResource("percent.js", "WebVella.Erp.Web.TagHelpers.WvFilterPercent");
				var scriptEl = new TagBuilder("script");
				scriptEl.Attributes.Add("type", "text/javascript");
				scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
				output.PostContent.AppendHtml(scriptEl);

				ViewContext.HttpContext.Items[typeof(WvFilterPercent) + "-percent"] = new WvTagHelperContext()
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
							PercentFilterInit(""{{FilterId}}"");
						});";
			scriptTemplate = scriptTemplate.Replace("{{FilterId}}", FilterId.ToString());

			initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

			output.PostContent.AppendHtml(initScript);
			#endregion

			return Task.CompletedTask;
		#endregion
		}


	}
}
