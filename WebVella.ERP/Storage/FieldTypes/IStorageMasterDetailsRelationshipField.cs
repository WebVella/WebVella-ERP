using System;

namespace WebVella.ERP.Storage
{
    public interface IStorageMasterDetailsRelationshipField : IStorageField
    {
        new string DefaultValue { get; set; }

        Guid Value { get; set; }
    }
}