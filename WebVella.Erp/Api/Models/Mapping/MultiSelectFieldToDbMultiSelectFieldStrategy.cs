using System.Linq;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class MultiSelectFieldToDbMultiSelectFieldStrategy : IMapStrategy<MultiSelectField, DbMultiSelectField>
{
	public DbMultiSelectField Map(MultiSelectField source)
	{
		if (source == null)
			return null;

		var dest = new DbMultiSelectField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Options = source.Options?.Select(o => StrategyRegistry.GetStrategy<SelectOption, DbSelectOption>().Map(o)).ToList();
		return dest;
	}
}
