using System;

namespace WebVella.ERP.Core
{
    public interface INumberField : IField
    {
        new decimal DefaultValue { get; set; }

        int MinValue { get; set; }

        int MaxValue { get; set; }

        decimal Value { get; set; }
    }
}