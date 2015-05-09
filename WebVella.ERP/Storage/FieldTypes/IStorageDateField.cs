using System;

namespace WebVella.ERP.Storage
{
    public interface IStorageDateField : IStorageField
    {
        DateTime DefaultValue { get; set; }

        string Format { get; set; }
    }
}