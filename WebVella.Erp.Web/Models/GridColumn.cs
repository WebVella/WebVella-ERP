using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public class GridColumn
	{
		[JsonProperty(PropertyName = "container_id")]
		public string ContainerId { get; set; } = "";

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; } = "";

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; } = "";

		[JsonProperty(PropertyName = "width")]
		public string Width { get; set; } = "";

		[JsonProperty(PropertyName = "searchable")]
		public bool Searchable { get; set; } = false;

		[JsonProperty(PropertyName = "sortable")]
		public bool Sortable { get; set; } = false;

		[JsonProperty(PropertyName = "class")]
		public string Class { get; set; } = "";
	}
}
