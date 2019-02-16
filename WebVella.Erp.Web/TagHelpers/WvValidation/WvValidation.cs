using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;
using WebVella.Erp.Exceptions;


namespace WebVella.Erp.Web.TagHelpers
{

	[HtmlTargetElement("wv-validation")]
	public class WvValidation : TagHelper
	{
		[HtmlAttributeName("is-visible")]
		public bool isVisible { get; set; } = true;

		[HtmlAttributeName("validation")]
		public ValidationException Validation { get; set; } = new ValidationException();

		[HtmlAttributeName("show-errors")]
		public bool ShowErrors { get; set; } = true;

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (!isVisible)
			{
				output.SuppressOutput();
				return Task.CompletedTask;
			}
			if (Validation.Errors.Count == 0 && String.IsNullOrWhiteSpace(Validation.Message))
			{
				output.SuppressOutput();
				return Task.CompletedTask;
			}
			output.TagName = "div";
			output.AddCssClass("alert alert-danger p-2");

			if (!ShowErrors && String.IsNullOrWhiteSpace(Validation.Message))
			{
				Validation.Message = "Validation Error";
			}

			if (!ShowErrors || Validation.Errors.Count == 0)
			{
				output.Content.AppendHtml(Validation.Message);
			}
			else
			{
				if (!String.IsNullOrWhiteSpace(Validation.Message))
				{
					var titleEl = new TagBuilder("h3");
					titleEl.AddCssClass("title");
					titleEl.InnerHtml.AppendHtml(Validation.Message);
					output.Content.AppendHtml(titleEl);
				}

				var ulEl = new TagBuilder("ul");
				foreach (var error in Validation.Errors)
				{
					var liEl = new TagBuilder("li");
					if (String.IsNullOrWhiteSpace(error.PropertyName))
					{
						liEl.InnerHtml.AppendHtml(error.Message);
					}
					else
					{
						var keyEl = new TagBuilder("strong");
						keyEl.InnerHtml.AppendHtml(error.PropertyName);
						liEl.InnerHtml.AppendHtml(keyEl);
						liEl.InnerHtml.AppendHtml($" - {error.Message}");
					}
					ulEl.InnerHtml.AppendHtml(liEl);
				}

				output.Content.AppendHtml(ulEl);
			}

			return Task.CompletedTask;
		}
	}
}
