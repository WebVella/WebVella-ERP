using System;

namespace WebVella.ERP.Api.Models
{
    public class DateTimeField : Field
    {
        public DateTime? DefaultValue { get; set; }

        public string Format { get; set; }
    }
}