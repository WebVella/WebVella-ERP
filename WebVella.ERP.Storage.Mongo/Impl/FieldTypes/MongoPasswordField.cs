using WebVella.ERP.Storage;
using WebVella.ERP.Api;
using MongoDB.Bson.Serialization.Attributes;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoPasswordField : MongoBaseField, IStoragePasswordField
    {
		[BsonElement("maxLength")]
		public int? MaxLength { get; set; }

		[BsonElement("minLength")]
		public int? MinLength { get; set; }

		[BsonElement("encrypted")]
		public bool Encrypted { get; set; }
    }
}