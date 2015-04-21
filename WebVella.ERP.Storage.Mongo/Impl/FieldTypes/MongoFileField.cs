using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoFileField : MongoBaseField, IStorageFileField
    {
        public new string DefaultValue { get; set; }

        public string Value { get; set; }
    }
}