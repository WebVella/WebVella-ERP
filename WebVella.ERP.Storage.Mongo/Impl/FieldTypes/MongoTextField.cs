using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoTextField : MongoBaseField, IStorageTextField
    {
        public string DefaultValue { get; set; }

        public int MaxLength { get; set; }
    }
}