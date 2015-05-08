using System;

namespace WebVella.ERP.Api.Models
{
    public class CurrencyField : Field
    {
        public decimal? DefaultValue { get; set; }

        public decimal? MinValue { get; set; }

        public decimal? MaxValue { get; set; }

        public CurrencyTypes Currency { get; set; }
    }
}