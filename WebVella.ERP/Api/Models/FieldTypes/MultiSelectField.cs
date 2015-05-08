using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
    public class MultiSelectField : Field
    {
        public IEnumerable<string> DefaultValue { get; set; }

        public IDictionary<string, string> Options { get; set; }
    }
}