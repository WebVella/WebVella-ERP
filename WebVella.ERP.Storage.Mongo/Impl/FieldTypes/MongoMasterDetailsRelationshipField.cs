using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoMasterDetailsRelationshipField : MongoBaseField, IStorageMasterDetailsRelationshipField
    {
        public Guid RelatedEntityId { get; set; }
    }
}