namespace WebVella.Erp.Api.Models.Mapping;

internal class InputCheckboxFieldToCheckboxFieldStrategy : IMapStrategy<InputCheckboxField, CheckboxField>
{
	public CheckboxField Map(InputCheckboxField source)
	{
		if (source == null)
			return null;

		var dest = new CheckboxField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		return dest;
	}
}
