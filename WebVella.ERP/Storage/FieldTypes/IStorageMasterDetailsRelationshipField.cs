using System;

namespace WebVella.ERP.Storage
{
    public interface IStorageMasterDetailsRelationshipField : IStorageField
    {
        Guid RelatedEntityId { get; set; }
    }
}