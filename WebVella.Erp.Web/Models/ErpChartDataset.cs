using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebVella.Erp.Web.Models
{
	public class ErpChartDataset
	{
		/// <summary>
		/// The label for the dataset which appears in the legend and tooltips.
		/// </summary>
		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		[JsonProperty(PropertyName = "data")]
		public List<decimal> Data { get; set; }

		[JsonProperty(PropertyName = "borderColor")]
		public object BorderColor { get; set; } = null; // List<string> or string

		[JsonProperty(PropertyName = "backgroundColor")]
		public object BackgroundColor { get; set; } = null; // List<string> or string

		[JsonProperty(PropertyName = "fill")]
		public bool? Fill { get; set; } = null;

		[JsonProperty(PropertyName = "borderWidth")]
		public int BorderWidth { get; set; } = 2;

	}
}
