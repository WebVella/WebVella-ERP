using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WebVella.ERP.Storage.Mongo
{
    internal class MongoEntityRelationOptions : IStorageEntityRelationOptions
    {
        [BsonElement("relationId")]
        public Guid? RelationId { get; set; }

        [BsonElement("relationName")]
        public string RelationName { get; set; }

        [BsonElement("direction")]
        public string Direction { get; set; }
    }
}