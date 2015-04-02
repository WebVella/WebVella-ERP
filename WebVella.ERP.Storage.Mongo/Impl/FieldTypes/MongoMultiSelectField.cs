using System;
using System.Collections.Generic;

namespace WebVella.ERP.Core
{
    public class MongoMultiSelectField : MongoBaseField, IMultiSelectField
    {
        public new IEnumerable<string> DefaultValue { get; set; }

        public IDictionary<string, string> Options { get; set; }

        public IEnumerable<string> Values { get; set; }
    }
}