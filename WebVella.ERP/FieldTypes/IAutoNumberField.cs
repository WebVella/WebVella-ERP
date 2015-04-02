using System;

namespace WebVella.ERP.Core
{
    public interface IAutoNumberField : IField
    {
        new decimal DefaultValue { get; set; }

        string DisplayFormat { get; set; }

        decimal Value { get; set; }
    }
}