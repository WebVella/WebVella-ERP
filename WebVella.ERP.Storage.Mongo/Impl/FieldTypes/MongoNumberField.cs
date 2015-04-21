using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoNumberField : MongoBaseField, IStorageNumberField
    {
        public new decimal DefaultValue { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public decimal Value { get; set; }
    }
}