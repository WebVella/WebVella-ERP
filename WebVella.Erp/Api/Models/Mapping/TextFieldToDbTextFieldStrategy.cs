using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class TextFieldToDbTextFieldStrategy : IMapStrategy<TextField, DbTextField>
{
	public DbTextField Map(TextField source)
	{
		if (source == null)
			return null;

		var dest = new DbTextField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		return dest;
	}
}
