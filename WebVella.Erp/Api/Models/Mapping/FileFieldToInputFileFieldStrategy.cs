namespace WebVella.Erp.Api.Models.Mapping;

internal class FileFieldToInputFileFieldStrategy : IMapStrategy<FileField, InputFileField>
{
	public InputFileField Map(FileField source)
	{
		if (source == null)
			return null;

		var dest = new InputFileField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		return dest;
	}
}
