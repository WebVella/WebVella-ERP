using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoEntity : MongoDocumentBase, IStorageEntity
    {
        public string Name { get; set; }

        public bool System { get; set; }

        public List<IStorageField> Fields { get; set; }

        public List<IStorageView> Views { get; set; }

        public List<IStorageView> Forms { get; set; }
    }
}