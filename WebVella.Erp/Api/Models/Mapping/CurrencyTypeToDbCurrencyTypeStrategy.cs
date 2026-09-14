using WebVella.Erp.Api;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class CurrencyTypeToDbCurrencyTypeStrategy : IMapStrategy<CurrencyType, DbCurrencyType>
{
	public DbCurrencyType Map(CurrencyType source)
	{
		if (source == null)
			return null;

		return new DbCurrencyType
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
