using System;

namespace WebVella.ERP.Core
{
    public class MongoHtmlField : MongoBaseField, IHtmlField
    {
        public new string DefaultValue { get; set; }

        public string Value { get; set; }
    }
}