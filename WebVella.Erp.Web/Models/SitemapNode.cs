using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public class SitemapNode
	{
		[JsonProperty("id")]
		public Guid Id { get; set; } = Guid.Empty;

		[JsonProperty("parent_id")]
		public Guid? ParentId { get; set; } = null;

		[JsonProperty("weight")]
		public int Weight { get; set; } = 1;

		[JsonProperty("group_name")]
		public string GroupName { get; set; } = ""; // If empty this means the items has no group

		[JsonProperty("label")]
		public string Label { get; set; } = "";

		[JsonProperty("name")]
		public string Name { get; set; } = "";

		[JsonProperty("icon_class")]
		public string IconClass { get; set; } = "";

		[JsonProperty("url")]
		public string Url { get; set; } = ""; //can have hardcoded URL

		[JsonProperty("label_translations")]
		public List<TranslationResource> LabelTranslations { get; set; } = new List<TranslationResource>(); //To be easily discoverd when stored in the db one idea is to generate keys based on "sitemapId-areaName-title"

		[JsonProperty("access")]
		public List<Guid> Access { get; set; } = new List<Guid>(); //show in menu for the added roles, or for all if no roles are selected

		[JsonProperty("type")]
		public SitemapNodeType Type { get; set; } = SitemapNodeType.EntityList;

		[JsonProperty("entity_id")]
		public Guid? EntityId { get; set; } = null;

		[JsonProperty("entity_list_pages")]
		public List<Guid> EntityListPages { get; set; } = new List<Guid>();

		[JsonProperty("entity_create_pages")]
		public List<Guid> EntityCreatePages { get; set; } = new List<Guid>();

		[JsonProperty("entity_details_pages")]
		public List<Guid> EntityDetailsPages { get; set; } = new List<Guid>();

		[JsonProperty("entity_manage_pages")]
		public List<Guid> EntityManagePages { get; set; } = new List<Guid>();
	}
}
