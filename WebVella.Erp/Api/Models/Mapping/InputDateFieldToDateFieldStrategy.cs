namespace WebVella.Erp.Api.Models.Mapping;

internal class InputDateFieldToDateFieldStrategy : IMapStrategy<InputDateField, DateField>
{
	public DateField Map(InputDateField source)
	{
		if (source == null)
			return null;

		var dest = new DateField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Format = source.Format;
		dest.UseCurrentTimeAsDefaultValue = source.UseCurrentTimeAsDefaultValue;
		return dest;
	}
}
