using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class EntityRelationOptionsItemToDbEntityRelationOptionsStrategy : IMapStrategy<EntityRelationOptionsItem, DbEntityRelationOptions>
{
	public DbEntityRelationOptions Map(EntityRelationOptionsItem source)
	{
		if (source == null)
			return null;

		var dest = new DbEntityRelationOptions();
		dest.RelationId = source.RelationId;
		dest.RelationName = source.RelationName;
		dest.Direction = source.Direction;
		return dest;
	}
}
