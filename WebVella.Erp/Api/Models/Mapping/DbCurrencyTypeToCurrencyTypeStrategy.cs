using WebVella.Erp.Api;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbCurrencyTypeToCurrencyTypeStrategy : IMapStrategy<DbCurrencyType, CurrencyType>
{
	public CurrencyType Map(DbCurrencyType source)
	{
		if (source == null)
			return null;

		return new CurrencyType
		{
			Symbol = source.Symbol,
			SymbolNative = source.SymbolNative,
			Name = source.Name,
			NamePlural = source.NamePlural,
			Code = source.Code,
			DecimalDigits = source.DecimalDigits,
			Rounding = source.Rounding,
			SymbolPlacement = source.SymbolPlacement
		};
	}
}
