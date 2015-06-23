using MongoDB.Bson.Serialization.Attributes;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoHtmlField : MongoBaseField, IStorageHtmlField
    {
		[BsonElement("defaultValue")]
		public string DefaultValue { get; set; }
    }
}