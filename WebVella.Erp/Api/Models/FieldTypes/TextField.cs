using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
    public class InputTextField : InputField
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.TextField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }
    }

	[Serializable]
	public class TextField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.TextField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "maxLength")]
		public int? MaxLength { get; set; }
	}
}