namespace WebVella.Erp.Api.Models.Mapping;

internal class InputImageFieldToImageFieldStrategy : IMapStrategy<InputImageField, ImageField>
{
	public ImageField Map(InputImageField source)
	{
		if (source == null)
			return null;

		var dest = new ImageField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		return dest;
	}
}
