using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbMultiLineTextFieldToMultiLineTextFieldStrategy : IMapStrategy<DbMultiLineTextField, MultiLineTextField>
{
	public MultiLineTextField Map(DbMultiLineTextField source)
	{
		if (source == null)
			return null;

		var dest = new MultiLineTextField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		dest.VisibleLineNumber = source.VisibleLineNumber;
		return dest;
	}
}
