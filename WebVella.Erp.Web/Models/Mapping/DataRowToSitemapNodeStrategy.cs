using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.Mapping;

namespace WebVella.Erp.Web.Models.Mapping;

internal class DataRowToSitemapNodeStrategy : IMapStrategy<DataRow, SitemapNode>
{
	public SitemapNode Map(DataRow data)
	{
		if (data == null)
			return null;

		SitemapNode model = new SitemapNode();
		model.Id = new Guid(data["id"].ToString());
		model.ParentId = data["parent_id"] != DBNull.Value ? new Guid(data["parent_id"].ToString()) : (Guid?)null;
		model.Name = (string)data["name"];
		model.Label = (string)data["label"];
		model.Url = (string)data["url"];
		model.IconClass = (string)data["icon_class"];
		model.Type = (SitemapNodeType)((int)data["type"]);
		model.EntityId = data["entity_id"] == DBNull.Value ? null : (Guid?)data["entity_id"];
		model.Weight = (int)data["weight"];

		model.GroupName = null;

		if (!string.IsNullOrWhiteSpace((string)data["label_translations"]))
			model.LabelTranslations = JsonConvert.DeserializeObject<List<TranslationResource>>((string)data["label_translations"]);
		else
			model.LabelTranslations = new List<TranslationResource>();

		model.Access = new List<Guid>();
		if (data["access_roles"] != null)
		{
			model.Access.AddRange((Guid[])data["access_roles"]);
		}

		model.EntityListPages = new List<Guid>();
		if (data["entity_list_pages"] != null)
		{
			model.EntityListPages.AddRange((Guid[])data["entity_list_pages"]);
		}

		model.EntityCreatePages = new List<Guid>();
		if (data["entity_create_pages"] != null)
		{
			model.EntityCreatePages.AddRange((Guid[])data["entity_create_pages"]);
		}

		model.EntityDetailsPages = new List<Guid>();
		if (data["entity_details_pages"] != null)
		{
			model.EntityDetailsPages.AddRange((Guid[])data["entity_details_pages"]);
		}

		model.EntityManagePages = new List<Guid>();
		if (data["entity_manage_pages"] != null)
		{
			model.EntityManagePages.AddRange((Guid[])data["entity_manage_pages"]);
		}

		return model;
	}
}
