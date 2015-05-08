using System;

namespace WebVella.ERP.Storage
{
    public interface IStoragePrimaryKeyField : IStorageField
    {
        Guid DefaultValue { get; set; }
    }
}