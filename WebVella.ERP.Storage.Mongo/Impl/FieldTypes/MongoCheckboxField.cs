using MongoDB.Bson.Serialization.Attributes;
using WebVella.ERP.Storage;


namespace WebVella.ERP.Storage.Mongo
{
    public class MongoCheckboxField : MongoBaseField, IStorageCheckboxField
    {
		[BsonElement("defaultValue")]
		public bool DefaultValue { get; set; }
    }
}