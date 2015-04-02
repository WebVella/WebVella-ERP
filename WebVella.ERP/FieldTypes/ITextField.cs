using System;

namespace WebVella.ERP.Core
{
    public interface ITextField : IField
    {
        new string DefaultValue { get; set; }

        int MaxLength { get; set; }

        string Value { get; set; }
    }
}