using System;

namespace WebVella.ERP.Core
{
    public class MongoDateTimeField : MongoBaseField, IDateTimeField
    {
        public new IDateTimeFieldValue DefaultValue { get; set; }
    }
}