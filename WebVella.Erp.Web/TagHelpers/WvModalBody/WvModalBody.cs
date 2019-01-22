using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.TagHelpers
{

	//[OutputElementHint("div")]
	[HtmlTargetElement("wv-modal-body")]
	public class WvModalBody : TagHelper
	{

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{

			output.TagName = "div";
			output.AddCssClass("modal-body");

			return Task.CompletedTask;
		}
	}
}
