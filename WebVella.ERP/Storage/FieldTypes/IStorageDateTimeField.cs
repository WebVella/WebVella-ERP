using System;

namespace WebVella.ERP.Storage
{
    public interface IStorageDateTimeField : IStorageField
    {
        DateTime DefaultValue { get; set; }

        string Format { get; set; }
    }
}