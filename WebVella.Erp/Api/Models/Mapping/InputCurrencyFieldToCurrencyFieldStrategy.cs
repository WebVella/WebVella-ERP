namespace WebVella.Erp.Api.Models.Mapping;

internal class InputCurrencyFieldToCurrencyFieldStrategy : IMapStrategy<InputCurrencyField, CurrencyField>
{
	public CurrencyField Map(InputCurrencyField source)
	{
		if (source == null)
			return null;

		var dest = new CurrencyField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MinValue = source.MinValue;
		dest.MaxValue = source.MaxValue;
		dest.Currency = source.Currency;
		return dest;
	}
}
