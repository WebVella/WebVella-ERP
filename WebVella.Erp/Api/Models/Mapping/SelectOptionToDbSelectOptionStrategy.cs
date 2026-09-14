using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class SelectOptionToDbSelectOptionStrategy : IMapStrategy<SelectOption, DbSelectOption>
{
	public DbSelectOption Map(SelectOption source)
	{
		if (source == null)
			return null;

		return new DbSelectOption
		{
			Value = source.Value,
			Label = source.Label,
			IconClass = source.IconClass,
			Color = source.Color
		};
	}
}
