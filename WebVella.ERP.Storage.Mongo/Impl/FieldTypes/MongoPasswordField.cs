using WebVella.ERP.Storage;
using WebVella.ERP.Api;

namespace WebVella.ERP.Core
{
    public class MongoPasswordField : MongoBaseField, IStoragePasswordField
    {
        public new string DefaultValue { get; set; }

        public int MaxLength { get; set; }

        public PasswordFieldMaskTypes MaskType { get; set; }

        public char MaskCharacter { get; set; }
    }
}