using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public class ErpChartData
	{
		[JsonProperty(PropertyName = "labels")]
		public List<string> Labels { get; set; }

		[JsonProperty(PropertyName = "datasets")]
		public List<ErpChartDataset> Datasets { get; set; }

	}
}
