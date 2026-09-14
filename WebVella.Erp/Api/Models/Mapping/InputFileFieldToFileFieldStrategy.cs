namespace WebVella.Erp.Api.Models.Mapping;

internal class InputFileFieldToFileFieldStrategy : IMapStrategy<InputFileField, FileField>
{
	public FileField Map(InputFileField source)
	{
		if (source == null)
			return null;

		var dest = new FileField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		return dest;
	}
}
