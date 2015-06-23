using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoMultiSelectField : MongoBaseField, IStorageMultiSelectField
    {
		[BsonElement("defaultValue")]
		public IEnumerable<string> DefaultValue { get; set; }

		[BsonElement("options")]
		public IList<IStorageMultiSelectFieldOption> Options { get; set; }
    }

    public class MongoMultiSelectFieldOption : IStorageMultiSelectFieldOption
    {
		[BsonElement("key")]
		public string Key { get; set; }

		[BsonElement("value")]
		public string Value { get; set; }
    }
}