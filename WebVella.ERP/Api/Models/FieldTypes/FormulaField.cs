using System;

namespace WebVella.ERP.Api.Models
{
    public class FormulaField : Field
    {
        public FormulaFieldReturnType ReturnType { get; set; }

        public string FormulaText { get; set; }

        public int? DecimalPlaces { get; set; }
    }
}