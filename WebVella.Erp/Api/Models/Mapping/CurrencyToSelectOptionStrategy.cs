namespace WebVella.Erp.Api.Models.Mapping;

internal class CurrencyToSelectOptionStrategy : IMapStrategy<Currency, SelectOption>
{
	public SelectOption Map(Currency source)
	{
		if (source == null)
			return null;

		return new SelectOption()
		{
			Value = source.IsoCode.ToUpperInvariant(),
			Label = source.Name
		};
	}
}
