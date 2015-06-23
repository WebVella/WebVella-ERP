using WebVella.ERP.Api;

namespace WebVella.ERP.Storage
{
    public interface IStorageCurrencyField : IStorageField
    {
        decimal? DefaultValue { get; set; }

        decimal? MinValue { get; set; }

        decimal? MaxValue { get; set; }

		IStorageCurrencyType Currency { get; set; }
    }

	public interface IStorageCurrencyType
	{
		string Symbol { get; set; }

		string SymbolNative { get; set; }

		string Name { get; set; }

		string NamePlural { get; set; }

		string Code { get; set; }

		int DecimalDigits { get; set; }

		int Rounding { get; set; }

		CurrencySymbolPlacement SymbolPlacement { get; set; }
	}
}