using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoLookupRelationField : MongoBaseField, IStorageLookupRelationField
    {
        public Guid RelatedEntityId { get; set; }
    }
}