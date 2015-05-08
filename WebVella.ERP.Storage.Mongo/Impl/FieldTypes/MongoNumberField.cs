using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoNumberField : MongoBaseField, IStorageNumberField
    {
        public decimal DefaultValue { get; set; }

        public decimal MinValue { get; set; }

        public decimal MaxValue { get; set; }

        public byte DecimalPlaces { get; set; }
    }
}