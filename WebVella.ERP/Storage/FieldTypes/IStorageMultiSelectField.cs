using System.Collections.Generic;

namespace WebVella.ERP.Storage
{
    public interface IStorageMultiSelectField : IStorageField
    {
        new IEnumerable<string> DefaultValue { get; set; }

        IDictionary<string, string> Options { get; set; }
    }
}