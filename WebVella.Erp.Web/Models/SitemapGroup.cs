using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public class SitemapGroup
	{
		[JsonProperty("id")]
		public Guid Id { get; set; } = Guid.Empty;

		[JsonProperty("weight")]
		public int Weight { get; set; } = 1;

		[JsonProperty("label")]
		public string Label { get; set; } = "";

		[JsonProperty("name")]
		public string Name { get; set; } = ""; //identifier for the sitemap nodes

		[JsonProperty("label_translations")]
		public List<TranslationResource> LabelTranslations { get; set; } = new List<TranslationResource>(); //To be easily discoverd when stored in the db one idea is to generate keys based on "sitemapId-areaName-groupName-title"

		[JsonProperty("render_roles")]
		public List<Guid> RenderRoles { get; set; } = new List<Guid>(); //show in menu for the added roles, or for all if no roles are selected
	}
}
