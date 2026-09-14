using System.Collections.Generic;
using System.Linq;

namespace WebVella.Erp.Api.Models.Mapping;

internal class SelectFieldToInputSelectFieldStrategy : IMapStrategy<SelectField, InputSelectField>
{
	public InputSelectField Map(SelectField source)
	{
		if (source == null)
			return null;

		var dest = new InputSelectField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
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
