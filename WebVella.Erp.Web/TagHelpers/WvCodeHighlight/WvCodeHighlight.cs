using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace WebVella.Erp.Web.TagHelpers
{
    [HtmlTargetElement(Attributes = "wv-code-highlight,wv-code-string")]
	public class WvCodeHighlight : TagHelper
    {

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

		[HtmlAttributeName("is-visible")]
		public bool isVisible { get; set; } = true;

		[HtmlAttributeName("wv-code-highlight")]
		public string HighlightLanguage { get; set; } = "language-html";

		[HtmlAttributeName("wv-code-string")]
		public string CodeString { get; set; } = "<span>code string not set</span>";

		private class WvCodeHighlightContext
		{
			public bool Initialized { get; set; } = false;
		}

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
			if (!isVisible)
			{
				output.SuppressOutput();
				return Task.CompletedTask;
			}

			var prependTemplate = $@"<code class='{HighlightLanguage}'>";

			var appendTemplate = $@"</code>";

			output.TagName = null;
			var preEl = new TagBuilder("pre");
			preEl.InnerHtml.AppendHtml(prependTemplate);
			preEl.InnerHtml.AppendHtml(CodeString);
			preEl.InnerHtml.AppendHtml(appendTemplate);
			output.PreContent.AppendHtml(preEl);


			var moduleAdded = false;
			if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvCodeHighlight)))
			{
				var tagHelperContext = (WvCodeHighlightContext)ViewContext.HttpContext.Items[typeof(WvCodeHighlight)];
				moduleAdded = tagHelperContext.Initialized;
			}
			if (!moduleAdded)
			{
				var scriptEl = new TagBuilder("script");
				scriptEl.Attributes.Add("src", "/js/prism/prism.js");
				scriptEl.Attributes.Add("type", "text/javascript");
				output.PostContent.AppendHtml(scriptEl);

				var linkEl = new TagBuilder("link");
				linkEl.Attributes.Add("href", "/js/prism/prism.css");
				linkEl.Attributes.Add("rel", "stylesheet");
				linkEl.Attributes.Add("type", "text/css");
				output.PostContent.AppendHtml(linkEl);

				ViewContext.HttpContext.Items[typeof(WvCodeHighlight)] = new WvCodeHighlightContext()
				{
					Initialized = true
				};
			}



			return Task.CompletedTask;

        }


    }


}
