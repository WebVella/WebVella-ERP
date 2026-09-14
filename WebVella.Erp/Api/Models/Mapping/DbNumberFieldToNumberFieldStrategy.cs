using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbNumberFieldToNumberFieldStrategy : IMapStrategy<DbNumberField, NumberField>
{
	public NumberField Map(DbNumberField source)
	{
		if (source == null)
			return null;

		var dest = new NumberField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MinValue = source.MinValue;
		dest.MaxValue = source.MaxValue;
		dest.DecimalPlaces = source.DecimalPlaces;
		return dest;
	}
}
