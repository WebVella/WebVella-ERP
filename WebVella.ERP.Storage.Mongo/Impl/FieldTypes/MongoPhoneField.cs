using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoPhoneField : MongoBaseField, IStoragePhoneField
    {
        public new string DefaultValue { get; set; }

        public string Format { get; set; }

        public int MaxLength { get; set; }

        public string Value { get; set; }
    }
}