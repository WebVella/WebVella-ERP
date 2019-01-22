using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace WebVella.Erp.Web.TagHelpers
{
	[HtmlTargetElement("time", Attributes = "asp-date-time")]
	public class TimeTagHelper : TagHelper
	{
		[HtmlAttributeName("asp-date-time")]
		public DateTime DateTime { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.Attributes.RemoveAll("title");
			output.Attributes.SetAttribute(new TagHelperAttribute("title", DateTime.ToString("dddd, MMMM d, yyyy 'at' h:mm tt")));
		}
	}

}
