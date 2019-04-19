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
	[HtmlTargetElement("wv-button")]
	public class WvButton : TagHelper
	{
		public WvButton(IHtmlGenerator generator)
		{
			Generator = generator;
		}

		protected IHtmlGenerator Generator { get; }

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		[HtmlAttributeName("is-visible")]
		public bool isVisible { get; set; } = true;

		[HtmlAttributeName("type")]
		public ButtonType Type { get; set; } = ButtonType.Button;

		[HtmlAttributeName("text")]
		public string Text { get; set; } = "";

		[HtmlAttributeName("color")]
		public ErpColor Color { get; set; } = ErpColor.White;

		[HtmlAttributeName("size")]
		public CssSize Size { get; set; } = CssSize.Inherit;


		[HtmlAttributeName("class")]
		public string Class { get; set; } = "";


		[HtmlAttributeName("id")]
		public string Id { get; set; } = "";

		[HtmlAttributeName("is-outline")]
		public bool isOutline { get; set; } = false;


		[HtmlAttributeName("icon-class")]
		public string IconClass { get; set; } = "";

        [HtmlAttributeName("icon-right")]
        public bool IconRight { get; set; } = false;

        [HtmlAttributeName("is-block")]
		public bool isBlock { get; set; } = false;



		[HtmlAttributeName("is-active")]
		public bool isActive { get; set; } = false;

		[HtmlAttributeName("is-disabled")]
		public bool isDisabled { get; set; } = false;



		[HtmlAttributeName("onclick")]
		public string OnClick { get; set; } = "";

		[HtmlAttributeName("href")]
		public string Href { get; set; } = "";

		[HtmlAttributeName("new-tab")]
		public bool NewTab { get; set; } = false;

		[HtmlAttributeName("form")]
		public string Form { get; set; } = "";

		[HtmlAttributeName("formaction")]
		public string FormAction { get; set; } = "";


		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (!isVisible) {
				output.SuppressOutput();
				return Task.CompletedTask;
			}
				

			#region << Init >>
			if (Size == CssSize.Inherit)
			{
				if (context.Items.ContainsKey(typeof(CssSize)))
				{
					Size = (CssSize)context.Items[typeof(CssSize)];
				}
				else
				{
					Size = CssSize.Small;
				}
			}
			#endregion

			var classList = new List<string>();

			#region << TagName, type and main class >>
			if (Type == ButtonType.LinkAsButton || Type == ButtonType.ButtonLink)
			{
				output.TagName = "a";
			}
			else if (Type == ButtonType.Submit){
				output.TagName = "button";
				output.Attributes.SetAttribute("type","submit");
			}
			else
			{
				output.TagName = "button";
				output.Attributes.SetAttribute("type", "button");
			}
			classList.Add("btn");
			#endregion

			#region << Color and Outline >>
			if (Type != ButtonType.ButtonLink)
			{
				if (Color != ErpColor.None)
				{
					if (isOutline)
					{
						classList.Add("btn-outline-" + Color.GetLabel());
					}
					else
					{
						classList.Add("btn-" + Color.GetLabel());
					}
				}
			}
			else {
				classList.Add("btn-link");
			}
			#endregion

			#region << Active >>
			if (isActive) {
				classList.Add("active");
			}
			#endregion

			#region << Disabled >>
			if (isDisabled && Type == ButtonType.LinkAsButton)
			{
				classList.Add("disabled");
			}
			else if (isDisabled) {
				output.Attributes.SetAttribute("disabled",null);
			}
			#endregion

			#region << Form & FormAction >>
			if (Type == ButtonType.Submit && !String.IsNullOrWhiteSpace(Form))
			{
				output.Attributes.SetAttribute("form", Form);
				if (!String.IsNullOrWhiteSpace(FormAction)){
					output.Attributes.SetAttribute("formaction", FormAction);
				}
			}
			#endregion

			#region << Href & NewTab >>
			if (Type == ButtonType.LinkAsButton || Type == ButtonType.ButtonLink) {
				if (String.IsNullOrWhiteSpace(Href))
				{
					output.Attributes.SetAttribute("href", "#");
				}
				else {
					output.Attributes.SetAttribute("href", Href);
				}
				if (NewTab)
				{
					output.Attributes.SetAttribute("target", "_blank");
				}
			}
			#endregion

			#region << Onclick >>
			if (!String.IsNullOrWhiteSpace(OnClick))
			{
				output.Attributes.SetAttribute("onclick", OnClick);
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

			#region << Size >>
			if (Size == CssSize.Small)
			{
				classList.Add("btn-sm");
			}
			else if (Size == CssSize.Large) {
				classList.Add("btn-lg");
			}
			#endregion

			#region << isBlock >>
			if (isBlock) {
				classList.Add("btn-block");
			}
			#endregion

			#region << Left Icon >>
			if (!String.IsNullOrWhiteSpace(IconClass))
			{
				var iconEl = new TagBuilder("i");
				iconEl.AddCssClass(IconClass);
				iconEl.AddCssClass("icon");
                if (!IconRight)
                    output.PreContent.AppendHtml(iconEl);
                else
                {
                    iconEl.AddCssClass("right");
                    output.PostContent.AppendHtml(iconEl);
                }
            }
			#endregion

			#region << Text and BtnIcon >>
			if (!String.IsNullOrWhiteSpace(Text))
			{
				output.Content.AppendHtml(Text);
			}
			else {
				classList.Add("btn-icon");
			}
            #endregion

            output.Attributes.SetAttribute("class", String.Join(" ", classList));
			return Task.CompletedTask;
		}
	}
}
