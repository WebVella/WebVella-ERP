using WebVella.ERP.Storage;


namespace WebVella.ERP.Core
{
    public class MongoCheckboxField : MongoBaseField, IStorageCheckboxField
    {
        public new bool DefaultValue { get; set; }
    }
}