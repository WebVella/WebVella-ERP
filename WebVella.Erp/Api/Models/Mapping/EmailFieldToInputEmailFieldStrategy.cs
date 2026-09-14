namespace WebVella.Erp.Api.Models.Mapping;

internal class EmailFieldToInputEmailFieldStrategy : IMapStrategy<EmailField, InputEmailField>
{
	public InputEmailField Map(EmailField source)
	{
		if (source == null)
			return null;

		var dest = new InputEmailField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		return dest;
	}
}
