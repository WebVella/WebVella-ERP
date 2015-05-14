using System.Collections.Generic;

namespace WebVella.ERP.Storage
{
    public interface IStorageMultiSelectField : IStorageField
    {
        IEnumerable<string> DefaultValue { get; set; }

        IList<IStorageMultiSelectFieldOption> Options { get; set; }
    }

    public interface IStorageMultiSelectFieldOption
    {
        string Key { get; set; }

        string Value { get; set; }
    }
}