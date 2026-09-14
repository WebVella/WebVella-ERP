using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class NumberFieldToDbNumberFieldStrategy : IMapStrategy<NumberField, DbNumberField>
{
	public DbNumberField Map(NumberField source)
	{
		if (source == null)
			return null;

		var dest = new DbNumberField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MinValue = source.MinValue;
		dest.MaxValue = source.MaxValue;
		dest.DecimalPlaces = source.DecimalPlaces ?? 2;
		return dest;
	}
}
