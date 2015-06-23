using MongoDB.Bson.Serialization.Attributes;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoAutoNumberField : MongoBaseField, IStorageAutoNumberField
    {
		[BsonElement("defaultValue")]
		public decimal? DefaultValue { get; set; }

		[BsonElement("DisplayFormat")]
		public string DisplayFormat { get; set; }

		[BsonElement("startingNumber")]
		public decimal? StartingNumber { get; set; }
    }
}