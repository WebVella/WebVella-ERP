using MongoDB.Bson.Serialization.Attributes;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoImageField : MongoBaseField, IStorageImageField
    {
		[BsonElement("defaultValue")]
		public string DefaultValue { get; set; }
    }
}