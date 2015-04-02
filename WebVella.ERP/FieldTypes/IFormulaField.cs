
using System;

namespace WebVella.ERP.Core
{
    public enum FormulaReturnType
    {
        Checkbox,
        Currency,
        Date,
        DateTime,
        Number,
        Percent,
        Text
    }

    public interface IFormulaField : IField
    {
        FormulaReturnType ReturnType { get; set; }

        string FormulaText { get; set; }

        decimal MinValue { get; set; }

        decimal MaxValue { get; set; }

        int DecimalPlaces { get; set; }

        object Value { get; set; }
    }
}