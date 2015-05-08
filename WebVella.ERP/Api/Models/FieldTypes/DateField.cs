using System;

namespace WebVella.ERP.Api.Models
{
    public class DateField : Field
    {
        public DateTime? DefaultValue { get; set; }

        public string Format { get; set; }
    }
}