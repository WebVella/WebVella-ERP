using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbEntityRelationOptionsToEntityRelationOptionsItemStrategy : IMapStrategy<DbEntityRelationOptions, EntityRelationOptionsItem>
{
	public EntityRelationOptionsItem Map(DbEntityRelationOptions source)
	{
		if (source == null)
			return null;

		var dest = new EntityRelationOptionsItem();
		dest.RelationId = source.RelationId;
		dest.RelationName = source.RelationName;
		dest.Direction = source.Direction;
		return dest;
	}
}
