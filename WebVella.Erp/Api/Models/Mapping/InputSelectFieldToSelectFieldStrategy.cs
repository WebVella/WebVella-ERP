using System.Linq;

namespace WebVella.Erp.Api.Models.Mapping;

internal class InputSelectFieldToSelectFieldStrategy : IMapStrategy<InputSelectField, SelectField>
{
	public SelectField Map(InputSelectField source)
	{
		if (source == null)
			return null;

		var dest = new SelectField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Options = source.Options?.Select(o => new SelectOption
		{
			Value = o.Value,
			Label = o.Label,
			IconClass = o.IconClass,
			Color = o.Color
		}).ToList();
		return dest;
	}
}
