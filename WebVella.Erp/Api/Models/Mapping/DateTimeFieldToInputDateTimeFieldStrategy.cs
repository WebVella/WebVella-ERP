namespace WebVella.Erp.Api.Models.Mapping;

internal class DateTimeFieldToInputDateTimeFieldStrategy : IMapStrategy<DateTimeField, InputDateTimeField>
{
	public InputDateTimeField Map(DateTimeField source)
	{
		if (source == null)
			return null;

		var dest = new InputDateTimeField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Format = source.Format;
		dest.UseCurrentTimeAsDefaultValue = source.UseCurrentTimeAsDefaultValue;
		return dest;
	}
}
