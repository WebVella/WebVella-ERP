namespace WebVella.Erp.Api.Models.Mapping;

internal class InputPasswordFieldToPasswordFieldStrategy : IMapStrategy<InputPasswordField, PasswordField>
{
	public PasswordField Map(InputPasswordField source)
	{

		if (source == null)
			return null;

		var dest = new PasswordField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.MaxLength = source.MaxLength;
		dest.MinLength = source.MinLength;
		dest.Encrypted = source.Encrypted;
		return dest;
	}
}
