using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebVella.Erp.Web.TagHelpers
{
    [HtmlTargetElement(Attributes = "wv-active-page-regex")]
    public class WvActivePageRegex : TagHelper
    {

		[HtmlAttributeName("wv-active-page-regex")]
		public string RegexPattern { get; set; }

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		[HtmlAttributeName("href")]
		public string Href { get; set; }

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
			output.Attributes.RemoveAll("wv-active-page-regex");

			if (!IsRegexPatternValid(RegexPattern) && String.IsNullOrWhiteSpace(Href))
            {
				//output.SuppressOutput();
				return Task.CompletedTask;
            }

            if (ShouldBeActive())
            {
                MakeActive(output);
            }

			if (!String.IsNullOrWhiteSpace(Href))
			{
				output.Attributes.SetAttribute("href", Href);
			}

			return Task.CompletedTask;
		}

		private bool ShouldBeActive()
        {
            string currentPage = ViewContext.RouteData.Values["page"].ToString().ToLowerInvariant();

			if (!String.IsNullOrWhiteSpace(currentPage))
			{
				Regex rgx = new Regex(RegexPattern);

				if (rgx.IsMatch(currentPage))
				{
					return true;
				}
			}
			else {
				string currentUrl = ViewContext.HttpContext.Request.Path.ToString().ToLowerInvariant();
				Regex rgx = new Regex(RegexPattern);

				if (rgx.IsMatch(currentUrl))
				{
					return true;
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
