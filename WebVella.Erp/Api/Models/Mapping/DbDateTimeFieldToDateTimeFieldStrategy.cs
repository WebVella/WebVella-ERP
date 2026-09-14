using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbDateTimeFieldToDateTimeFieldStrategy : IMapStrategy<DbDateTimeField, DateTimeField>
{
	public DateTimeField Map(DbDateTimeField source)
	{
		if (source == null)
			return null;

		var dest = new DateTimeField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Format = source.Format;
		dest.UseCurrentTimeAsDefaultValue = source.UseCurrentTimeAsDefaultValue;
		return dest;
	}
}
