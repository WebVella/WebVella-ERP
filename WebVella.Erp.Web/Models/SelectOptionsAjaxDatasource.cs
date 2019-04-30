using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebVella.Erp.Web.Models
{
	public class SelectOptionsAjaxDatasource
	{
		[JsonProperty(PropertyName = "ds")]
		public string DatasourceName { get; set; } = "";

		[JsonProperty(PropertyName = "value")]
		public string Value { get; set; } = "id";

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; } = "label";
	}
}
