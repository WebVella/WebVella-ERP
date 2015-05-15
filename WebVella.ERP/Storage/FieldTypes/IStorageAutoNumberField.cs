using System;

namespace WebVella.ERP.Storage
{
    public interface IStorageAutoNumberField : IStorageField
    {
        decimal? DefaultValue { get; set; }

        string DisplayFormat { get; set; }

        decimal? StartingNumber { get; set; }
    }
}