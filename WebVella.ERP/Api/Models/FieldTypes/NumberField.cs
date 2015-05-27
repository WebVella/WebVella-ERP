using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class InputNumberField : InputField
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.NumberField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public decimal? DefaultValue { get; set; }

        [JsonProperty(PropertyName = "minValue")]
        public decimal? MinValue { get; set; }

        [JsonProperty(PropertyName = "maxValue")]
        public decimal? MaxValue { get; set; }

        [JsonProperty(PropertyName = "decimalPlaces")]
        public byte? DecimalPlaces { get; set; }
    }

	public class NumberField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.NumberField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public decimal? DefaultValue { get; set; }

		[JsonProperty(PropertyName = "minValue")]
		public decimal? MinValue { get; set; }

		[JsonProperty(PropertyName = "maxValue")]
		public decimal? MaxValue { get; set; }

		[JsonProperty(PropertyName = "decimalPlaces")]
		public byte? DecimalPlaces { get; set; }
	}
}