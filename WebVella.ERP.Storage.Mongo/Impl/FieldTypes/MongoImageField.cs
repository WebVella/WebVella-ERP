using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoImageField : MongoBaseField, IStorageImageField
    {
        public string DefaultValue { get; set; }
    }
}