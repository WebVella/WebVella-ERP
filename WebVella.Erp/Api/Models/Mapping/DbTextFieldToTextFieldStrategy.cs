using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbTextFieldToTextFieldStrategy : IMapStrategy<DbTextField, TextField>
{
	public TextField Map(DbTextField source)
	{
		if (source == null)
			return null;

		var dest = new TextField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		return dest;
	}
}
