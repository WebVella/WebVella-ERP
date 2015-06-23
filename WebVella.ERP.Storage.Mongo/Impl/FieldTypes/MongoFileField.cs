using MongoDB.Bson.Serialization.Attributes;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoFileField : MongoBaseField, IStorageFileField
    {
		[BsonElement("defaultValue")]
		public string DefaultValue { get; set; }
    }
}