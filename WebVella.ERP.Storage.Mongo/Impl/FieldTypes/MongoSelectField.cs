using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoSelectField : MongoBaseField, IStorageSelectField
    {
        public string DefaultValue { get; set; }

        public IList<IStorageSelectFieldOption> Options { get; set; }
    }

    public class MongoSelectFieldOption : IStorageSelectFieldOption
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}