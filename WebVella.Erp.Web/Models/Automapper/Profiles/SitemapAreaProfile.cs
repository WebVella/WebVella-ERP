using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using Newtonsoft.Json.Linq;
using WebVella.Erp.Api;
using Newtonsoft.Json;
using WebVella.Erp.Api.Models.AutoMapper;

namespace WebVella.Erp.Web.Models.AutoMapper.Profiles
{
	public class SitemapAreaProfile : Profile
	{
		public SitemapAreaProfile()
		{
			CreateMap<JToken, SitemapArea>().ConvertUsing(source => JTokenToSitemapAreaConvert(source));
		}

		private static SitemapArea JTokenToSitemapAreaConvert(JToken data)
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
					model.Groups.Add(jGroup.MapToSingleObject<SitemapGroup>());
			}

			model.Nodes = new List<SitemapNode>();
			if (data["nodes"] != null)
			{
				foreach (var jNode in data["nodes"].AsJEnumerable())
					model.Nodes.Add(jNode.MapToSingleObject<SitemapNode>());
			}

			return model;
		}
	}
}
