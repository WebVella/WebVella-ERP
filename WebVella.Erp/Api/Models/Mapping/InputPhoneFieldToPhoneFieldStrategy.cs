namespace WebVella.Erp.Api.Models.Mapping;

internal class InputPhoneFieldToPhoneFieldStrategy : IMapStrategy<InputPhoneField, PhoneField>
{
	public PhoneField Map(InputPhoneField source)
	{
		if (source == null)
			return null;

		var dest = new PhoneField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Format = source.Format;
		dest.MaxLength = source.MaxLength;
		return dest;
	}
}
