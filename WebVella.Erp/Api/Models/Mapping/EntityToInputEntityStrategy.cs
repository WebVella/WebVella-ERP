namespace WebVella.Erp.Api.Models.Mapping;

internal class EntityToInputEntityStrategy : IMapStrategy<Entity, InputEntity>
{
	public InputEntity Map(Entity source)
	{
		if (source == null)
			return null;

		var dest = new InputEntity();
		dest.Id = source.Id;
		dest.Name = source.Name;
		dest.Label = source.Label;
		dest.LabelPlural = source.LabelPlural;
		dest.System = source.System;
		dest.IconName = source.IconName;
		dest.Color = source.Color;
		dest.RecordPermissions = source.RecordPermissions;
		dest.RecordScreenIdField = source.RecordScreenIdField;
		return dest;
	}
}
