using MongoDB.Bson.Serialization.Attributes;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoUrlField : MongoBaseField, IStorageUrlField
    {
		[BsonElement("defaultValue")]
		public string DefaultValue { get; set; }

		[BsonElement("maxLength")]
		public int? MaxLength { get; set; }

		[BsonElement("openTargetInNewWindow")]
		public bool OpenTargetInNewWindow { get; set; }
    }
}
