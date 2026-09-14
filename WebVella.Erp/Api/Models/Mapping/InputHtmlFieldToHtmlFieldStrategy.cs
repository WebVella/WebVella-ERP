namespace WebVella.Erp.Api.Models.Mapping;

internal class InputHtmlFieldToHtmlFieldStrategy : IMapStrategy<InputHtmlField, HtmlField>
{
	public HtmlField Map(InputHtmlField source)
	{
		if (source == null)
			return null;

		var dest = new HtmlField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		return dest;
	}
}
