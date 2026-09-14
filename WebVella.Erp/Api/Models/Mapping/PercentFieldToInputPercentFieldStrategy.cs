namespace WebVella.Erp.Api.Models.Mapping;

internal class PercentFieldToInputPercentFieldStrategy : IMapStrategy<PercentField, InputPercentField>
{
	public InputPercentField Map(PercentField source)
	{
		if (source == null)
			return null;

		var dest = new InputPercentField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MinValue = source.MinValue;
		dest.MaxValue = source.MaxValue;
		dest.DecimalPlaces = source.DecimalPlaces;
		return dest;
	}
}
