using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
	public class InputCurrencyField : InputField
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.CurrencyField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public decimal? DefaultValue { get; set; }

		[JsonProperty(PropertyName = "minValue")]
		public decimal? MinValue { get; set; }

		[JsonProperty(PropertyName = "maxValue")]
		public decimal? MaxValue { get; set; }

		[JsonProperty(PropertyName = "currency")]
		public CurrencyType Currency { get; set; }
	}

	[Serializable]
	public class CurrencyField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.CurrencyField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public decimal? DefaultValue { get; set; }

        [JsonProperty(PropertyName = "minValue")]
        public decimal? MinValue { get; set; }

        [JsonProperty(PropertyName = "maxValue")]
        public decimal? MaxValue { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public CurrencyType Currency { get; set; }
    }
}