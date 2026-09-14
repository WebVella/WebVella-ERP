using System.Linq;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class SelectFieldToDbSelectFieldStrategy : IMapStrategy<SelectField, DbSelectField>
{
	public DbSelectField Map(SelectField source)
	{
		if (source == null)
			return null;

		var dest = new DbSelectField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Options = source.Options?.Select(o => StrategyRegistry.GetStrategy<SelectOption, DbSelectOption>().Map(o)).ToList();
		return dest;
	}
}
