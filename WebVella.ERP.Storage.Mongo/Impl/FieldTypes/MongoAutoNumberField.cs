using System;

namespace WebVella.ERP.Core
{
    public class MongoAutoNumberField : MongoBaseField, IAutoNumberField
    {
        public new INumberFieldValue DefaultValue { get; set; }
    }
}