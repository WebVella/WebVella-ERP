using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoSystemSettings : MongoDocumentBase, IStorageSystemSettings
    {
		[BsonElement("version")]
		public int Version { get; set; }
    }
}
