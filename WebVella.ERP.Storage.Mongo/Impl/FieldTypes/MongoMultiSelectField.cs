using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoMultiSelectField : MongoBaseField, IStorageMultiSelectField
    {
        public new IEnumerable<string> DefaultValue { get; set; }

        public IDictionary<string, string> Options { get; set; }

        public IEnumerable<string> Values { get; set; }
    }
}