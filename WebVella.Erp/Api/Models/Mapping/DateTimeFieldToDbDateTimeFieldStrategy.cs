using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DateTimeFieldToDbDateTimeFieldStrategy : IMapStrategy<DateTimeField, DbDateTimeField>
{
	public DbDateTimeField Map(DateTimeField source)
	{
		if (source == null)
			return null;

		var dest = new DbDateTimeField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Format = source.Format;
		dest.UseCurrentTimeAsDefaultValue = source.UseCurrentTimeAsDefaultValue ?? false;
		return dest;
	}
}
