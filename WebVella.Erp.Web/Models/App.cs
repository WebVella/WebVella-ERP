using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.Erp.Web.Models
{
	public class App
	{
		[JsonProperty("id")]
		public Guid Id { get; set; } = Guid.Empty;

		[JsonProperty("name")]
		public string Name { get; set; } = "";

		[JsonProperty("label")]
		public string Label { get; set; } = "";

		[JsonProperty("description")]
		public string Description { get; set; } = "";

		[JsonProperty("icon_class")]
		public string IconClass { get; set; } = "";

		[JsonProperty("author")]
		public string Author { get; set; } = "";

		[JsonProperty("color")]
		public string Color { get; set; } = "#2196F3";

		[JsonProperty("sitemap")]
		public Sitemap Sitemap { get; set; } = null;

		[JsonProperty("home_pages")]
		public List<ErpPage> HomePages { get; set; } = new List<ErpPage>();

		[JsonProperty("entities")]
		public List<AppEntity> Entities { get; set; } = new List<AppEntity>();

		[JsonProperty("weight")]
		public int Weight { get; set; } = 1;

		[JsonProperty("access")]
		public List<Guid> Access { get; set; } = new List<Guid>(); //show in menu for the added roles, or for all if no roles are selected

	}
}
