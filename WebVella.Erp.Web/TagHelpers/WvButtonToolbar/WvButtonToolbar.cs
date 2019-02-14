using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web.TagHelpers
{
	[HtmlTargetElement("wv-button-toolbar")]
	public class WvButtonToolbar : TagHelper
	{
		public WvButtonToolbar(IHtmlGenerator generator)
		{
			Generator = generator;
		}

		protected IHtmlGenerator Generator { get; }

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		[HtmlAttributeName("is-visible")]
		public bool isVisible { get; set; } = true;

		[HtmlAttributeName("size")]
		public CssSize Size { get; set; } = CssSize.Small;

		[HtmlAttributeName("is-vertical")]
		public bool IsVertical { get; set; } = false;

		[HtmlAttributeName("class")]
		public string Class { get; set; } = "";

		[HtmlAttributeName("id")]
		public string Id { get; set; } = "";

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (!isVisible)
			{
				output.SuppressOutput();
				return Task.CompletedTask;
			}

			var classList = new List<string>();

			output.TagName = "div";
			classList.Add("btn-toolbar");

			#region << Id >>
			if (!String.IsNullOrWhiteSpace(Id))
			{
				output.Attributes.SetAttribute("id", Id);
			}
			#endregion

			#region << Class >>
			if (!String.IsNullOrWhiteSpace(Class))
			{
				classList.Add(Class);
			}
			#endregion

			output.Attributes.SetAttribute("class", String.Join(" ", classList));

			context.Items[typeof(CssSize)] = Size;
			context.Items["IsVertical"] = IsVertical;
			return Task.CompletedTask;
		}
	}
}
