namespace WebVella.Erp.Api.Models.Mapping;

internal class InputPercentFieldToPercentFieldStrategy : IMapStrategy<InputPercentField, PercentField>
{
	public PercentField Map(InputPercentField source)
	{
		if (source == null)
			return null;

		var dest = new PercentField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MinValue = source.MinValue;
		dest.MaxValue = source.MaxValue;
		dest.DecimalPlaces = source.DecimalPlaces;
		return dest;
	}
}
