using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.Mapping;

namespace WebVella.Erp.Web.Models.Mapping;

internal class JTokenToSitemapAreaStrategy : IMapStrategy<JToken, SitemapArea>
{
	public SitemapArea Map(JToken data)
	{
		if (data == null)
			return null;

		SitemapArea model = new SitemapArea();
		model.Id = new Guid(data["id"].ToString());
		model.AppId = new Guid(data["app_id"].ToString());
		model.Name = (string)data["name"];
		model.Label = (string)data["label"];
		model.Description = (string)data["description"];
		model.IconClass = (string)data["icon_class"];
		model.ShowGroupNames = (bool)data["show_group_names"];
		model.Color = (string)data["color"];
		model.Weight = (int)data["weight"];

		if (!string.IsNullOrWhiteSpace((string)data["label_translations"]))
			model.LabelTranslations = JsonConvert.DeserializeObject<List<TranslationResource>>((string)data["label_translations"]);
		else
			model.LabelTranslations = new List<TranslationResource>();

		if (!string.IsNullOrWhiteSpace((string)data["description_translations"]))
			model.DescriptionTranslations = JsonConvert.DeserializeObject<List<TranslationResource>>((string)data["description_translations"]);
		else
			model.DescriptionTranslations = new List<TranslationResource>();

		model.Access = new List<Guid>();
		if (data["access"] != null)
		{
			foreach (var rId in data["access"].AsJEnumerable())
				model.Access.Add(new Guid(rId.ToString()));
		}

		model.Groups = new List<SitemapGroup>();
		if (data["groups"] != null)
		{
			foreach (var jGroup in data["groups"].AsJEnumerable())
				model.Groups.Add(MappingExtensions.MapToSingleObject<SitemapGroup>(jGroup));
		}

		model.Nodes = new List<SitemapNode>();
		if (data["nodes"] != null)
		{
			foreach (var jNode in data["nodes"].AsJEnumerable())
				model.Nodes.Add(MappingExtensions.MapToSingleObject<SitemapNode>(jNode));
		}

		return model;
	}
}
