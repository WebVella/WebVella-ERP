using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoHtmlField : MongoBaseField, IStorageHtmlField
    {
        public string DefaultValue { get; set; }
    }
}