using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using Yahoo.Yui.Compressor;

namespace WebVella.Erp.Web.TagHelpers
{

	[HtmlTargetElement("wv-chart")]
	public class WvChart : TagHelper
	{
		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		[HtmlAttributeName("is-visible")]
		public bool isVisible { get; set; } = true;

		[HtmlAttributeName("width")]
		public string Width { get; set; } = null;

		[HtmlAttributeName("height")]
		public string Height { get; set; } = null;

		[HtmlAttributeName("id")]
		public string Id { get; set; } = "";

		[HtmlAttributeName("type")]
		public ErpChartType Type { get; set; } = ErpChartType.Line;

		[HtmlAttributeName("datasets")]
		public List<ErpChartDataset> Datasets { get; set; } = new List<ErpChartDataset>();

		[HtmlAttributeName("labels")]
		public List<string> Labels { get; set; } = new List<string>();

		[HtmlAttributeName("show-legend")]
		public bool ShowLegend { get; set; } = false;

		private ErpChart Chart { get; set; } = null;




		public bool InitChart(TagHelperContext context, TagHelperOutput output)
		{
			var isSuccess = true;

			#region << Init >>
			output.TagName = null;
			if (string.IsNullOrWhiteSpace(Id))
			{
				Id = "wv-chart-" + Guid.NewGuid();
			}

			#region << Add Chart lib >>
			{
				var tagHelperInitialized = false;
				var fileName = "chart.js";
				if (ViewContext.HttpContext.Items.ContainsKey(typeof(ErpChart) + fileName))
				{
					var tagHelperContext = (WvTagHelperContext)ViewContext.HttpContext.Items[typeof(ErpChart) + fileName];
					tagHelperInitialized = tagHelperContext.Initialized;
				}
				if (!tagHelperInitialized)
				{
					var scriptEl = new TagBuilder("script");
					scriptEl.Attributes.Add("type", "text/javascript");
					scriptEl.Attributes.Add("src", "/lib/Chart.js/Chart.bundle.min.js");
					output.PreElement.AppendHtml(scriptEl);

					ViewContext.HttpContext.Items[typeof(ErpChart) + fileName] = new WvTagHelperContext()
					{
						Initialized = true
					};

				}
			}
			#endregion

			#endregion

			#region << Chart Wrapper >>
			var chartWrapperEl = new TagBuilder("div");
			chartWrapperEl.AddCssClass("d-block");
			var style = "position: relative;";
			if (Height != null)
				style += "height:" + Height + ";";

			if (Width != null)
				style += "width:" + Width + ";";
			else
				style += "width:100%;";
			chartWrapperEl.Attributes.Add("style", style);
			#endregion

			#region << PreElement >>
			output.PreElement.AppendHtml(chartWrapperEl.RenderStartTag());
			#endregion


			#region << RenderCanvas >>
			var canvasEl = new TagBuilder("canvas");
			canvasEl.Attributes.Add("id", Id);
			//canvasEl.Attributes.Add("height", "300");
			//canvasEl.Attributes.Add("width", "300");
			output.Content.AppendHtml(canvasEl);

			#endregion

			#region << PostElement
			output.PostElement.AppendHtml(chartWrapperEl.RenderEndTag());
			#endregion

			return isSuccess;
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (!isVisible)
			{
				output.SuppressOutput();
				return;
			}

			#region << Init >>
			var initSuccess = InitChart(context, output);

			if (!initSuccess)
			{
				return;
			}

			Chart = new ErpChart(Type, Labels, Datasets, ShowLegend);

			#endregion

			#region << Render Js>>
			var scriptJs = "<script>";
			scriptJs += "$(function(){";
			scriptJs += $"new Chart('{Id}', {{";
			scriptJs += $"type: '{ModelExtensions.GetLabel(Chart.Type)}',";
			scriptJs += $"data: " + JsonConvert.SerializeObject(Chart.Data) + ",";
			scriptJs += $"options: " + JsonConvert.SerializeObject(Chart.Options);
			scriptJs += "});";
			scriptJs += "});";
			scriptJs += "</script>";

			//var tooltipCallback = "{title: function(tooltipArray, data){return '';}, label: function(tooltipItem, data){return tooltipItem.xLabel + ' : ' + tooltipItem.yLabel;} }";
			//scriptJs = scriptJs.Replace("\"{{tooltipCallback}}\"", tooltipCallback);
			output.PostContent.AppendHtml(scriptJs);

			#endregion

			//prevents CS1998 warning (no use of await in async method)
			await Task.FromResult<object>(null);
		}

		
	}
}
