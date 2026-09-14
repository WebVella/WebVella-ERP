namespace WebVella.Erp.Api.Models.Mapping;

internal class InputMultiLineTextFieldToMultiLineTextFieldStrategy : IMapStrategy<InputMultiLineTextField, MultiLineTextField>
{

	public MultiLineTextField Map(InputMultiLineTextField source)
	{
		if (source == null)
			return null;

		var dest = new MultiLineTextField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		dest.VisibleLineNumber = source.VisibleLineNumber;
		return dest;
	}
}
