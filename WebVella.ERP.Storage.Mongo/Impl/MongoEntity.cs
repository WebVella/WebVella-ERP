using System;
using MongoDB.Bson.Serialization.Attributes;

namespace WebVella.ERP.Storage.Mongo
{ 
    public class MongoEntity : IEntity
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}