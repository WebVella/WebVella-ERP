using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoLookupRelationField : MongoBaseField, IStorageLookupRelationField
    {
        public Guid RelatedEntityId { get; set; }
    }
}