using System;

namespace WebVella.ERP.Api.Models
{
    public class NumberField : Field
    {
        public decimal? DefaultValue { get; set; }

        public decimal? MinValue { get; set; }

        public decimal? MaxValue { get; set; }

        public byte? DecimalPlaces { get; set; }
    }
}