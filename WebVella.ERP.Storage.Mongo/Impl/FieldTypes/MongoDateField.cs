using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoDateField : MongoBaseField, IStorageDateField
    {
        public DateTime DefaultValue { get; set; }

        public string Format { get; set; }
    }
}