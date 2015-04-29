using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoDateTimeField : MongoBaseField, IStorageDateTimeField
    {
        public new DateTime DefaultValue { get; set; }

        public string Format { get; set; }
    }
}