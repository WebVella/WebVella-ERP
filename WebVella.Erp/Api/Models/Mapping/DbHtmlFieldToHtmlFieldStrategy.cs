using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbHtmlFieldToHtmlFieldStrategy : IMapStrategy<DbHtmlField, HtmlField>
{
	public HtmlField Map(DbHtmlField source)
	{
		if (source == null)
			return null;

		var dest = new HtmlField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		return dest;
	}
}
