using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
    public class InputImageField : InputField
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.ImageField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }
    }

	[Serializable]
	public class ImageField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.ImageField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public string DefaultValue { get; set; }
	}
}