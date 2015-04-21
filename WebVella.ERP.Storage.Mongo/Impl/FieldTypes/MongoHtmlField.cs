using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoHtmlField : MongoBaseField, IStorageHtmlField
    {
        public new string DefaultValue { get; set; }

        public string Value { get; set; }
    }
}