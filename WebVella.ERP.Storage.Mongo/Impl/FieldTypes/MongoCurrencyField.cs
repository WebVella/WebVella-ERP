using WebVella.ERP.Storage;
using WebVella.ERP.Api;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoCurrencyField : MongoBaseField, IStorageCurrencyField
    {
        public decimal DefaultValue { get; set; }

        public decimal MinValue { get; set; }

        public decimal MaxValue { get; set; }

        public CurrencyTypes Currency { get; set; }
    }
}