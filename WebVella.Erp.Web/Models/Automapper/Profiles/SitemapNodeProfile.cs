using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using Newtonsoft.Json.Linq;
using WebVella.Erp.Api;
using Newtonsoft.Json;
using System.Data;

namespace WebVella.Erp.Web.Models.AutoMapper.Profiles
{
	public class SitemapNodeProfile : Profile
	{
		public SitemapNodeProfile()
		{
			CreateMap<JToken, SitemapNode>().ConvertUsing(source => JTokenToSitemapNodeConvert(source));
			CreateMap<DataRow, SitemapNode>().ConvertUsing(source => DataRowToSitemapNodeConvert(source));
		}

		private static SitemapNode JTokenToSitemapNodeConvert(JToken data)
		{
			if (data == null)
				return null;

			SitemapNode model = new SitemapNode();
			model.Id = new Guid(data["id"].ToString());
			model.ParentId = (data["parent_id"] != null && data["parent_id"].Value<string>() != null) ? new Guid(data["parent_id"].Value<string>()) : (Guid?)null;
			model.Name = (string)data["name"];
			model.Label = (string)data["label"];
			model.Url = (string)data["url"];
			model.IconClass = (string)data["icon_class"];
			model.Type = (SitemapNodeType)((int)data["type"]);
			model.EntityId = (Guid?)data["entity_id"];
			model.Weight = (int)data["weight"];

			model.GroupName = (string)null;

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

		private static SitemapNode DataRowToSitemapNodeConvert(DataRow data)
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

			model.GroupName = (string)null;

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
}
