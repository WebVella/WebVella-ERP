using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoEmailField : MongoBaseField, IStorageEmailField
    {
        public string DefaultValue { get; set; }

        public int MaxLength { get; set; }
    }
}