namespace WebVella.Erp.Api.Models.Mapping;

internal class UrlFieldToInputUrlFieldStrategy : IMapStrategy<UrlField, InputUrlField>
{
	public InputUrlField Map(UrlField source)
	{
		if (source == null)
			return null;

		var dest = new InputUrlField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		dest.OpenTargetInNewWindow = source.OpenTargetInNewWindow;
		return dest;
	}
}
