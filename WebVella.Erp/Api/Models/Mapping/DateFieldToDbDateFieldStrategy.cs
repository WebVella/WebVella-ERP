using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DateFieldToDbDateFieldStrategy : IMapStrategy<DateField, DbDateField>
{
	public DbDateField Map(DateField source)
	{
		if (source == null)
			return null;

		var dest = new DbDateField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Format = source.Format;
		dest.UseCurrentTimeAsDefaultValue = source.UseCurrentTimeAsDefaultValue ?? false;
		return dest;
	}
}
