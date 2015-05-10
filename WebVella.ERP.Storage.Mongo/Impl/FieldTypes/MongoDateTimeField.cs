using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoDateTimeField : MongoBaseField, IStorageDateTimeField
    {
        public DateTime DefaultValue { get; set; }

        public string Format { get; set; }
    }
}