using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoMultiLineTextField : MongoBaseField, IStorageMultiLineTextField
    {
        public string DefaultValue { get; set; }

        public int? MaxLength { get; set; }

        public int? VisibleLineNumber { get; set; }
    }
}