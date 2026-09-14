using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbCurrencyFieldToCurrencyFieldStrategy : IMapStrategy<DbCurrencyField, CurrencyField>
{
	public CurrencyField Map(DbCurrencyField source)
	{
		if (source == null)
			return null;

		var dest = new CurrencyField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MinValue = source.MinValue;
		dest.MaxValue = source.MaxValue;

		dest.Currency = source.Currency == null
			? null
			: StrategyRegistry.GetStrategy<DbCurrencyType, CurrencyType>().Map(source.Currency);

		return dest;
	}
}
