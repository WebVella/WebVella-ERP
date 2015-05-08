using WebVella.ERP.Storage;


namespace WebVella.ERP.Core
{
    public class MongoCheckboxField : MongoBaseField, IStorageCheckboxField
    {
        public bool DefaultValue { get; set; }
    }
}