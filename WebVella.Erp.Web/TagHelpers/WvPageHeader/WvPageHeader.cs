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
using HtmlAgilityPack;

namespace WebVella.Erp.Web.TagHelpers
{
	[HtmlTargetElement("wv-page-header")]
	[RestrictChildren("wv-page-header-actions", "wv-page-header-toolbar")]
	public class WvPageHeader : TagHelper
	{
		[HtmlAttributeName("color")]
		public string Color { get; set; } = "";

		[HtmlAttributeName("icon-color")]
		public string IconColor { get; set; } = "";

		[HtmlAttributeName("area-label")]
		public string AreaLabel { get; set; } = "";

		[HtmlAttributeName("area-sublabel")]
		public string AreaSubLabel { get; set; } = "";

		[HtmlAttributeName("title")]
		public string Title { get; set; } = "";

		[HtmlAttributeName("subtitle")]
		public string SubTitle { get; set; } = "";

		[HtmlAttributeName("description")]
		public string Description { get; set; } = "";

		[HtmlAttributeName("icon-class")]
		public string IconClass { get; set; } = "";

		[HtmlAttributeName("return-url")]
		public string ReturnUrl { get; set; } = "";

		[HtmlAttributeName("page-switch-items")]
		public List<PageSwitchItem> PageSwitchItems { get; set; } = new List<PageSwitchItem>();

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{

			var content = await output.GetChildContentAsync();
			var htmlDoc = new HtmlDocument();
			htmlDoc.LoadHtml(content.GetContent());

			var actionsContentHtml = "";
			var toolbarContentHtml = "";
			var actionTagHelperList = htmlDoc.DocumentNode.Descendants("wv-page-header-actions");
			var toolbarTagHelperList = htmlDoc.DocumentNode.Descendants("wv-page-header-toolbar");

			foreach (var node in actionTagHelperList)
			{
				actionsContentHtml += node.InnerHtml.ToString();
			}

			foreach (var node in toolbarTagHelperList)
			{
				toolbarContentHtml += node.InnerHtml.ToString();
			}


			output.TagName = "div";
			output.AddCssClass($"pc-page-header {(!String.IsNullOrWhiteSpace(ReturnUrl) ? "has-btn-back" : "")} {(String.IsNullOrWhiteSpace(IconClass) ? "no-icon" : "")}");

			if (!String.IsNullOrWhiteSpace(ReturnUrl))
			{
				var backBtnEl = new TagBuilder("a");
				backBtnEl.AddCssClass("btn btn-sm btn-white btn-back");
				backBtnEl.Attributes.Add("href", ReturnUrl);
				var backBtnIconEl = new TagBuilder("span");
				backBtnIconEl.AddCssClass("ti-arrow-left");
				backBtnEl.InnerHtml.AppendHtml(backBtnIconEl);
				output.Content.AppendHtml(backBtnEl);
			}

			var rowEl = new TagBuilder("div");
			rowEl.AddCssClass("row m-0 no-gutters");

			var metaColEl = new TagBuilder("div");
			metaColEl.AddCssClass("col-md");
			var metaLabelAuxEl = new TagBuilder("div");
			metaLabelAuxEl.AddCssClass("meta-label-aux");
			if (!String.IsNullOrWhiteSpace(Color)) {
				metaLabelAuxEl.Attributes.Add("style", $"color:{Color};");
			}

			var metaLabelAuxTextEl = new TagBuilder("span");
			metaLabelAuxTextEl.AddCssClass("text");
			metaLabelAuxTextEl.InnerHtml.AppendHtml(AreaLabel);
			metaLabelAuxEl.InnerHtml.AppendHtml(metaLabelAuxTextEl);

			if (!String.IsNullOrWhiteSpace(AreaSubLabel)) {
				var metaSubLabelDividerAuxTextEl = new TagBuilder("span");
				metaSubLabelDividerAuxTextEl.AddCssClass("divider");
				metaSubLabelDividerAuxTextEl.InnerHtml.AppendHtml("/");
				metaLabelAuxEl.InnerHtml.AppendHtml(metaSubLabelDividerAuxTextEl);

				var metaSubLabelAuxTextEl = new TagBuilder("span");
				metaSubLabelAuxTextEl.AddCssClass("text");
				metaSubLabelAuxTextEl.InnerHtml.AppendHtml(AreaSubLabel);
				metaLabelAuxEl.InnerHtml.AppendHtml(metaSubLabelAuxTextEl);
			}

			metaColEl.InnerHtml.AppendHtml(metaLabelAuxEl);

			var metaLabelEl = new TagBuilder("div");
			metaLabelEl.AddCssClass("meta-label");

			if (!String.IsNullOrWhiteSpace(IconClass))
			{
				var metaLabelIconWrapperEl = new TagBuilder("span");
				metaLabelIconWrapperEl.AddCssClass("meta-icon");
				if (!String.IsNullOrWhiteSpace(Color))
				{
					metaLabelIconWrapperEl.Attributes.Add("style", $"background-color:{Color};");
				}
				var metaLabelIconEl = new TagBuilder("span");
				metaLabelIconEl.AddCssClass(IconClass);
				if (!String.IsNullOrWhiteSpace(IconColor))
				{
					metaLabelIconEl.Attributes.Add("style", $"color:{IconColor};");
				}
				metaLabelIconWrapperEl.InnerHtml.AppendHtml(metaLabelIconEl);
				metaLabelEl.InnerHtml.AppendHtml(metaLabelIconWrapperEl);
			}


			var metaLabelTextEl = new TagBuilder("span");
			metaLabelTextEl.AddCssClass("text");
			metaLabelTextEl.InnerHtml.AppendHtml(Title);

			if (!String.IsNullOrWhiteSpace(SubTitle)) {
				var divider = new TagBuilder("span");
				divider.AddCssClass("ti-angle-right divider");
				metaLabelTextEl.InnerHtml.AppendHtml(divider);

				var metaSubLabelTextEl = new TagBuilder("span");
				metaSubLabelTextEl.AddCssClass("subtext");
				metaSubLabelTextEl.InnerHtml.AppendHtml(SubTitle);
				metaLabelTextEl.InnerHtml.AppendHtml(metaSubLabelTextEl);
			}

			if (PageSwitchItems.Count > 1) { //If only the current page there is no switch needed
				var switchDropdownEl = new TagBuilder("div");
				switchDropdownEl.AddCssClass("dropdown");
				switchDropdownEl.AddCssClass("d-inline-block");

				//link
				var metaSubLabelTextEl = new TagBuilder("a");
				metaSubLabelTextEl.AddCssClass("page-switch");
				metaSubLabelTextEl.AddCssClass("dropdown-toggle");
				metaSubLabelTextEl.Attributes.Add("data-toggle", "dropdown");
				metaSubLabelTextEl.Attributes.Add("href", "#");
				metaSubLabelTextEl.InnerHtml.AppendHtml("switch");
				switchDropdownEl.InnerHtml.AppendHtml(metaSubLabelTextEl);

				//Dropdown
				var switchDDMenuEl = new TagBuilder("div");
				switchDDMenuEl.AddCssClass("dropdown-menu");
				foreach (var pageSwitchItem in PageSwitchItems)
				{
					var switchItemEl = new TagBuilder("a");
					switchItemEl.AddCssClass("dropdown-item pl-2 pr-2");
					if (pageSwitchItem.IsSelected)
						switchItemEl.InnerHtml.AppendHtml("<i class=\"fas fa-fw fa-angle-right\"></i>");
					else
						switchItemEl.InnerHtml.AppendHtml("<i class=\"fa fa-fw\"></i>");

					switchItemEl.Attributes.Add("href", pageSwitchItem.Url);
					switchItemEl.InnerHtml.AppendHtml(pageSwitchItem.Label);
					switchDDMenuEl.InnerHtml.AppendHtml(switchItemEl);
				}
				switchDropdownEl.InnerHtml.AppendHtml(switchDDMenuEl);
				metaLabelTextEl.InnerHtml.AppendHtml(switchDropdownEl);
			}

			metaLabelEl.InnerHtml.AppendHtml(metaLabelTextEl);
			metaColEl.InnerHtml.AppendHtml(metaLabelEl);

			if (!String.IsNullOrWhiteSpace(Description)) {
				var metaDescriptionEl = new TagBuilder("div");
				metaDescriptionEl.AddCssClass("description");
				metaDescriptionEl.InnerHtml.AppendHtml(Description);
				metaColEl.InnerHtml.AppendHtml(metaDescriptionEl);
			}
			rowEl.InnerHtml.AppendHtml(metaColEl);

			if (!String.IsNullOrWhiteSpace(actionsContentHtml)) {
				var actionColEl = new TagBuilder("div");
				actionColEl.AddCssClass("col-md-auto align-self-end");
				actionColEl.InnerHtml.AppendHtml(actionsContentHtml);

				rowEl.InnerHtml.AppendHtml(actionColEl);
			}
			

			output.Content.AppendHtml(rowEl);

			if (!String.IsNullOrWhiteSpace(toolbarContentHtml))
			{
				var wrapEl = new TagBuilder("div");
				wrapEl.AddCssClass("page-header-toolbar pl-2 pr-2");
				wrapEl.InnerHtml.AppendHtml(toolbarContentHtml);
				output.Content.AppendHtml(wrapEl);
			}
			

			//return Task.CompletedTask;
		}


	}
}
