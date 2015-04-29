using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
    public class SelectField : Field
    {
        public new string DefaultValue { get; set; }

        public IDictionary<string, string> Options { get; set; }
    }
}