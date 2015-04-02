using System;

namespace WebVella.ERP.Core
{
    public class MongoFormulaField : MongoBaseField, IFormulaField
    {
        public FormulaReturnType ReturnType { get; set; }

        public string FormulaText { get; set; }

        public decimal MinValue { get; set; }

        public decimal MaxValue { get; set; }

        public int DecimalPlaces { get; set; }

        public object Value { get; set; }
    }
}