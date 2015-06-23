using MongoDB.Bson.Serialization.Attributes;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoPhoneField : MongoBaseField, IStoragePhoneField
    {
		[BsonElement("defaultValue")]
		public string DefaultValue { get; set; }

		[BsonElement("format")]
		public string Format { get; set; }

		[BsonElement("maxLength")]
		public int? MaxLength { get; set; }
    }
}