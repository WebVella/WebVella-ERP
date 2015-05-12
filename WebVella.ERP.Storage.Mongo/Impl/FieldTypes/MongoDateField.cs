using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoDateField : MongoBaseField, IStorageDateField
    {
        public DateTime? DefaultValue { get; set; }

        public string Format { get; set; }

        public bool UseCurrentTimeAsDefaultValue { get; set; }
    }
}