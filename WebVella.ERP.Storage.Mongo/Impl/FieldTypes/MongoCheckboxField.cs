using WebVella.ERP.Storage;


namespace WebVella.ERP.Storage.Mongo
{
    public class MongoCheckboxField : MongoBaseField, IStorageCheckboxField
    {
        public bool DefaultValue { get; set; }
    }
}