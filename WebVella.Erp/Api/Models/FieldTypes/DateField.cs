using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
	public class InputDateField : InputField
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.DateField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public DateTime? DefaultValue { get; set; }

		[JsonProperty(PropertyName = "format")]
		public string Format { get; set; }

		[JsonProperty(PropertyName = "useCurrentTimeAsDefaultValue")]
		public bool? UseCurrentTimeAsDefaultValue { get; set; }
	}

	[Serializable]
	public class DateField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.DateField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public DateTime? DefaultValue { get; set; }

        [JsonProperty(PropertyName = "format")]
        public string Format { get; set; }

        [JsonProperty(PropertyName = "useCurrentTimeAsDefaultValue")]
        public bool? UseCurrentTimeAsDefaultValue { get; set; }
    }
}