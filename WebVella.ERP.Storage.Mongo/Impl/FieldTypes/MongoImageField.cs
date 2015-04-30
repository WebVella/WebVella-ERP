using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoImageField : MongoBaseField, IStorageImageField
    {
        public new string DefaultValue { get; set; }

        public string TargetEntityType { get; set; }

        public string RelationshipName { get; set; }
    }
}