using System;

namespace WebVella.ERP.Core
{
    public interface IPercentField : IField
    {
        new decimal DefaultValue { get; set; }

        decimal MinValue { get; set; }

        decimal MaxValue { get; set; }

        byte DecimalPlaces { get; set; }

        decimal Value { get; set; }
    }
}