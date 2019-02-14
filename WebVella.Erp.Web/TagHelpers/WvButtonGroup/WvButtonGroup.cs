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
	[HtmlTargetElement("wv-button-group")]
	public class WvButtonGroup : TagHelper
	{
		public WvButtonGroup(IHtmlGenerator generator)
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
		public CssSize Size { get; set; } = CssSize.Inherit;

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

			#region << Init >>
			if (Size == CssSize.Inherit) {
				if (context.Items.ContainsKey(typeof(CssSize)))
				{
					Size = (CssSize)context.Items[typeof(CssSize)];
				}
				else
				{
					Size = CssSize.Small;
				}
			}
			if (context.Items.ContainsKey("IsVertical"))
			{
				IsVertical = (bool)context.Items["IsVertical"];
			}
			#endregion


			var classList = new List<string>();

			output.TagName = "div";
			if (IsVertical)
			{
				classList.Add("btn-group-vertical");
			}
			else
			{
				classList.Add("btn-group");
			}

			#region << Size >>
			if (Size == CssSize.Small)
			{
				classList.Add("btn-group-sm");
			}
			else if (Size == CssSize.Large) {
				classList.Add("btn-group-lg");
			}
			#endregion

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
			return Task.CompletedTask;
		}
	}
}
