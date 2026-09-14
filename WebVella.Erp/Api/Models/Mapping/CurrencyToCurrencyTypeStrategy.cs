using WebVella.Erp.Api;

namespace WebVella.Erp.Api.Models.Mapping;

internal class CurrencyToCurrencyTypeStrategy : IMapStrategy<Currency, CurrencyType>
{
	public CurrencyType Map(Currency source)
	{
		if (source == null)
			return null;

		var decimalDigits = 2;
		if (source.SubUnitToUnit == 1000)
			decimalDigits = 3;

		var symbol = source.IsoCode.ToUpperInvariant();
		if (source.AlternateSymbols.Count > 0)
			symbol = source.AlternateSymbols[0];

		var symPlacement = CurrencySymbolPlacement.After;
		if (source.SymbolFirst)
			symPlacement = CurrencySymbolPlacement.Before;

		return new CurrencyType()
		{
			Code = source.IsoCode.ToUpperInvariant(),
			DecimalDigits = decimalDigits,
			Name = source.Name,
			NamePlural = source.Name,
			Rounding = 0,
			SymbolNative = source.Symbol,
			Symbol = symbol,
			SymbolPlacement = symPlacement
		};
	}
}
