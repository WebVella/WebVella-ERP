using MongoDB.Bson.Serialization.Attributes;
using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoGuidField : MongoBaseField, IStorageGuidField
    {
		[BsonElement("defaultValue")]
		public Guid? DefaultValue { get; set; }

		[BsonElement("generateNewId")]
		public bool? GenerateNewId { get; set; }
    }
}