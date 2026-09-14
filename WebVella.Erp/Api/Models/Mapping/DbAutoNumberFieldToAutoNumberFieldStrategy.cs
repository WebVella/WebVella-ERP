using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbAutoNumberFieldToAutoNumberFieldStrategy : IMapStrategy<DbAutoNumberField, AutoNumberField>
{
	public AutoNumberField Map(DbAutoNumberField source)
	{
		if (source == null)
			return null;

		var dest = new AutoNumberField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.DisplayFormat = source.DisplayFormat;
		dest.StartingNumber = source.StartingNumber;
		return dest;
	}
}
