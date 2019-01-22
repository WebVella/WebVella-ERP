using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public class SitemapArea
	{
		[JsonProperty("id")]
		public Guid Id { get; set; } = Guid.Empty;

		[JsonProperty("app_id")]
		public Guid AppId { get; set; } = Guid.Empty;

		[JsonProperty("weight")]
		public int Weight { get; set; } = 1;

		[JsonProperty("label")]
		public string Label { get; set; } = "";

		[JsonProperty("description")]
		public string Description { get; set; } = "";

		[JsonProperty("name")]
		public string Name { get; set; } = "";

		[JsonProperty("icon_class")]
		public string IconClass { get; set; } = "";

		[JsonProperty("show_group_names")]
		public bool ShowGroupNames { get; set; } = false;

		[JsonProperty("color")]
		public string Color { get; set; } = "";

		[JsonProperty("label_translations")]
		public List<TranslationResource> LabelTranslations { get; set; } = new List<TranslationResource>(); //To be easily discovered when stored in the db one idea is to generate keys based on "sitemapId-areaName-title"

		[JsonProperty("description_translations")]
		public List<TranslationResource> DescriptionTranslations { get; set; } = new List<TranslationResource>(); //To be easily discovered when stored in the db one idea is to generate keys based on "sitemapId-areaName-title"

		[JsonProperty("groups")]
		public List<SitemapGroup> Groups { get; set; } = new List<SitemapGroup>(); //can have hardcoded URL

		[JsonProperty("nodes")]
		public List<SitemapNode> Nodes { get; set; } = new List<SitemapNode>(); //can have hardcoded URL

		[JsonProperty("access")]
		public List<Guid> Access { get; set; } = new List<Guid>(); //show in menu for the added roles, or for all if no roles are selected

	}
}
