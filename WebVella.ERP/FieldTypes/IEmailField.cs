using System;

namespace WebVella.ERP.Core
{
    public interface IEmailField : IField
    {
        new string DefaultValue { get; set; }

        int MaxLength { get; set; }

        string Value { get; set; }
    }
}