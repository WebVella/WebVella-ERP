using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.Mapping;

namespace WebVella.Erp.Web.Models.Mapping;

internal class ErpPageToEntityRecordStrategy : IMapStrategy<ErpPage, EntityRecord>
{
	public EntityRecord Map(ErpPage data)
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
