using System.Collections.Generic;
using System.Linq;

namespace WebVella.Erp.Api.Models.Mapping;

internal class MultiSelectFieldToInputMultiSelectFieldStrategy : IMapStrategy<MultiSelectField, InputMultiSelectField>
{
	public InputMultiSelectField Map(MultiSelectField source)
	{
		if (source == null)
			return null;

		var dest = new InputMultiSelectField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Options = source.Options?.ToList();
		return dest;
	}
}
