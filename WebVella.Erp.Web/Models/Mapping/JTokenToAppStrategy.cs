using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using WebVella.Erp.Api.Models.Mapping;

namespace WebVella.Erp.Web.Models.Mapping;

internal class JTokenToAppStrategy : IMapStrategy<JToken, App>
{
	public App Map(JToken data)
	{
		if (data == null)
			return null;

		App model = new App();
		model.Id = new Guid(data["id"].ToString());
		model.Name = (string)data["name"];
		model.Label = (string)data["label"];
		model.Description = (string)data["description"];
		model.IconClass = (string)data["icon_class"];
		model.Author = (string)data["author"];
		model.Color = (string)data["color"];
		model.Weight = (int)data["weight"];

		model.Access = new List<Guid>();
		if (data["access"] != null)
		{
			foreach (var rId in data["access"].AsJEnumerable())
				model.Access.Add(new Guid(rId.ToString()));
		}

		model.Sitemap = new Sitemap();
		if (data["areas"] != null)
		{
			foreach (var jArea in data["areas"].AsJEnumerable())
				model.Sitemap.Areas.Add(MappingExtensions.MapToSingleObject<SitemapArea>(jArea));
		}

		model.HomePages = new List<ErpPage>();
		if (data["pages"] != null)
		{
			foreach (var jPage in data["pages"].AsJEnumerable())
			{
				var erpPage = MappingExtensions.MapToSingleObject<ErpPage>(jPage);
				if (erpPage.Type == PageType.Application && erpPage.AreaId == null && erpPage.NodeId == null)
				{
					model.HomePages.Add(erpPage);
				}
			}
		}

		return model;
	}
}
