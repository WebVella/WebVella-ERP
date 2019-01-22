using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebVella.Erp.Web.TagHelpers
{
    [HtmlTargetElement(Attributes = "wv-active-page-equals")]
    public class WvActivePageEquals : TagHelper
    {
		[HtmlAttributeName("asp-page")]
		public string AspPage { get; set; }

		[HtmlAttributeName("href")]
		public string Href { get; set; }

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
			output.Attributes.RemoveAll("wv-active-page-equals");

			if (String.IsNullOrWhiteSpace(AspPage) && String.IsNullOrWhiteSpace(Href))
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
			if (!String.IsNullOrWhiteSpace(AspPage))
			{
				string currentPage = ViewContext.RouteData.Values["page"].ToString().ToLowerInvariant();
				AspPage = AspPage.Trim().ToLowerInvariant();
				var regexPattern = "^" + AspPage + "$";
				if (IsRegexPatternValid(regexPattern))
				{
					Regex rgx = new Regex(regexPattern);

					if (rgx.IsMatch(currentPage))
					{
						return true;
					}
				}
			}
			else if (!String.IsNullOrWhiteSpace(Href))
			{
				string currentUrl = ViewContext.HttpContext.Request.Path.ToString().ToLowerInvariant();
				if (currentUrl == Href) {
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
