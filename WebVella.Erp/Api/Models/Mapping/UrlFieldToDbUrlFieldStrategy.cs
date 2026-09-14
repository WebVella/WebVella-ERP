using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class UrlFieldToDbUrlFieldStrategy : IMapStrategy<UrlField, DbUrlField>
{
	public DbUrlField Map(UrlField source)
	{
		if (source == null)
			return null;

		var dest = new DbUrlField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		dest.OpenTargetInNewWindow = source.OpenTargetInNewWindow ?? false;
		return dest;
	}
}
