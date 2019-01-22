using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using Yahoo.Yui.Compressor;
using WebVella.Erp.Web.Utils;
using WebVella.Erp.Api.Models;
using System.Linq;
using System.Globalization;

namespace WebVella.Erp.Web.TagHelpers
{
	[HtmlTargetElement("wv-page-header-actions", ParentTag = "wv-page-header")]
	public class WvPageHeaderActions : TagHelper
	{
		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			var content = await output.GetChildContentAsync();
			output.Content.AppendHtml(content);
			//return Task.CompletedTask;
		}

	}
}
