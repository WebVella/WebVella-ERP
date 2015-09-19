using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;


namespace WebVella.ERP.Storage.Mongo
{
    public class MongoFieldPermissions : IStorageFieldPermissions
    {
        [BsonElement("canRead")]
        public List<Guid> CanRead { get; set; }

        [BsonElement("canUpdate")]
        public List<Guid> CanUpdate { get; set; }

        public MongoFieldPermissions()
        {
            CanRead = new List<Guid>();
            CanUpdate = new List<Guid>();
        }
    }
}