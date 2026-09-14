namespace WebVella.Erp.Api.Models.Mapping;

internal class ImageFieldToInputImageFieldStrategy : IMapStrategy<ImageField, InputImageField>
{
	public InputImageField Map(ImageField source)
	{
		if (source == null)
			return null;

		var dest = new InputImageField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		return dest;
	}
}
