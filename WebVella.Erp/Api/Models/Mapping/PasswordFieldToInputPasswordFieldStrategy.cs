namespace WebVella.Erp.Api.Models.Mapping;

internal class PasswordFieldToInputPasswordFieldStrategy : IMapStrategy<PasswordField, InputPasswordField>
{
	public InputPasswordField Map(PasswordField source)
	{
		if (source == null)
			return null;

		var dest = new InputPasswordField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.MaxLength = source.MaxLength;
		dest.MinLength = source.MinLength;
		dest.Encrypted = source.Encrypted;
		return dest;
	}
}
