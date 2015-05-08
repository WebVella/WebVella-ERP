using System;

namespace WebVella.ERP.Api.Models
{
    public class NumberField : Field
    {
        public static FieldType FieldType { get { return FieldType.NumberField; } }

        public decimal? DefaultValue { get; set; }

        public decimal? MinValue { get; set; }

        public decimal? MaxValue { get; set; }

        public byte? DecimalPlaces { get; set; }
    }

    public class NumberFieldMeta : NumberField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }
    }
}