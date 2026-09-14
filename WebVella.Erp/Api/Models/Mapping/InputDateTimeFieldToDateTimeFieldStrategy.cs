namespace WebVella.Erp.Api.Models.Mapping;

internal class InputDateTimeFieldToDateTimeFieldStrategy : IMapStrategy<InputDateTimeField, DateTimeField>
{
	public DateTimeField Map(InputDateTimeField source)
	{
		if (source == null)
			return null;

		var dest = new DateTimeField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Format = source.Format;
		dest.UseCurrentTimeAsDefaultValue = source.UseCurrentTimeAsDefaultValue;
		return dest;
	}
}
