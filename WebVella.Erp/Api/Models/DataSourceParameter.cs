using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
	public class DataSourceParameter
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "value")]
		public string Value { get; set; }

		[JsonProperty(PropertyName = "ignore_parse_errors")]
		public bool IgnoreParseErrors { get; set; } = false;
	}
}
