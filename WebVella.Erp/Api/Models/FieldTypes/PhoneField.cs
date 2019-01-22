using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
    public class InputPhoneField : InputField
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.PhoneField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty(PropertyName = "format")]
        public string Format { get; set; }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }
    }

	[Serializable]
	public class PhoneField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.PhoneField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "format")]
		public string Format { get; set; }

		[JsonProperty(PropertyName = "maxLength")]
		public int? MaxLength { get; set; }
	}
}