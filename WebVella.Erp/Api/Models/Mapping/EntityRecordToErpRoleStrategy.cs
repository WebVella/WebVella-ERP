using System;

namespace WebVella.Erp.Api.Models.Mapping;

internal class EntityRecordToErpRoleStrategy : IMapStrategy<EntityRecord, ErpRole>
{
	public ErpRole Map(EntityRecord source)
	{
		if (source == null)
			return null;

		ErpRole dest = new ErpRole();
		dest.Id = (Guid)source["id"];
		dest.Name = (string)source["name"];
		dest.Description = (string)source["description"];
		return dest;
	}
}
