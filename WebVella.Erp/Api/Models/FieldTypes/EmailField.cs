using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
    public class InputEmailField : InputField
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.EmailField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }
    }

	[Serializable]
	public class EmailField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.EmailField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "maxLength")]
		public int? MaxLength { get; set; }
	}
}