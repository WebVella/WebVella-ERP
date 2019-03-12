using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.TagHelpers
{
	[HtmlTargetElement("wv-grid")]
	//[OutputElementHint("table")]
	public class WvGrid : TagHelper
	{
		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		[HtmlAttributeName("is-visible")]
		public bool isVisible { get; set; } = true;

		[HtmlAttributeName("columns")]
		public List<GridColumn> Columns { get; set; } = new List<GridColumn>();

		[HtmlAttributeName("striped")]
		public bool Striped { get; set; } = false;

		[HtmlAttributeName("small")]
		public bool Small { get; set; } = false;

		[HtmlAttributeName("bordered")]
		public bool Bordered { get; set; } = false;

		[HtmlAttributeName("borderless")]
		public bool Borderless { get; set; } = false;

		[HtmlAttributeName("hover")]
		public bool Hover { get; set; } = false;

		[HtmlAttributeName("responsive-breakpoint")]
		public CssBreakpoint ResponsiveBreakpoint { get; set; } = CssBreakpoint.None;

		[HtmlAttributeName("vertical-align")]
		public VerticalAlignmentType VerticalAlign { get; set; } = VerticalAlignmentType.None;

		[HtmlAttributeName("id")]
		public string Id { get; set; } = "";

		[HtmlAttributeName("prefix")]
		public string Prefix { get; set; } = "";

		[HtmlAttributeName("name")]
		public string Name { get; set; } = "";

		[HtmlAttributeName("culture")]
		public CultureInfo Culture { get; set; } = new CultureInfo("en-US");

		[HtmlAttributeName("class")]
		public string Class { get; set; } = "";

		[HtmlAttributeName("query-string-sortby")]
		public string QueryStringSortBy { get; set; } = "sortBy";

		[HtmlAttributeName("query-string-sort-order")]
		public string QueryStringSortOrder { get; set; } = "sortOrder";

		[HtmlAttributeName("query-string-page")]
		public string QueryStringPage { get; set; } = "page";

		[HtmlAttributeName("page")]
		public int Page { get; set; } = 1;

		[HtmlAttributeName("total-count")]
		public int TotalCount { get; set; } = 0;

		[HtmlAttributeName("page-size")]
		public int PageSize { get; set; } = 0;

		[HtmlAttributeName("has-thead")]
		public bool HasThead { get; set; } = true;

		[HtmlAttributeName("has-tfoot")]
		public bool HasTfoot { get; set; } = true;

		private int RecordsCount { get; set; } = 0;

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (!isVisible)
			{
				output.SuppressOutput();
				return;
			}
			#region << Init >>
			if (String.IsNullOrWhiteSpace(Id))
			{
				Id = "wv-" + Guid.NewGuid();
			}

			//Records Count
			if (VerticalAlign != VerticalAlignmentType.None)
			{
				context.Items[typeof(VerticalAlignmentType)] = VerticalAlign;
			}
			var content = await output.GetChildContentAsync();
			var htmlDoc = new HtmlDocument();
			htmlDoc.LoadHtml(content.GetContent());
			var actionTagHelperList = htmlDoc.DocumentNode.Descendants("tr");
			RecordsCount = actionTagHelperList.Count();

			#endregion

			#region << Setup wrapper >>
			var wrapperEl = new TagBuilder("div");
			var classList = new List<string>();
			classList.Add("erp-list");

			wrapperEl.Attributes.Add("id", Id);

			if (ResponsiveBreakpoint != CssBreakpoint.None)
			{
				var cssClass = $"{(ResponsiveBreakpoint == CssBreakpoint.XSmall ? "table-responsive" : $"table-responsive-{ResponsiveBreakpoint.GetLabel()}")}";
				classList.Add(cssClass);
			}

			if (!String.IsNullOrWhiteSpace(Name))
			{
				classList.Add($"erp-list-{Name}");
			}

			if (!String.IsNullOrWhiteSpace(Prefix))
			{
				wrapperEl.Attributes.Add("data-list-query-prefix", $"{Prefix}");
			}

			wrapperEl.Attributes.Add("class", String.Join(" ", classList));

			output.PreElement.AppendHtml(wrapperEl.RenderStartTag());
			output.PostElement.AppendHtml(wrapperEl.RenderEndTag());
			#endregion

			output.TagName = "table";

			#region << Table >>
			output.AddCssClass("table");

			#region << Css classes >>
			if (Striped)
			{
				output.AddCssClass("table-striped");
			}

			if (Small)
			{
				output.AddCssClass("table-sm");
			}

			if (Bordered)
			{
				output.AddCssClass("table-bordered");
			}

			if (Borderless)
			{
				output.AddCssClass("table-borderless");
			}

			if (Hover)
			{
				output.AddCssClass("table-hover");
			}

			if (!String.IsNullOrWhiteSpace(Class))
			{
				output.AddCssClass(Class);
			}

			if (Id != null)
			{
				output.Attributes.Add("id", $"list-table-{Id}");
			}

			#endregion

			#endregion

			#region << Thead >>
			if (HasThead)
			{
				var theadEl = new TagBuilder("thead");
				var trEl = new TagBuilder("tr");

				foreach (var column in Columns)
				{
					var thEl = new TagBuilder("th");
					var columnCssList = new List<string>();
					if (!String.IsNullOrWhiteSpace(column.Class)) {
						columnCssList.Add(column.Class);
					}
					thEl.Attributes.Add("data-filter-name", $"{column.Name}");
					if (!String.IsNullOrEmpty(column.Width))
					{
						thEl.Attributes.Add("style", $"width:{column.Width};");
					}
					thEl.AddCssClass(String.Join(" ", columnCssList));

					var sortedColumn = "";
					if (ViewContext.HttpContext.Request.Query.ContainsKey((String.IsNullOrWhiteSpace(Prefix) ? "" : Prefix) + QueryStringSortBy))
					{
						sortedColumn = ViewContext.HttpContext.Request.Query[(String.IsNullOrWhiteSpace(Prefix) ? "" : Prefix) + QueryStringSortBy];
					}
					var sortOrder = "";
					if (ViewContext.HttpContext.Request.Query.ContainsKey((String.IsNullOrWhiteSpace(Prefix) ? "" : Prefix) + QueryStringSortOrder))
					{
						sortOrder = ViewContext.HttpContext.Request.Query[(String.IsNullOrWhiteSpace(Prefix) ? "" : Prefix) + QueryStringSortOrder];
					}
					thEl.InnerHtml.AppendHtml(column.Label);
					if (column.Sortable)
					{
						var columnSortOrder = "";
						if (sortedColumn == column.Name)
						{
							columnSortOrder = sortOrder;
						}

						var sortLink = new TagBuilder("a");
						sortLink.AddCssClass("sort-link");
						sortLink.Attributes.Add("href", "javascript:void(0)");
						sortLink.Attributes.Add("data-dataname", $"{column.Name}");
						var sortMarkerEl = new TagBuilder("span");
						sortMarkerEl.AddCssClass($"ml-1 sort-marker {columnSortOrder}");
						var caretUpIcon = new TagBuilder("span");
						caretUpIcon.AddCssClass("fa fa-caret-up");
						sortMarkerEl.InnerHtml.AppendHtml(caretUpIcon);
						var caretDownIcon = new TagBuilder("span");
						caretDownIcon.AddCssClass("fa fa-caret-down");
						sortMarkerEl.InnerHtml.AppendHtml(caretDownIcon);
						sortLink.InnerHtml.AppendHtml(sortMarkerEl);
						sortLink.InnerHtml.AppendHtml(new TagBuilder("em"));

						thEl.InnerHtml.AppendHtml(sortLink);
					}
					trEl.InnerHtml.AppendHtml(thEl);
				}

				theadEl.InnerHtml.AppendHtml(trEl);
				output.PreContent.AppendHtml(theadEl);
			}
			#endregion

			#region << Tbody >>
			var tbodyEl = new TagBuilder("tbody");
			output.PreContent.AppendHtml(tbodyEl.RenderStartTag());

			output.PostContent.AppendHtml(tbodyEl.RenderEndTag());
			#endregion

			#region << Tfoot >>
			if (HasTfoot)
			{
				var tfootEl = new TagBuilder("tfoot");
				var trEl = new TagBuilder("tr");
				var tdEl = new TagBuilder("td");
				tdEl.Attributes.Add("colspan", Columns.Count.ToString());

				if (TotalCount > (PageSize * Page) || Page != 1)
				{
					var inputGroupEl = new TagBuilder("div");
					inputGroupEl.AddCssClass("input-group float-right input-group-sm pager-goto");
					var inputPrependEl = new TagBuilder("span");
					inputPrependEl.AddCssClass("input-group-prepend");
					var inputPrependTextEl = new TagBuilder("span");
					inputPrependTextEl.AddCssClass("input-group-text");
					inputPrependTextEl.InnerHtml.Append("page");
					inputPrependEl.InnerHtml.AppendHtml(inputPrependTextEl);
					inputGroupEl.InnerHtml.AppendHtml(inputPrependEl);

					var inputCtrEl = new TagBuilder("input");
					inputCtrEl.AddCssClass("form-control");
					inputCtrEl.Attributes.Add("value", Page.ToString());
					inputCtrEl.Attributes.Add("id", $"list-pager-form-{Id}");
					inputGroupEl.InnerHtml.AppendHtml(inputCtrEl);

					tdEl.InnerHtml.AppendHtml(inputGroupEl);
				}
				else
				{
					var floatEl = new TagBuilder("div");
					floatEl.AddCssClass("pager-info float-right");
					var divider = new TagBuilder("span");
					divider.AddCssClass("divider go-gray mr-2 ml-2");
					divider.InnerHtml.Append("|");
					floatEl.InnerHtml.AppendHtml(divider);
					var spanEl = new TagBuilder("span");
					spanEl.InnerHtml.AppendHtml("Single page");
					floatEl.InnerHtml.AppendHtml(spanEl);
					tdEl.InnerHtml.AppendHtml(floatEl);
				}

				if (TotalCount != 0)
				{
					var upperRecordCount = Page * PageSize;
					if (TotalCount < upperRecordCount)
					{
						upperRecordCount = TotalCount;
					}
					var floatEl = new TagBuilder("div");
					floatEl.AddCssClass("pager-info float-right");
					var pageString = $"{(Page - 1) * PageSize + 1}-{upperRecordCount} of {TotalCount}";
					floatEl.InnerHtml.Append(pageString);
					tdEl.InnerHtml.AppendHtml(floatEl);
				}

				if (PageSize == RecordsCount || Page != 1)
				{
					var pagePrevDisabled = Page == 1;
					var pageNextDisabled = (TotalCount <= (PageSize * Page));

					var btnGroupEl = new TagBuilder("div");
					btnGroupEl.AddCssClass("btn-group float-left pager");

					var prevBtnEl = new TagBuilder("button");
					prevBtnEl.Attributes.Add("type", "button");
					prevBtnEl.Attributes.Add("title", "Previous page");
					prevBtnEl.AddCssClass($"btn btn-sm btn-outline-secondary {(pagePrevDisabled ? "disabled" : "")}");
					if (pagePrevDisabled)
					{
						prevBtnEl.Attributes.Add("disabled", "disabled");
					}
					prevBtnEl.Attributes.Add("onclick", $"ErpListChangePage('{(String.IsNullOrWhiteSpace(Prefix) ? "" : Prefix) + QueryStringPage}',{Page - 1})");
					var prevBtnIconEl = new TagBuilder("span");
					prevBtnIconEl.AddCssClass("fa fa-fw fa-angle-left mr-1");
					prevBtnEl.InnerHtml.AppendHtml(prevBtnIconEl);
					prevBtnEl.InnerHtml.Append("Prev");
					btnGroupEl.InnerHtml.AppendHtml(prevBtnEl);

					var nextBtnEl = new TagBuilder("button");
					nextBtnEl.Attributes.Add("type", "button");
					nextBtnEl.Attributes.Add("title", "Next page");
					nextBtnEl.AddCssClass($"btn btn-sm btn-outline-secondary {(pageNextDisabled ? "disabled" : "")}");
					if (pageNextDisabled)
					{
						nextBtnEl.Attributes.Add("disabled", "disabled");
					}
					nextBtnEl.Attributes.Add("onclick", $"ErpListChangePage('{(String.IsNullOrWhiteSpace(Prefix) ? "" : Prefix) + QueryStringPage}',{Page + 1})");
					nextBtnEl.InnerHtml.Append("Next");
					var nextBtnIconEl = new TagBuilder("span");
					nextBtnIconEl.AddCssClass("fa fa-fw fa-angle-right ml-1");
					nextBtnEl.InnerHtml.AppendHtml(nextBtnIconEl);
					btnGroupEl.InnerHtml.AppendHtml(nextBtnEl);

					tdEl.InnerHtml.AppendHtml(btnGroupEl);
				}
				trEl.InnerHtml.AppendHtml(tdEl);
				tfootEl.InnerHtml.AppendHtml(trEl);
				output.PostContent.AppendHtml(tfootEl);
			}
			#endregion

			var jsCompressor = new JavaScriptCompressor();

			#region << Init Scripts >>
			var tagHelperInitialized = false;
			if (ViewContext.HttpContext.Items.ContainsKey(typeof(WvGrid) + "-sort"))
			{
				var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(WvGrid) + "-sort"];
				tagHelperInitialized = tagHelperContext.Initialized;
			}
			if (!tagHelperInitialized)
			{
				var scriptContent = FileService.GetEmbeddedTextResource("sort-and-page.js", "WebVella.Erp.Web.TagHelpers.WvGrid");
				var scriptEl = new TagBuilder("script");
				scriptEl.Attributes.Add("type", "text/javascript");
				scriptEl.InnerHtml.AppendHtml(jsCompressor.Compress(scriptContent));
				output.PostElement.AppendHtml(scriptEl);

				ViewContext.HttpContext.Items[typeof(WvGrid) + "-sort"] = new WvTagHelperContext()
				{
					Initialized = true
				};

			}
			#endregion

			#region << Add Inline Init Script for this instance >>
			var initScript = new TagBuilder("script");
			initScript.Attributes.Add("type", "text/javascript");
			var scriptTemplate = @"
						$(function(){
							ErpListPagerInputSubmit(""list-pager-form-{{Id}}"",""{{QueryStringPage}}"");
							ErpListSortInit(""{{Id}}"",""{{QueryStringSortBy}}"",""{{QueryStringSortOrder}}"");
						});";
			scriptTemplate = scriptTemplate.Replace("{{Id}}", Id.ToString());
			scriptTemplate = scriptTemplate.Replace("{{QueryStringSortBy}}", (String.IsNullOrWhiteSpace(Prefix) ? "" : Prefix) + QueryStringSortBy);
			scriptTemplate = scriptTemplate.Replace("{{QueryStringSortOrder}}", (String.IsNullOrWhiteSpace(Prefix) ? "" : Prefix) + QueryStringSortOrder);
			scriptTemplate = scriptTemplate.Replace("{{QueryStringPage}}", (String.IsNullOrWhiteSpace(Prefix) ? "" : Prefix) + QueryStringPage);

			initScript.InnerHtml.AppendHtml(jsCompressor.Compress(scriptTemplate));

			output.PostElement.AppendHtml(initScript);
			#endregion


			context.Items["Id"] = Id;
			context.Items["Culture"] = Culture;
			context.Items["QueryStringSortBy"] = QueryStringSortBy;
			context.Items["QueryStringSortOrder"] = QueryStringSortOrder;
			context.Items["QueryStringPage"] = QueryStringPage;
			context.Items["HasThead"] = HasThead;

			if (!String.IsNullOrWhiteSpace(Prefix))
			{
				context.Items["Prefix"] = Prefix;
			}
			if (!String.IsNullOrWhiteSpace(Name))
			{
				context.Items["Name"] = Name;
			}

			if (Columns.Any())
			{
				context.Items["Columns"] = Columns;
			}
			if (VerticalAlign != VerticalAlignmentType.None) {
				context.Items[typeof (VerticalAlignmentType)] = VerticalAlign;
			}
			//return Task.CompletedTask;
		}
	}
}
