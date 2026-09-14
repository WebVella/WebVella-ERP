using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class PercentFieldToDbPercentFieldStrategy : IMapStrategy<PercentField, DbPercentField>
{
	public DbPercentField Map(PercentField source)
	{
		if (source == null)
			return null;

		var dest = new DbPercentField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MinValue = source.MinValue;
		dest.MaxValue = source.MaxValue;
		dest.DecimalPlaces = source.DecimalPlaces ?? 2;
		return dest;
	}
}
