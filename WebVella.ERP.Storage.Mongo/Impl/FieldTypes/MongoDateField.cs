using System;

namespace WebVella.ERP.Core
{
    public class MongoDateField : MongoBaseField, IDateField
    {
        public new DateTime DefaultValue { get; set; }

        public string Format { get; set; }

        public DateTime Value { get; set; }
    }
}