using System;

namespace WebVella.ERP.Storage
{
    public interface IStorageLookupRelationField : IStorageField
    {
        Guid RelatedEntityId { get; set; }
    }
}