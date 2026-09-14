using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbFileFieldToFileFieldStrategy : IMapStrategy<DbFileField, FileField>
{
	public FileField Map(DbFileField source)
	{
		if (source == null)
			return null;

		var dest = new FileField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		return dest;
	}
}
