using System;

namespace WebVella.ERP.Api.Models
{
    public class EmailField : Field
    {
        public static FieldType FieldType { get { return FieldType.EmailField; } }

        public string DefaultValue { get; set; }

        public int? MaxLength { get; set; }
    }

    public class EmailFieldMeta : EmailField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }
    }
}