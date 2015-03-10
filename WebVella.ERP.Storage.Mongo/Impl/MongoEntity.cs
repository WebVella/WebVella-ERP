using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoEntity : IEntity
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<IField> Fields { get; set; }
    }
}