namespace WebVella.Erp.Api.Models.Mapping;

internal class InputEmailFieldToEmailFieldStrategy : IMapStrategy<InputEmailField, EmailField>
{
	public EmailField Map(InputEmailField source)
	{
		if (source == null)
			return null;

		var dest = new EmailField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		return dest;
	}
}
