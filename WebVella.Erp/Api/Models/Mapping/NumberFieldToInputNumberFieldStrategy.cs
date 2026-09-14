namespace WebVella.Erp.Api.Models.Mapping;

internal class NumberFieldToInputNumberFieldStrategy : IMapStrategy<NumberField, InputNumberField>
{
	public InputNumberField Map(NumberField source)
	{
		if (source == null)
			return null;

		var dest = new InputNumberField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MinValue = source.MinValue;
		dest.MaxValue = source.MaxValue;
		dest.DecimalPlaces = source.DecimalPlaces;
		return dest;
	}
}
