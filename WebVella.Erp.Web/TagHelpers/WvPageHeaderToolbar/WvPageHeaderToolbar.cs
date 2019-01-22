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
	[HtmlTargetElement("wv-page-header-toolbar", ParentTag = "wv-page-header")]
	public class WvPageHeaderToolbar : TagHelper
	{
		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			return Task.CompletedTask;
		}

	}
}
