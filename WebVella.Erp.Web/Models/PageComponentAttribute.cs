using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.Erp.Web.Models
{
	public class PageComponentAttribute : Attribute
	{
		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; } = "";

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; } = "";

		[JsonProperty(PropertyName = "icon_class")]
		public string IconClass { get; set; } = "";

		[JsonProperty(PropertyName = "color")]
		public string Color { get; set; } = "";

		[JsonProperty(PropertyName = "category")]
		public string Category { get; set; } = "";

		[JsonProperty(PropertyName = "library")]
		public string Library { get; set; } = "";

		[JsonProperty(PropertyName = "version")]
		public string Version { get; set; } = "";

		[JsonProperty(PropertyName = "is_inline")]
		public bool IsInline { get; set; } = false;

		[JsonProperty(PropertyName = "tags")]
		public List<string> Tags { get; private set; } = new List<string>();
	}
}
