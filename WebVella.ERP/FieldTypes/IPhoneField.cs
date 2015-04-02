using System;

namespace WebVella.ERP.Core
{
    public interface IPhoneField : IField
    {
        new string DefaultValue { get; set; }

        string Format { get; set; }

        int MaxLength { get; set; }

        string Value { get; set; }
    }
}