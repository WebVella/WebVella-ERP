using System;

namespace WebVella.ERP.Storage
{
    public interface IStorageDateTimeField : IStorageField
    {
        new DateTime DefaultValue { get; set; }

        string Format { get; set; }
    }
}