using WebVella.ERP.Storage;
using WebVella.ERP.Api;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoPasswordField : MongoBaseField, IStoragePasswordField
    {
        public int MaxLength { get; set; }

        public bool Encrypted { get; set; }

        public PasswordFieldMaskTypes MaskType { get; set; }

        public char MaskCharacter { get; set; }
    }
}