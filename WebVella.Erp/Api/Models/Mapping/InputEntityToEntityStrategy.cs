using System;

namespace WebVella.Erp.Api.Models.Mapping;

internal class InputEntityToEntityStrategy : IMapStrategy<InputEntity, Entity>
{
	public Entity Map(InputEntity source)
	{
		if (source == null)
			return null;

		var dest = new Entity();
		dest.Id = source.Id.HasValue ? source.Id.Value : Guid.Empty;
		dest.Name = source.Name;
		dest.Label = source.Label;
		dest.LabelPlural = source.LabelPlural;
		dest.System = source.System.HasValue ? source.System.Value : false;
		dest.IconName = source.IconName;
		dest.Color = source.Color;
		dest.RecordPermissions = source.RecordPermissions;
		dest.RecordScreenIdField = source.RecordScreenIdField;
		return dest;
	}
}
