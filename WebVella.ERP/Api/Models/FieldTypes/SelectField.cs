using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
    public class SelectField : Field
    {
        public static FieldType FieldType { get { return FieldType.SelectField; } }

        public string DefaultValue { get; set; }

        public IDictionary<string, string> Options { get; set; }
    }

    public class SelectFieldMeta : SelectField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }
    }
}