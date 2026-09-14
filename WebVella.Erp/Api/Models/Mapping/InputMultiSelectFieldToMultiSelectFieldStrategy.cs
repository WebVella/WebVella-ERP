using System.Collections.Generic;
using System.Linq;

namespace WebVella.Erp.Api.Models.Mapping;

internal class InputMultiSelectFieldToMultiSelectFieldStrategy : IMapStrategy<InputMultiSelectField, MultiSelectField>
{
	public MultiSelectField Map(InputMultiSelectField source)
	{
		if (source == null)
			return null;

		var dest = new MultiSelectField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Options = source.Options?.ToList();
		return dest;
	}
}
