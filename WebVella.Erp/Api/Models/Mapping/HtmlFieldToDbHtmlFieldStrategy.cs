using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class HtmlFieldToDbHtmlFieldStrategy : IMapStrategy<HtmlField, DbHtmlField>
{
	public DbHtmlField Map(HtmlField source)
	{
		if (source == null)
			return null;

		var dest = new DbHtmlField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);

		dest.DefaultValue = source.DefaultValue;

		return dest;
	}
}
