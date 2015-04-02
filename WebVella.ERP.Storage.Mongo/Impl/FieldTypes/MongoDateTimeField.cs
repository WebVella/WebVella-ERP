using System;

namespace WebVella.ERP.Core
{
    public class MongoDateTimeField : MongoBaseField, IDateTimeField
    {
        public new DateTime DefaultValue { get; set; }

        public string Format { get; set; }

        public DateTime Value { get; set; }
    }
}