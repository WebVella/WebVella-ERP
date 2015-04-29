using System;

namespace WebVella.ERP.Api.Models
{
    public class DateTimeField : Field
    {
        public new DateTime? DefaultValue { get; set; }

        public string Format { get; set; }
    }
}