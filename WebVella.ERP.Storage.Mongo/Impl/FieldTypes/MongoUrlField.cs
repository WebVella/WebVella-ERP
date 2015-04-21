using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoUrlField : MongoBaseField, IStorageUrlField
    {
        public new string DefaultValue { get; set; }

        public int MaxLength { get; set; }

        public bool OpenTargetInNewWindow { get; set; }

        public string Value { get; set; }
    }
}
