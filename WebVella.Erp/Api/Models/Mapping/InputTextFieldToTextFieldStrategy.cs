namespace WebVella.Erp.Api.Models.Mapping;

internal class InputTextFieldToTextFieldStrategy : IMapStrategy<InputTextField, TextField>
{
	public TextField Map(InputTextField source)
	{
		if (source == null)
			return null;

		var dest = new TextField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		return dest;
	}
}
