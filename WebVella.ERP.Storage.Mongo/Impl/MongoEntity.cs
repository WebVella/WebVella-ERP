using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoEntity : MongoDocumentBase, IEntity
    {
        public string Name { get; set; }

        public bool System { get; set; }

        public List<IField> Fields { get; set; }
    }
}