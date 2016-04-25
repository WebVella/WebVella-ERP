using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
	[Serializable]
	public class ActionItem
    {
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "menu")]
		public string Menu { get; set; }

		[JsonProperty(PropertyName = "template")]
		public string Template { get; set; }

		[JsonProperty(PropertyName = "weight")]
		public decimal? Weight { get; set; }
	}
}
