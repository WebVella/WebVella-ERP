using System;

namespace WebVella.ERP.Core
{
    public class MongoDateField : MongoBaseField, IDateField
    {
        public new IDateTimeFieldValue DefaultValue { get; set; }
    }
}