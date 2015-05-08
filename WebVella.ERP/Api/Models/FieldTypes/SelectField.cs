using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
    public class SelectField : Field
    {
        public string DefaultValue { get; set; }

        public IDictionary<string, string> Options { get; set; }
    }
}