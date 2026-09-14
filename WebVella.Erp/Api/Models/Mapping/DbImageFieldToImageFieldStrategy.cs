using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbImageFieldToImageFieldStrategy : IMapStrategy<DbImageField, ImageField>
{
	public ImageField Map(DbImageField source)
	{
		if (source == null)
			return null;

		var dest = new ImageField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		return dest;
	}
}
