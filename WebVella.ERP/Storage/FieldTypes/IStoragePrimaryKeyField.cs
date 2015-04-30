using System;

namespace WebVella.ERP.Storage
{
    public interface IStoragePrimaryKeyField : IStorageField
    {
        new Guid DefaultValue { get; set; }
    }
}