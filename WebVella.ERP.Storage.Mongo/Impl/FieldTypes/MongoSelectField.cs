using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoSelectField : MongoBaseField, IStorageSelectField
    {
		[BsonElement("defaultValue")]
		public string DefaultValue { get; set; }

		[BsonElement("options")]
		public IList<IStorageSelectFieldOption> Options { get; set; }
    }

    public class MongoSelectFieldOption : IStorageSelectFieldOption
    {
		[BsonElement("key")]
		public string Key { get; set; }

		[BsonElement("value")]
		public string Value { get; set; }
    }
}