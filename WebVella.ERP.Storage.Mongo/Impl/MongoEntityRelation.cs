using System;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage.Mongo
{
    internal class MongoEntityRelation : MongoDocumentBase, IStorageEntityRelation
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        public bool System { get; set; }

        public EntityRelationType RelationType { get; set; }

        public Guid OriginEntityId { get; set; }

        public Guid OriginFieldId { get; set; }

        public Guid TargetEntityId { get; set; }

        public Guid TargetFieldId { get; set; }
    }
}