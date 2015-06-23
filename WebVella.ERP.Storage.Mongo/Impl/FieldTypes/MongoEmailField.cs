using MongoDB.Bson.Serialization.Attributes;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoEmailField : MongoBaseField, IStorageEmailField
    {
		[BsonElement("defaultValue")]
		public string DefaultValue { get; set; }

		[BsonElement("maxLength")]
		public int? MaxLength { get; set; }
    }
}