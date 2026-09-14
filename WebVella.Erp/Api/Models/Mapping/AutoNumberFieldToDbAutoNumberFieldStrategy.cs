using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class AutoNumberFieldToDbAutoNumberFieldStrategy : IMapStrategy<AutoNumberField, DbAutoNumberField>
{
	public DbAutoNumberField Map(AutoNumberField source)
	{
		if (source == null)
			return null;

		var dest = new DbAutoNumberField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);

		dest.DefaultValue = source.DefaultValue;
		dest.DisplayFormat = source.DisplayFormat;
		dest.StartingNumber = source.StartingNumber;
		return dest;
	}
}
