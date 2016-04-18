using Newtonsoft.Json;

namespace WebVella.ERP.Api.Models
{
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
