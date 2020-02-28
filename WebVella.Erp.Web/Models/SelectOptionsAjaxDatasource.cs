using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public class SelectOptionsAjaxDatasource
	{
		[JsonProperty(PropertyName = "ds")]
		public string DatasourceName { get; set; } = "";

		[JsonProperty(PropertyName = "use_select_api")]
		public bool UseSelectApi { get; set; } = false;

		[JsonProperty(PropertyName = "value")]
		public string Value { get; set; } = "id";

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; } = "label";

		[JsonProperty(PropertyName = "page_size")]
		public int PageSize { get; set; } = 10;

		[JsonProperty(PropertyName = "init_options")]
		public List<SelectOption> InitOptions { get; set; } = new List<SelectOption>();
	}
}
