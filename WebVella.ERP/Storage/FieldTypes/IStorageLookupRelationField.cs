using System;

namespace WebVella.ERP.Storage
{
    public interface IStorageLookupRelationField : IStorageField
    {
        new string DefaultValue { get; set; }

        Guid Value { get; set; }
    }
}