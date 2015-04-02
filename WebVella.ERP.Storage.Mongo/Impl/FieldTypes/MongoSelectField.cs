using System;
using System.Collections.Generic;

namespace WebVella.ERP.Core
{
    public class MongoSelectField : MongoBaseField, ISelectField
    {
        public new string DefaultValue { get; set; }

        public IDictionary<string, string> Options { get; set; }

        public string Value { get; set; }
    }
}