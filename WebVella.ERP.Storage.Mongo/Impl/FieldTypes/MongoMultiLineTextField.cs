using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoMultiLineTextField : MongoBaseField, IStorageMultiLineTextField
    {
        public new string DefaultValue { get; set; }

        public int MaxLength { get; set; }

        public int VisibleLineNumber { get; set; }
    }
}