using System;

namespace WebVella.ERP.Api.Models
{
    public class CheckboxField : Field
    {
        public static FieldType FieldType { get { return FieldType.CheckboxField; } }

        public bool? DefaultValue { get; set; }
    }

    public class CheckboxFieldMeta : CheckboxField
    {
        public Guid EntityId { get; set; }

        public string EntityName { get; set; }
    }
}