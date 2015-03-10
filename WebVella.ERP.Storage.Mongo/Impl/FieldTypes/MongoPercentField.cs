using System;

namespace WebVella.ERP.Core
{
    public class MongoPercentField : MongoBaseField, IPercentField
    {
        public new INumberFieldValue DefaultValue { get; set; }
    }
}