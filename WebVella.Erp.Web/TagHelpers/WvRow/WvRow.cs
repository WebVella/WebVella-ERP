using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.TagHelpers
{

	//[OutputElementHint("div")]
	[HtmlTargetElement("wv-row")]
	public class WvRow : TagHelper
	{

		[HtmlAttributeName("flex-vertical-alignment")]
		public FlexVerticalAlignmentType FlexVerticalAlignment { get; set; } = FlexVerticalAlignmentType.None;

		[HtmlAttributeName("flex-horizontal-alignment")]
		public FlexHorizontalAlignmentType FlexHorizontalAlignment { get; set; } = FlexHorizontalAlignmentType.None;

		[HtmlAttributeName("no-gutters")]
		public bool NoGutters { get; set; } = false;

		[HtmlAttributeName("class")]
		public string Class { get; set; } = "";


		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{

			output.TagName = "";
			//output.TagName = "div";
			//var wrapperClassList = new List<string>();
			//wrapperClassList.Add("container-fluid");
			//output.Attributes.SetAttribute("class", String.Join(" ", wrapperClassList));



			//Inner wrapper
			var rowEl = new TagBuilder("div");
			rowEl.AddCssClass("lnr row");
			if (!String.IsNullOrWhiteSpace(Class)) {
				rowEl.AddCssClass(Class);
			}

			if (FlexVerticalAlignment != FlexVerticalAlignmentType.None)
			{
				switch (FlexVerticalAlignment)
				{
					case FlexVerticalAlignmentType.Start:
						rowEl.AddCssClass("align-items-start");
						break;
					case FlexVerticalAlignmentType.Center:
						rowEl.AddCssClass("align-items-center");
						break;
					case FlexVerticalAlignmentType.End:
						rowEl.AddCssClass("align-items-end");
						break;
					default:
						break;
				}

			}
			if (FlexHorizontalAlignment != FlexHorizontalAlignmentType.None)
			{
				switch (FlexHorizontalAlignment)
				{
					case FlexHorizontalAlignmentType.Start:
						rowEl.AddCssClass("justify-content-start");
						break;
					case FlexHorizontalAlignmentType.Center:
						rowEl.AddCssClass("justify-content-center");
						break;
					case FlexHorizontalAlignmentType.End:
						rowEl.AddCssClass("justify-content-end");
						break;
					case FlexHorizontalAlignmentType.Between:
						rowEl.AddCssClass("justify-content-between");
						break;
					case FlexHorizontalAlignmentType.Around:
						rowEl.AddCssClass($"justify-content-around");
						break;
					default:
						break;
				}
			}
			if (NoGutters)
			{
				rowEl.AddCssClass("no-gutters");
			}
			
			output.PreContent.AppendHtml(rowEl.RenderStartTag());

			output.PostContent.AppendHtml(rowEl.RenderEndTag());

			return Task.CompletedTask;
		}
	}
}
