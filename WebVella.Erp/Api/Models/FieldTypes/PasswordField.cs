using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
    public class InputPasswordField : InputField
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.PasswordField; } }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }

        [JsonProperty(PropertyName = "minLength")]
        public int? MinLength { get; set; }

        [JsonProperty(PropertyName = "encrypted")]
        public bool? Encrypted { get; set; }
    }

	[Serializable]
	public class PasswordField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.PasswordField; } }

		[JsonProperty(PropertyName = "maxLength")]
		public int? MaxLength { get; set; }

		[JsonProperty(PropertyName = "minLength")]
		public int? MinLength { get; set; }

		[JsonProperty(PropertyName = "encrypted")]
		public bool? Encrypted { get; set; }
	}
}