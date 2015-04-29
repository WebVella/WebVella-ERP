using System;

namespace WebVella.ERP.Api.Models
{
    public class PhoneField : Field
    {
        public new string DefaultValue { get; set; }

        public string Format { get; set; }

        public int? MaxLength { get; set; }
    }
}