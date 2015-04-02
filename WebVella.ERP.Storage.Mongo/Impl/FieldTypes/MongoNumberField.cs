using System;

namespace WebVella.ERP.Core
{
    public class MongoNumberField : MongoBaseField, INumberField
    {
        public new decimal DefaultValue { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public decimal Value { get; set; }
    }
}