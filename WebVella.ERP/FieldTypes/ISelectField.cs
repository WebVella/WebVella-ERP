using System;
using System.Collections.Generic;

namespace WebVella.ERP.Core
{
    public interface ISelectField : IField
    {
        new string DefaultValue { get; set; }

        IDictionary<string, string> Options { get; set; }

        string Value { get; set; }
    }
}