using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebVella.Erp.Web.Models
{
	public class Filter
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; } = ""; //some lists can have prefix

		[JsonProperty(PropertyName = "type")]
		public FilterType Type { get; set; } = FilterType.Undefined;

		[JsonProperty(PropertyName = "value")]
		public dynamic Value { get; set; } = null;

		[JsonProperty(PropertyName = "value2")]
		public dynamic Value2 { get; set; } = null;  //used wen Between and NotBetween

		[JsonProperty(PropertyName = "prefix")]
		public string Prefix { get; set; } = ""; //some lists can have prefix
	}
}
