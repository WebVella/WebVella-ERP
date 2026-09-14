using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class EmailFieldToDbEmailFieldStrategy : IMapStrategy<EmailField, DbEmailField>
{
	public DbEmailField Map(EmailField source)
	{
		if (source == null)
			return null;

		var dest = new DbEmailField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		return dest;
	}
}
