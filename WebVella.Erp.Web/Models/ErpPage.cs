using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Web.Models
{
	public class ErpPage
	{
		[JsonProperty("id")]
		public Guid Id { get; set; } = Guid.Empty;

		[JsonProperty("weight")]
		public int Weight { get; set; } = 10;

		[JsonProperty("label")]
		public string Label { get; set; } = "";

		[JsonProperty("label_translations")]
		public List<TranslationResource> LabelTranslations { get; set; } = new List<TranslationResource>();

		[JsonProperty("name")]
		public string Name { get; set; } = "";

		[JsonProperty("icon_class")]
		public string IconClass { get; set; } = null; //overrides the default one

		[JsonProperty("system")]
		public bool System { get; set; } = false; //for the pages that should not be deleted or presented in the lists -> default home, list and record page

		[JsonProperty("type")]
		public PageType Type { get; set; } = PageType.Site;

		[JsonProperty("app_id")]
		public Guid? AppId { get; set; } = null; //required only for Application Page

		[JsonProperty("entity_id")]
		public Guid? EntityId { get; set; } = null; //required only for EntityRecord and EntityList page type

		[JsonProperty("sitemap_area_id")]
		public Guid? AreaId { get; set; } = null; //required when app page to fine tune the sibling pages

		[JsonProperty("node_id")]
		public Guid? NodeId { get; set; } = null; //required when app page to fine tune the sibling pages

		[JsonProperty("is_razor_body")]
		public bool IsRazorBody { get; set; } = false; //specifies that body is razor view source code

		[JsonProperty("razor_body")]
		public string RazorBody { get; set; } = string.Empty;

		[JsonProperty("layout")]
		public string Layout { get; set; } = string.Empty;

		//lazy loading 
		private List<PageBodyNode> body = null;
		[JsonProperty("body")]
		public List<PageBodyNode> Body
		{
			get
			{
				if (body == null)
					body = new PageService().GetPageBody(Id);

				return body;
			}
			set
			{
				body = value;
			}
		}
	}
}
