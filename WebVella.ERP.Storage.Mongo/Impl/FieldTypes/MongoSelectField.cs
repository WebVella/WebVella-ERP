using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoSelectField : MongoBaseField, IStorageSelectField
    {
        public new string DefaultValue { get; set; }

        public IDictionary<string, string> Options { get; set; }

        public string Value { get; set; }
    }
}