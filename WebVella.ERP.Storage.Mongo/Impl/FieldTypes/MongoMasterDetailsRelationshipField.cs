using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoMasterDetailsRelationshipField : MongoBaseField, IStorageMasterDetailsRelationshipField
    {
        public Guid RelatedEntityId { get; set; }
    }
}