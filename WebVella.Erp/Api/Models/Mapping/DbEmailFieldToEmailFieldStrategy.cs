using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbEmailFieldToEmailFieldStrategy : IMapStrategy<DbEmailField, EmailField>
{
	public EmailField Map(DbEmailField source)
	{
		if (source == null)
			return null;

		var dest = new EmailField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		return dest;
	}
}
