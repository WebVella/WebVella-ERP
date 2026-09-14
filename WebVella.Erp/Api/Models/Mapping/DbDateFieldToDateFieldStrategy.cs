using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbDateFieldToDateFieldStrategy : IMapStrategy<DbDateField, DateField>
{
	public DateField Map(DbDateField source)
	{
		if (source == null)
			return null;

		var dest = new DateField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Format = source.Format;
		dest.UseCurrentTimeAsDefaultValue = source.UseCurrentTimeAsDefaultValue;
		return dest;
	}
}
