namespace WebVella.Erp.Api.Models.Mapping;

internal class CurrencyFieldToInputCurrencyFieldStrategy : IMapStrategy<CurrencyField, InputCurrencyField>
{
	public InputCurrencyField Map(CurrencyField source)
	{
		if (source == null)
			return null;

		var dest = new InputCurrencyField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);

		dest.DefaultValue = source.DefaultValue;
		dest.MinValue = source.MinValue;
		dest.MaxValue = source.MaxValue;
		dest.Currency = source.Currency;
		return dest;
	}
}
