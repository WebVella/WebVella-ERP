using WebVella.ERP.Api;

namespace WebVella.ERP.Storage
{
    public interface IStorageCurrencyField : IStorageField
    {
        new decimal DefaultValue { get; set; }

        decimal MinValue { get; set; }

        decimal MaxValue { get; set; }

        CurrencyTypes Currency { get; set; }

        decimal Value { get; set; }
    }
}