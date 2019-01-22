using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.TagHelpers
{
	[HtmlTargetElement("wv-grid-column", ParentTag = "wv-grid-row")]
	public class WvGridColumn : TagHelper
	{
		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		[HtmlAttributeName("vertical-align")]
		public VerticalAlignmentType VerticalAlign { get; set; } = VerticalAlignmentType.None;

		[HtmlAttributeName("horizontal-align")]
		public HorizontalAlignmentType HorizontalAlign { get; set; } = HorizontalAlignmentType.None;

		[HtmlAttributeName("text-wrap")]
		public bool TextWrap { get; set; } = true;

		[HtmlAttributeName("class")]
		public string Class { get; set; } = null;

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{

			#region << Init >>
			if (VerticalAlign == VerticalAlignmentType.None && context.Items.ContainsKey(typeof(VerticalAlignmentType))) {
				VerticalAlign = (VerticalAlignmentType)context.Items[typeof(VerticalAlignmentType)];
			}

			#endregion

			#region << Render >>

			output.TagName = "td";

			if (!String.IsNullOrWhiteSpace(Class)) {
				output.AddCssClass(Class);
			}

			var styleList = new List<string>();
			switch (VerticalAlign) {
				case VerticalAlignmentType.Top:
					styleList.Add("vertical-align:top");
					break;
				case VerticalAlignmentType.Middle:
					styleList.Add("vertical-align:middle");
					break;
				case VerticalAlignmentType.Bottom:
					styleList.Add("vertical-align:bottom");
					break;
				default:
					break;
			}

			if(TextWrap){
				styleList.Add("white-space:nowrap");
			}

			switch (HorizontalAlign)
			{
				case HorizontalAlignmentType.Left:
					styleList.Add("text-align:left");
					break;
				case HorizontalAlignmentType.Center:
					styleList.Add("text-align:center");
					break;
				case HorizontalAlignmentType.Right:
					styleList.Add("text-align:right");
					break;
				default:
					break;
			}

			if (styleList.Count > 0) {
				output.Attributes.Add("style", String.Join("; ",styleList));
			}

			#endregion

			return Task.CompletedTask;
		}
	}
}
