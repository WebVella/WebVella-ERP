namespace WebVella.Erp.Api.Models.Mapping;

internal class TextFieldToInputTextFieldStrategy : IMapStrategy<TextField, InputTextField>
{
	public InputTextField Map(TextField source)
	{
		if (source == null)
			return null;

		var dest = new InputTextField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		return dest;
	}
}
