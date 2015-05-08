using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoFileField : MongoBaseField, IStorageFileField
    {
        public string DefaultValue { get; set; }
    }
}