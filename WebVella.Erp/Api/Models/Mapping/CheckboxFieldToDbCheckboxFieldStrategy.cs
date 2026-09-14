using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class CheckboxFieldToDbCheckboxFieldStrategy : IMapStrategy<CheckboxField, DbCheckboxField>
{
	public DbCheckboxField Map(CheckboxField source)
	{
		if (source == null)
			return null;

		var dest = new DbCheckboxField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue ?? false;
		return dest;
	}
}
