using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class FileFieldToDbFileFieldStrategy : IMapStrategy<FileField, DbFileField>
{
	public DbFileField Map(FileField source)
	{
		if (source == null)
			return null;

		var dest = new DbFileField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		return dest;
	}
}
