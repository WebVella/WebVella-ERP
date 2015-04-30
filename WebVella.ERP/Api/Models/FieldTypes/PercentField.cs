using System;

namespace WebVella.ERP.Api.Models
{
    public class PercentField : Field
    {
        public new decimal? DefaultValue { get; set; }

        public decimal? MinValue { get; set; }

        public decimal? MaxValue { get; set; }

        public byte? DecimalPlaces { get; set; }
    }
}