using System;

namespace WebVella.ERP.Api.Models
{
    public class TextField : Field
    {
        public string DefaultValue { get; set; }

        public int? MaxLength { get; set; }
    }
}