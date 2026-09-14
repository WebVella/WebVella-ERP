namespace WebVella.Erp.Api.Models.Mapping;

internal class PhoneFieldToInputPhoneFieldStrategy : IMapStrategy<PhoneField, InputPhoneField>
{
	public InputPhoneField Map(PhoneField source)
	{
		if (source == null)
			return null;

		var dest = new InputPhoneField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Format = source.Format;
		dest.MaxLength = source.MaxLength;
		return dest;
	}
}
