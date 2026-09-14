using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class ImageFieldToDbImageFieldStrategy : IMapStrategy<ImageField, DbImageField>
{
	public DbImageField Map(ImageField source)
	{
		if (source == null)
			return null;

		var dest = new DbImageField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		return dest;
	}
}
