using System.Collections.Generic;

namespace WebVella.ERP.Storage
{ 
    public interface IStorageSelectField : IStorageField
    {
        string DefaultValue { get; set; }

        IList<IStorageSelectFieldOption> Options { get; set; }
    }

    public interface IStorageSelectFieldOption
    {
        string Key { get; set; }

        string Value { get; set; }
    }
}