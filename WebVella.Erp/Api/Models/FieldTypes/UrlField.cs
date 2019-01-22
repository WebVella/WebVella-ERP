using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
    public class InputUrlField : InputField
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.UrlField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }

        [JsonProperty(PropertyName = "openTargetInNewWindow")]
        public bool? OpenTargetInNewWindow { get; set; }
    }

	[Serializable]
	public class UrlField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.UrlField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "maxLength")]
		public int? MaxLength { get; set; }

		[JsonProperty(PropertyName = "openTargetInNewWindow")]
		public bool? OpenTargetInNewWindow { get; set; }
	}
}
