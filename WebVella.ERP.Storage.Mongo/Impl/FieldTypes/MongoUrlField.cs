using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoUrlField : MongoBaseField, IStorageUrlField
    {
        public string DefaultValue { get; set; }

        public int MaxLength { get; set; }

        public bool OpenTargetInNewWindow { get; set; }
    }
}
