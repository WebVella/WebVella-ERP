using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoMultiSelectField : MongoBaseField, IStorageMultiSelectField
    {
        public IEnumerable<string> DefaultValue { get; set; }

        public IDictionary<string, string> Options { get; set; }
    }
}