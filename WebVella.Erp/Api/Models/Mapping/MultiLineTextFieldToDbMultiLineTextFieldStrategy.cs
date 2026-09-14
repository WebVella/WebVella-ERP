using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class MultiLineTextFieldToDbMultiLineTextFieldStrategy : IMapStrategy<MultiLineTextField, DbMultiLineTextField>
{
	public DbMultiLineTextField Map(MultiLineTextField source)
	{
		if (source == null)
			return null;

		var dest = new DbMultiLineTextField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		dest.VisibleLineNumber = source.VisibleLineNumber;
		return dest;
	}
}
