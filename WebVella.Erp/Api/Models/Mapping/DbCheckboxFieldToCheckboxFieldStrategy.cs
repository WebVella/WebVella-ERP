using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbCheckboxFieldToCheckboxFieldStrategy : IMapStrategy<DbCheckboxField, CheckboxField>
{
	public CheckboxField Map(DbCheckboxField source)
	{
		if (source == null)
			return null;

		var dest = new CheckboxField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		return dest;
	}
}
