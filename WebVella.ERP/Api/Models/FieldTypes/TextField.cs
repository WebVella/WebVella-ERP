using System;

namespace WebVella.ERP.Api.Models
{
    public class TextField : Field
    {
        public new string DefaultValue { get; set; }

        public int? MaxLength { get; set; }
    }
}