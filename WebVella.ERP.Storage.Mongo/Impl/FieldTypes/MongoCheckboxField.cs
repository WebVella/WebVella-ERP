using System;

namespace WebVella.ERP.Core
{
    public class MongoCheckboxField : MongoBaseField, ICheckboxField
    {
        public new bool DefaultValue { get; set; }

        public bool Value { get; set; }
    }
}