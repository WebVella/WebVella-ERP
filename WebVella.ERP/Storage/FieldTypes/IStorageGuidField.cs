using System;

namespace WebVella.ERP.Storage
{
    public interface IStorageGuidField : IStorageField
    {
        Guid? DefaultValue { get; set; }
        bool? GenerateNewId { get; set; }
    }
}