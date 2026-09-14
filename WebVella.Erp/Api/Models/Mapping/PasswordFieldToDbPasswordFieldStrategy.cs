using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class PasswordFieldToDbPasswordFieldStrategy : IMapStrategy<PasswordField, DbPasswordField>
{
	public DbPasswordField Map(PasswordField source)
	{
		if (source == null)
			return null;

		var dest = new DbPasswordField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.MaxLength = source.MaxLength;
		dest.MinLength = source.MinLength;
		dest.Encrypted = source.Encrypted ?? false;
		return dest;
	}
}
