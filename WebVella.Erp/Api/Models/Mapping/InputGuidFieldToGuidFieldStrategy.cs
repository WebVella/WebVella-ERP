using System;

namespace WebVella.Erp.Api.Models.Mapping;

internal class InputGuidFieldToGuidFieldStrategy : IMapStrategy<InputGuidField, GuidField>
{
	public GuidField Map(InputGuidField source)
	{
		if (source == null)
			return null;

		var dest = new GuidField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.GenerateNewId = source.GenerateNewId;
		return dest;
	}
}
