using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbSelectOptionToSelectOptionStrategy : IMapStrategy<DbSelectOption, SelectOption>
{
	public SelectOption Map(DbSelectOption source)
	{
		if (source == null)
			return null;

		return new SelectOption
		{
			Value = source.Value,
			Label = source.Label,
			IconClass = source.IconClass,
			Color = source.Color
		};
	}
}
