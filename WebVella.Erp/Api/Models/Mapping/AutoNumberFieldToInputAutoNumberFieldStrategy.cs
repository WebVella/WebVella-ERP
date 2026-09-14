namespace WebVella.Erp.Api.Models.Mapping;

internal class AutoNumberFieldToInputAutoNumberFieldStrategy : IMapStrategy<AutoNumberField, InputAutoNumberField>
{

	public InputAutoNumberField Map(AutoNumberField source)
	{
		if (source == null)
			return null;

		var dest = new InputAutoNumberField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.DisplayFormat = source.DisplayFormat;
		dest.StartingNumber = source.StartingNumber;
		return dest;
	}
}
