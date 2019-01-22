using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.TagHelpers
{

	//[OutputElementHint("div")]
	public class WvColumn : TagHelper
	{

		[HtmlAttributeName("span")]
		public int? Span { get; set; } = 0; // 0 will render "col", null will not render the class,-1 will render "col-auto"

		[HtmlAttributeName("span-sm")]
		public int? SpanSm { get; set; } = null; // 0 will render "col-sm", null will not render the class,-1 will render "col-auto"

		[HtmlAttributeName("span-md")]
		public int? SpanMd { get; set; } = null; // 0 will render "col-md", null will not render the class,-1 will render "col-auto"

		[HtmlAttributeName("span-lg")]
		public int? SpanLg { get; set; } = null; // 0 will render "col-lg", null will not render the class,-1 will render "col-auto"

		[HtmlAttributeName("span-xl")]
		public int? SpanXl { get; set; } = null; // 0 will render "col-xl", null will not render the class,-1 will render "col-auto"

		[HtmlAttributeName("offset")]
		public int? Offset { get; set; } = null; // null will render the class

		[HtmlAttributeName("offset-sm")]
		public int? OffsetSm { get; set; } = null; // 0 or null will render no device specific span

		[HtmlAttributeName("offset-md")]
		public int? OffsetMd { get; set; } = null; // 0 or null will render no device specific span

		[HtmlAttributeName("offset-lg")]
		public int? OffsetLg { get; set; } = null; // 0 or null will render no device specific span

		[HtmlAttributeName("offset-xl")]
		public int? OffsetXl { get; set; } = null; // 0 or null will render no device specific span

		[HtmlAttributeName("flex-self-align")]
		public FlexSelfAlignType FlexSelftAlign { get; set; } = FlexSelfAlignType.None;

		[HtmlAttributeName("flex-order")]
		public int? FlexOrder { get; set; } = null; // will not render the class

		[HtmlAttributeName("class")]
		public string Class { get; set; } = "";

		[HtmlAttributeName("id")]
		public string Id { get; set; } = "";

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "div";
			var classList = new List<string>();

			#region << Set the CSS class >>
			classList.Add("lnc");
			var spanSet = false;
			if (Span != null && Span != 0)
			{
				classList.Add(Span == -1 ? "col-auto" : $"col-{Span}");
				spanSet = true;
			}

			if (SpanSm != null && SpanSm != 0)
			{
				classList.Add(SpanSm == -1 ? "col-sm-auto" : $"col-sm-{SpanSm}");
				spanSet = true;
			}

			if (SpanMd != null && SpanMd != 0)
			{
				classList.Add(SpanMd == -1 ? "col-md-auto" : $"col-md-{SpanMd}");
				spanSet = true;
			}

			if (SpanLg != null && SpanLg != 0)
			{
				classList.Add(SpanLg == -1 ? "col-lg-auto" : $"col-lg-{SpanLg}");
				spanSet = true;
			}

			if (SpanXl != null && SpanXl != 0)
			{
				classList.Add(SpanXl == -1 ? "col-xl-auto" : $"col-xl-{SpanXl}");
				spanSet = true;
			}

			if (!spanSet) {
				classList.Add($"col");
			}

			if (Offset != null && Offset != 0)
			{
				classList.Add($"offset-{Offset}");
			}

			if (OffsetSm != null && OffsetSm != 0)
			{
				classList.Add($"offset-sm-{OffsetSm}");
			}

			if (OffsetMd != null && OffsetMd != 0)
			{
				classList.Add($"offset-md-{OffsetMd}");
			}

			if (OffsetLg != null && OffsetLg != 0)
			{
				classList.Add($"offset-lg-{OffsetLg}");
			}

			if (OffsetXl != null && OffsetXl != 0)
			{
				classList.Add($"offset-xl-{OffsetXl}");
			}

			if (FlexSelftAlign != FlexSelfAlignType.None)
			{
				switch (FlexSelftAlign)
				{
					case FlexSelfAlignType.Start:
						classList.Add($"align-self-start");
						break;
					case FlexSelfAlignType.Center:
						classList.Add($"align-self-center");
						break;
					case FlexSelfAlignType.End:
						classList.Add($"align-self-end");
						break;
					default:
						break;
				}
			}

			if (FlexOrder != null)
			{
				classList.Add($"order-{FlexOrder}");
			}

			#endregion
			if (!String.IsNullOrWhiteSpace(Class))
			{
				classList.Add(Class);
			}
			output.Attributes.SetAttribute("class", String.Join(" ", classList));

			if (!String.IsNullOrWhiteSpace(Id))
			{
				output.Attributes.SetAttribute("id", Id);
			}

			return Task.CompletedTask;
		}
	}
}
