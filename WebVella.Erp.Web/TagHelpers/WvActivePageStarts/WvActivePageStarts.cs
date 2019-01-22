using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebVella.Erp.Web.TagHelpers
{
    [HtmlTargetElement(Attributes = "wv-active-page-starts")]
    public class WvActivePageStarts : TagHelper
    {
		[HtmlAttributeName("href")]
		public string Href { get; set; }

		[HtmlAttributeName("asp-page")]
		public string AspPage { get; set; }

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

			output.Attributes.RemoveAll("wv-active-page-starts");
			if (String.IsNullOrWhiteSpace(AspPage) && String.IsNullOrWhiteSpace(Href))
            {
				return Task.CompletedTask;
            }

            if (ShouldBeActive())
            {
                MakeActive(output);
            }

			if (!String.IsNullOrWhiteSpace(Href)) {
				output.Attributes.SetAttribute("href", Href);
			}
			return Task.CompletedTask;
		}

        private bool ShouldBeActive()
        {
			if (!String.IsNullOrWhiteSpace(AspPage)){
				string currentPage = ViewContext.RouteData.Values["page"].ToString().ToLowerInvariant();
				//AspPage should be processed and if it ends on index this should be removed
				AspPage = AspPage.Trim().ToLowerInvariant();
				//if (AspPage.EndsWith("/index")) {
				//    AspPage = AspPage.Substring(0, AspPage.Length - 6);
				//}
				var pathNodeList = AspPage.Split("/").ToList();
				pathNodeList.RemoveAt(pathNodeList.Count - 1);
				AspPage = String.Join("/", pathNodeList.ToArray());
				var regexPattern = "^" + AspPage;
				if (IsRegexPatternValid(regexPattern))
				{
					Regex rgx = new Regex(regexPattern);

					if (rgx.IsMatch(currentPage))
					{
						return true;
					}
				}
			}
			else if (!String.IsNullOrWhiteSpace(Href)) {
				string currentUrl = ViewContext.HttpContext.Request.Path.ToString().ToLowerInvariant();
				var regexPattern = "^" + Href;
				if (IsRegexPatternValid(regexPattern))
				{
					Regex rgx = new Regex(regexPattern);

					if (rgx.IsMatch(currentUrl))
					{
						return true;
					}
				}
			}
			return false;
		}

        private void MakeActive(TagHelperOutput output)
        {
            var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");
            output.Attributes.SetAttribute("class", classAttr.Value == null
                ? "active"
                : classAttr.Value.ToString() + " active");
        }

        private static bool IsRegexPatternValid(String pattern)
        {
            try
            {
                new Regex(pattern);
                return true;
            }
            catch { }
            return false;
        }
    }
}
