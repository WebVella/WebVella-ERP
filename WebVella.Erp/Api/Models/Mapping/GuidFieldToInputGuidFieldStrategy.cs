namespace WebVella.Erp.Api.Models.Mapping;

internal class GuidFieldToInputGuidFieldStrategy : IMapStrategy<GuidField, InputGuidField>
{
	public InputGuidField Map(GuidField source)
	{
		if (source == null)
			return null;

		var dest = new InputGuidField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.GenerateNewId = source.GenerateNewId;
		return dest;
	}
}
