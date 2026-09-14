using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbUrlFieldToUrlFieldStrategy : IMapStrategy<DbUrlField, UrlField>
{
	public UrlField Map(DbUrlField source)
	{
		if (source == null)
			return null;

		var dest = new UrlField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		dest.OpenTargetInNewWindow = source.OpenTargetInNewWindow;
		return dest;
	}
}
