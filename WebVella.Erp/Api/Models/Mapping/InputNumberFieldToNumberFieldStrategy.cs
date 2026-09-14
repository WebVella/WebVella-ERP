namespace WebVella.Erp.Api.Models.Mapping;

internal class InputNumberFieldToNumberFieldStrategy : IMapStrategy<InputNumberField, NumberField>
{
	public NumberField Map(InputNumberField source)
	{
		if (source == null)
			return null;

		var dest = new NumberField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MinValue = source.MinValue;
		dest.MaxValue = source.MaxValue;
		dest.DecimalPlaces = source.DecimalPlaces;
		return dest;
	}
}
