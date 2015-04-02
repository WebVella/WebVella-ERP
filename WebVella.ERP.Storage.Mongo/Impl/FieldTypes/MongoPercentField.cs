using System;

namespace WebVella.ERP.Core
{
    public class MongoPercentField : MongoBaseField, IPercentField
    {
        public new decimal DefaultValue { get; set; }

        public decimal MinValue { get; set; }

        public decimal MaxValue { get; set; }

        public byte DecimalPlaces { get; set; }

        public decimal Value { get; set; }
    }
}