using System;

namespace WebVella.ERP.Api.Models
{
    public class DateField : Field
    {
        public new DateTime? DefaultValue { get; set; }

        public string Format { get; set; }
    }
}