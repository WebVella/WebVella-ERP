using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoHtmlField : MongoBaseField, IStorageHtmlField
    {
        public string DefaultValue { get; set; }
    }
}