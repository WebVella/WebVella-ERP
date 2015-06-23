using MongoDB.Bson.Serialization.Attributes;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoMultiLineTextField : MongoBaseField, IStorageMultiLineTextField
    {
		[BsonElement("defaultValue")]
		public string DefaultValue { get; set; }

		[BsonElement("maxLength")]
		public int? MaxLength { get; set; }

		[BsonElement("visibleLineNumber")]
		public int? VisibleLineNumber { get; set; }
    }
}