using System;

namespace WebVella.ERP.Api.Models
{
    public class PhoneField : Field
    {
        public string DefaultValue { get; set; }

        public string Format { get; set; }

        public int? MaxLength { get; set; }
    }
}