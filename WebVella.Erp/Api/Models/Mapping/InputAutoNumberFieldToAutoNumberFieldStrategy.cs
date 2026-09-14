namespace WebVella.Erp.Api.Models.Mapping;

internal class InputAutoNumberFieldToAutoNumberFieldStrategy : IMapStrategy<InputAutoNumberField, AutoNumberField>
{
	public AutoNumberField Map(InputAutoNumberField source)
	{
		if (source == null)
			return null;

		var dest = new AutoNumberField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.DisplayFormat = source.DisplayFormat;
		dest.StartingNumber = source.StartingNumber;
		return dest;
	}
}
