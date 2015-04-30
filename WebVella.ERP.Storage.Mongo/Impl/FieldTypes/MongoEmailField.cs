using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoEmailField : MongoBaseField, IStorageEmailField
    {
        public new string DefaultValue { get; set; }

        public int MaxLength { get; set; }
    }
}