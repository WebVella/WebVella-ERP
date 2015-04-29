using System;

namespace WebVella.ERP.Api.Models
{
    public class MultiLineTextField : Field
    {
        public new string DefaultValue { get; set; }

        public int? MaxLength { get; set; }

        public int? VisibleLineNumber { get; set; }
    }
}