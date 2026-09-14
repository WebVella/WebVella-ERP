using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.Mapping;

namespace WebVella.Erp.Web.Models.Mapping;

internal class JTokenToErpPageStrategy : IMapStrategy<JToken, ErpPage>
{
	public ErpPage Map(JToken data)
	{
		if (data == null)
			return null;

		ErpPage model = new ErpPage();
		model.Id = new Guid(data["id"].ToString());
		model.Name = (string)data["name"];
		model.Label = (string)data["label"];
		model.System = (bool)data["system"];
		model.Type = (PageType)((int)data["type"]);
		model.IconClass = (string)data["icon_class"];
		model.Weight = (int)data["weight"];
		model.AppId = (Guid?)data["app_id"];
		model.EntityId = (Guid?)data["entity_id"];
		model.NodeId = (Guid?)data["node_id"];
		model.AreaId = (Guid?)data["area_id"];
		model.IsRazorBody = (bool)data["is_razor_body"];
		model.RazorBody = (string)data["razor_body"];
		model.Layout = (string)data["layout"];

		if (!string.IsNullOrWhiteSpace((string)data["label_translations"]))
			model.LabelTranslations = JsonConvert.DeserializeObject<List<TranslationResource>>((string)data["label_translations"]);
		else
			model.LabelTranslations = new List<TranslationResource>();

		//BODY IS NOT INIT HERE - IT SHOULD BE LOADED LAZY

		return model;
	}
}
