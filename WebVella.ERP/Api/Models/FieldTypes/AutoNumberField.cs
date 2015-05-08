using System;

namespace WebVella.ERP.Api.Models
{
    public class AutoNumberField : Field
    {
        public decimal? DefaultValue { get; set; }

        public string DisplayFormat { get; set; }

        public decimal? StartingNumber { get; set; }
    }
}