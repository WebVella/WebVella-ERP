using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class CurrencyFieldToDbCurrencyFieldStrategy : IMapStrategy<CurrencyField, DbCurrencyField>
{
	public DbCurrencyField Map(CurrencyField source)
	{
		if (source == null)
			return null;

		var dest = new DbCurrencyField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);

		dest.DefaultValue = source.DefaultValue;
		dest.MinValue = source.MinValue;
		dest.MaxValue = source.MaxValue;
		dest.Currency = source.Currency == null
			? null
			: StrategyRegistry.GetStrategy<CurrencyType, DbCurrencyType>().Map(source.Currency);
		return dest;
	}
}
