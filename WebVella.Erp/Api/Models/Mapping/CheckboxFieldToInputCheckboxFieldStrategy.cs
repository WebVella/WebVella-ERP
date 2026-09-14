namespace WebVella.Erp.Api.Models.Mapping;

internal class CheckboxFieldToInputCheckboxFieldStrategy : IMapStrategy<CheckboxField, InputCheckboxField>
{
	public InputCheckboxField Map(CheckboxField source)
	{
		if (source == null)
			return null;

		var dest = new InputCheckboxField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		return dest;
	}
}
