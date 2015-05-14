using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoMultiSelectField : MongoBaseField, IStorageMultiSelectField
    {
        public IEnumerable<string> DefaultValue { get; set; }

        public IList<IStorageMultiSelectFieldOption> Options { get; set; }
    }

    public class MongoMultiSelectFieldOption : IStorageMultiSelectFieldOption
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}