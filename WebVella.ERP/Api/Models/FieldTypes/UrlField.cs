using System;

namespace WebVella.ERP.Api.Models
{
    public class UrlField : Field
    {
        public string DefaultValue { get; set; }

        public int? MaxLength { get; set; }

        public bool? OpenTargetInNewWindow { get; set; }
    }
}
