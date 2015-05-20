using WebVella.ERP.Storage;
using WebVella.ERP.Api;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoPasswordField : MongoBaseField, IStoragePasswordField
    {
        public int? MaxLength { get; set; }

        public int? MinLength { get; set; }

        public bool Encrypted { get; set; }
    }
}