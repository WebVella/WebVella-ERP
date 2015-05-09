using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoSelectField : MongoBaseField, IStorageSelectField
    {
        public string DefaultValue { get; set; }

        public IDictionary<string, string> Options { get; set; }
    }
}