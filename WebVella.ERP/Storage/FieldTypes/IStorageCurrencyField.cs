using WebVella.ERP.Api;

namespace WebVella.ERP.Storage
{
    public interface IStorageCurrencyField : IStorageField
    {
        decimal? DefaultValue { get; set; }

        decimal? MinValue { get; set; }

        decimal? MaxValue { get; set; }

        CurrencyType Currency { get; set; }
    }
}