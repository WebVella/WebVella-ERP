using System.Linq;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbSelectFieldToSelectFieldStrategy : IMapStrategy<DbSelectField, SelectField>
{
	public SelectField Map(DbSelectField source)
	{
		if (source == null)
			return null;

		var dest = new SelectField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;

		dest.Options = source.Options
			?.Select(o => StrategyRegistry.GetStrategy<DbSelectOption, SelectOption>()
			.Map(o))
			.ToList();

		return dest;
	}
}
