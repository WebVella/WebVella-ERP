using System;
using AutoMapper;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebVella.Erp.Api.Models.AutoMapper;

namespace WebVella.Erp.Web.Models.AutoMapper.Profiles
{
	public class ErpPageProfile : Profile
	{
		public ErpPageProfile()
		{
			CreateMap<JToken, ErpPage>().ConvertUsing(source => JTokenToErpPageConvert(source));
			CreateMap<ErpPage, EntityRecord>().ConvertUsing(source => ErpPageToEntityRecordConvert(source));
		}

		private static ErpPage JTokenToErpPageConvert(JToken data)
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


		private static EntityRecord ErpPageToEntityRecordConvert(ErpPage data)
		{
			if (data == null)
				return null;

			EntityRecord model = new EntityRecord();
			model["id"] = data.Id;
			model["name"] = data.Name;
			model["label"] = data.Label;
			model["system"] = data.System;
			model["type"] = data.Type;
			model["icon_class"] = data.IconClass;
			model["weight"] = data.Weight;
			model["app_id"] = data.AppId;
			model["entity_id"] = data.EntityId;
			model["node_id"] = data.NodeId;
			model["area_id"] = data.AreaId;
			model["is_razor_body"] = data.IsRazorBody;
			model["layout"] = data.Layout;
			return model;
		}
	}
}
