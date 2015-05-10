using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoFileField : MongoBaseField, IStorageFileField
    {
        public string DefaultValue { get; set; }
    }
}