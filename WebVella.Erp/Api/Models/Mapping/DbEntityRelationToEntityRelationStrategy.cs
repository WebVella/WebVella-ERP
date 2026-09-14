using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbEntityRelationToEntityRelationStrategy : IMapStrategy<DbEntityRelation, EntityRelation>
{
	public EntityRelation Map(DbEntityRelation source)
	{
		if (source == null)
			return null;

		var dest = new EntityRelation();
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
