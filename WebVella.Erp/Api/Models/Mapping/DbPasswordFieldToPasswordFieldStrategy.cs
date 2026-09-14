using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbPasswordFieldToPasswordFieldStrategy : IMapStrategy<DbPasswordField, PasswordField>
{
	public PasswordField Map(DbPasswordField source)
	{
		if (source == null)
			return null;

		var dest = new PasswordField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.MaxLength = source.MaxLength;
		dest.MinLength = source.MinLength;
		dest.Encrypted = source.Encrypted;
		return dest;
	}
}
