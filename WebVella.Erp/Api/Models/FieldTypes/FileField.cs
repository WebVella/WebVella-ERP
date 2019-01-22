using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
    public class InputFileField : InputField
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.FileField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }
    }

	[Serializable]
	public class FileField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.FileField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public string DefaultValue { get; set; }
	}
}