using System;

namespace WebVella.ERP.Core
{
    public class MongoEmailField : MongoBaseField, IEmailField
    {
        public new string DefaultValue { get; set; }

        public int MaxLength { get; set; }

        public string Value { get; set; }
    }
}