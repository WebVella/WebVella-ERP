using System;

namespace WebVella.ERP.Core
{
    public class MongoNumberField : MongoBaseField, INumberField
    {
        public new INumberFieldValue DefaultValue { get; set; }
    }
}