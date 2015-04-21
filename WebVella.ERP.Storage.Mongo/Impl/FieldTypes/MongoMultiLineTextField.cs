using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoMultiLineTextField : MongoBaseField, IStorageMultiLineTextField
    {
        public new string DefaultValue { get; set; }

        public int LineNumber { get; set; }

        public string Value { get; set; }
    }
}