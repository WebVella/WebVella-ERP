using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoAutoNumberField : MongoBaseField, IStorageAutoNumberField
    {
        public decimal DefaultValue { get; set; }

        public string DisplayFormat { get; set; }

        public decimal StartingNumber { get; set; }
    }
}