namespace WebVella.Erp.Api.Models.Mapping;

internal class MultiLineTextFieldToInputMultiLineTextFieldStrategy : IMapStrategy<MultiLineTextField, InputMultiLineTextField>
{
	public InputMultiLineTextField Map(MultiLineTextField source)
	{
		if (source == null)
			return null;

		var dest = new InputMultiLineTextField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		dest.VisibleLineNumber = source.VisibleLineNumber;
		return dest;
	}
}
