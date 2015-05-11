using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    internal class MongoEntity : MongoDocumentBase, IStorageEntity
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public string PluralLabel { get; set; }

        public bool System { get; set; }

        public IStorageEntityPermissions Permissions { get; set; }

        public List<IStorageField> Fields { get; set; }

        public List<IStorageView> Views { get; set; }

        public List<IStorageForm> Forms { get; set; }

        public MongoEntity()
        {
            Fields = new List<IStorageField>();
            Views = new List<IStorageView>();
            Forms = new List<IStorageForm>();
        }
    }

    internal class MongoEntityPermissions : IStorageEntityPermissions
    {
        public List<Guid> CanRead { get; set; }

        public List<Guid> CanUpdate { get; set; }

        public List<Guid> CanDelete { get; set; }

        public MongoEntityPermissions()
        {
            CanRead = new List<Guid>();
            CanUpdate = new List<Guid>();
            CanDelete = new List<Guid>();
        }
    }
}