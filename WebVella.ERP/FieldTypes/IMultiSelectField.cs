
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Core
{
    public interface IMultiSelectField : IField
    {
        new IEnumerable<string> DefaultValue { get; set; }

        IDictionary<string, string> Options { get; set; }

        IEnumerable<string> Values { get; set; }
    }
}