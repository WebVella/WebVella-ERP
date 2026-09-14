namespace WebVella.Erp.Api.Models.Mapping;

internal class HtmlFieldToInputHtmlFieldStrategy : IMapStrategy<HtmlField, InputHtmlField>
{
	public InputHtmlField Map(HtmlField source)
	{
		if (source == null)
			return null;

		var dest = new InputHtmlField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		return dest;
	}
}
