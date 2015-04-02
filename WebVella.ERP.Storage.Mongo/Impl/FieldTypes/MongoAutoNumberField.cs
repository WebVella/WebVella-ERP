using System;

namespace WebVella.ERP.Core
{
    public class MongoAutoNumberField : MongoBaseField, IAutoNumberField
    {
        public new decimal DefaultValue { get; set; }

        public string DisplayFormat { get; set; }
        
        public decimal Value { get; set; }
    }
}