using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class EntityRelationToDbEntityRelationStrategy : IMapStrategy<EntityRelation, DbEntityRelation>
{
	public DbEntityRelation Map(EntityRelation source)
	{
		if (source == null)
			return null;

		var dest = new DbEntityRelation();
		dest.Id = source.Id;
		dest.Name = source.Name;
		dest.Label = source.Label;
		dest.Description = source.Description;
		dest.System = source.System;
		dest.RelationType = source.RelationType;
		dest.OriginEntityId = source.OriginEntityId;
		dest.OriginFieldId = source.OriginFieldId;
		dest.TargetEntityId = source.TargetEntityId;
		dest.TargetFieldId = source.TargetFieldId;
		return dest;
	}
}
