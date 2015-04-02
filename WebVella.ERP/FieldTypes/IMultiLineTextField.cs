using System;

namespace WebVella.ERP.Core
{
    public interface IMultiLineTextField : IField
    {
        new string DefaultValue { get; set; }

        int LineNumber { get; set; }

        string Value { get; set; }
    }
}