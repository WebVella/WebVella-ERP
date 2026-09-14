using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.Mapping;

namespace WebVella.Erp.Web.Models.Mapping;

internal class JTokenToSitemapNodeStrategy : IMapStrategy<JToken, SitemapNode>
{
	public SitemapNode Map(JToken data)
	{
		if (data == null)
			return null;

		SitemapNode model = new SitemapNode();
		model.Id = new Guid(data["id"].ToString());
		model.ParentId = (data["parent_id"] != null && data["parent_id"].Value<string>() != null)
			? new Guid(data["parent_id"].Value<string>())
			: (Guid?)null;
		model.Name = (string)data["name"];
		model.Label = (string)data["label"];
		model.Url = (string)data["url"];
		model.IconClass = (string)data["icon_class"];
		model.Type = (SitemapNodeType)((int)data["type"]);
		model.EntityId = (Guid?)data["entity_id"];
		model.Weight = (int)data["weight"];

		model.GroupName = null;

		if (!string.IsNullOrWhiteSpace((string)data["label_translations"]))
			model.LabelTranslations = JsonConvert.DeserializeObject<List<TranslationResource>>((string)data["label_translations"]);
		else
			model.LabelTranslations = new List<TranslationResource>();

		model.Access = new List<Guid>();
		if (data["access_roles"] != null)
		{
			foreach (var rId in data["access_roles"].AsJEnumerable())
				model.Access.Add(new Guid(rId.ToString()));
		}

		model.EntityListPages = new List<Guid>();
		if (data["entity_list_pages"] != null)
		{
			foreach (var rId in data["entity_list_pages"].AsJEnumerable())
				model.EntityListPages.Add(new Guid(rId.ToString()));
		}

		model.EntityCreatePages = new List<Guid>();
		if (data["entity_create_pages"] != null)
		{
			foreach (var rId in data["entity_create_pages"].AsJEnumerable())
				model.EntityCreatePages.Add(new Guid(rId.ToString()));
		}

		model.EntityDetailsPages = new List<Guid>();
		if (data["entity_details_pages"] != null)
		{
			foreach (var rId in data["entity_details_pages"].AsJEnumerable())
				model.EntityDetailsPages.Add(new Guid(rId.ToString()));
		}

		model.EntityManagePages = new List<Guid>();
		if (data["entity_manage_pages"] != null)
		{
			foreach (var rId in data["entity_manage_pages"].AsJEnumerable())
				model.EntityManagePages.Add(new Guid(rId.ToString()));
		}

		return model;
	}
}
