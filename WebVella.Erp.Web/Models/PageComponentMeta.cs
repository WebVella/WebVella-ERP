using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Web.Models
{
	public class PageComponentMeta
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		[JsonProperty(PropertyName = "icon_class")]
		public string IconClass { get; set; }

		[JsonProperty(PropertyName = "color")]
		public string Color { get; set; }

		[JsonProperty(PropertyName = "category")]
		public string Category { get; set; }

		[JsonProperty(PropertyName = "library")]
		public string Library { get; set; }

		[JsonProperty(PropertyName = "design_view_url")]
		public string DesignViewUrl { get; set; }

		[JsonProperty(PropertyName = "options_view_url")]
		public string OptionsViewUrl { get; set; }

		[JsonProperty(PropertyName = "help_view_url")]
		public string HelpViewUrl { get; set; }

		[JsonProperty(PropertyName = "service_js_url")]
		public string ServiceJsUrl { get; set; }

		[JsonProperty(PropertyName = "version")]
		public string Version { get; set; } = "1.0.0";

		[JsonProperty(PropertyName = "is_inline")]
		public bool IsInline { get; set; } = false;

		[JsonProperty(PropertyName = "usage_counter")]
		public int UsageCounter { get; set; } = 0;

		[JsonProperty(PropertyName = "last_used_on")]
		public DateTime LastUsedOn { get; set; } = DateTime.MinValue;
	}
}
