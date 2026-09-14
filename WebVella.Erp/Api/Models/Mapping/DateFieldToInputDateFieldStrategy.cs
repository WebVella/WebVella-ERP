namespace WebVella.Erp.Api.Models.Mapping;

internal class DateFieldToInputDateFieldStrategy : IMapStrategy<DateField, InputDateField>
{
	public InputDateField Map(DateField source)
	{
		if (source == null)
			return null;

		var dest = new InputDateField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Format = source.Format;
		dest.UseCurrentTimeAsDefaultValue = source.UseCurrentTimeAsDefaultValue;
		return dest;
	}
}
