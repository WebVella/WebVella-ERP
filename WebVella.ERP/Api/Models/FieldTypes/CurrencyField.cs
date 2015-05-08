using System;

namespace WebVella.ERP.Api.Models
{
    public class CurrencyField : Field
    {
        public static FieldType FieldType { get { return FieldType.CurrencyField; } }

        public decimal? DefaultValue { get; set; }

        public decimal? MinValue { get; set; }

        public decimal? MaxValue { get; set; }

        public CurrencyTypes Currency { get; set; }
    }

    public class CurrencyFieldMeta : CurrencyField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }
    }
}