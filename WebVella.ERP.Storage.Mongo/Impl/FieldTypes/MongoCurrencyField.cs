using WebVella.ERP.Storage;
using WebVella.ERP.Api;
using MongoDB.Bson.Serialization.Attributes;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoCurrencyField : MongoBaseField, IStorageCurrencyField
    {
		[BsonElement("defaultValue")]
		public decimal? DefaultValue { get; set; }

		[BsonElement("minValue")]
		public decimal? MinValue { get; set; }

		[BsonElement("maxValue")]
		public decimal? MaxValue { get; set; }

		[BsonElement("currency")]
		public CurrencyType Currency { get; set; }
    }
}