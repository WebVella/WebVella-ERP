using MongoDB.Bson.Serialization.Attributes;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoNumberField : MongoBaseField, IStorageNumberField
    {
		[BsonElement("defaultValue")]
		public decimal? DefaultValue { get; set; }

		[BsonElement("minValue")]
		public decimal? MinValue { get; set; }

		[BsonElement("maxValue")]
		public decimal? MaxValue { get; set; }

		[BsonElement("decimalPlaces")]
		public byte DecimalPlaces { get; set; }
    }
}