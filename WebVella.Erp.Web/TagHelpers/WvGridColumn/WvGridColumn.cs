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

		[HtmlAttributeName("is-visible")]
		public bool isVisible { get; set; } = true;

		[HtmlAttributeName("vertical-align")]
		public VerticalAlignmentType VerticalAlign { get; set; } = VerticalAlignmentType.None;

		[HtmlAttributeName("horizontal-align")]
		public HorizontalAlignmentType HorizontalAlign { get; set; } = HorizontalAlignmentType.None;

		[HtmlAttributeName("text-nowrap")]
		public bool TextNoWrap { get; set; } = false;

		[HtmlAttributeName("class")]
		public string Class { get; set; } = null;

        [HtmlAttributeName("colspan")]
        public int? Colspan { get; set; } = null;

        [HtmlAttributeName("width")]
		public string Width { get; set; } = "";

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (!isVisible)
			{
				output.SuppressOutput();
				return Task.CompletedTask;
			}

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

			if(TextNoWrap){
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

			if (!String.IsNullOrEmpty(Width))
			{
				styleList.Add($"width:{Width}");
			}

			if (styleList.Count > 0) {
				output.Attributes.Add("style", String.Join("; ",styleList));
			}

            if (Colspan != null) {
                output.Attributes.Add("colspan", Colspan);
            }

			#endregion

			return Task.CompletedTask;
		}
	}
}
