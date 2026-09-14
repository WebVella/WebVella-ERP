using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbPercentFieldToPercentFieldStrategy : IMapStrategy<DbPercentField, PercentField>
{
	public PercentField Map(DbPercentField source)
	{
		if (source == null)
			return null;

		var dest = new PercentField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MinValue = source.MinValue;
		dest.MaxValue = source.MaxValue;
		dest.DecimalPlaces = source.DecimalPlaces;
		return dest;
	}
}
