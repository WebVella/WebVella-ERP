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
		public IStorageCurrencyType Currency { get; set; }
    }

	public class MongoCurrencyType : IStorageCurrencyType
	{
		[BsonElement("symbol")]
		public string Symbol { get; set; }

		[BsonElement("symbolNative")]
		public string SymbolNative { get; set; }

		[BsonElement("name")]
		public string Name { get; set; }

		[BsonElement("namePlural")]
		public string NamePlural { get; set; }

		[BsonElement("code")]
		public string Code { get; set; }

		[BsonElement("decimalDigits")]
		public int DecimalDigits { get; set; }

		[BsonElement("rounding")]
		public int Rounding { get; set; }

		[BsonElement("symbolPlacement")]
		public CurrencySymbolPlacement SymbolPlacement { get; set; }
	}
}