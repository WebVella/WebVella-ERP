namespace WebVella.Erp.Api.Models.Mapping;

internal class InputUrlFieldToUrlFieldStrategy : IMapStrategy<InputUrlField, UrlField>
{
	public UrlField Map(InputUrlField source)
	{
		if (source == null)
			return null;

		var dest = new UrlField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		dest.OpenTargetInNewWindow = source.OpenTargetInNewWindow;
		return dest;
	}
}
