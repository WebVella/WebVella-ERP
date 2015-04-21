using System.Collections.Generic;

namespace WebVella.ERP.Storage
{ 
    public interface IStorageSelectField : IStorageField
    {
        new string DefaultValue { get; set; }

        IDictionary<string, string> Options { get; set; }

        string Value { get; set; }
    }
}