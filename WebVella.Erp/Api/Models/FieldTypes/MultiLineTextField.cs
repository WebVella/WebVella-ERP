using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
    public class InputMultiLineTextField : InputField
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.MultiLineTextField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }

        [JsonProperty(PropertyName = "visibleLineNumber")]
        public int? VisibleLineNumber { get; set; }
    }

	[Serializable]
	public class MultiLineTextField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.MultiLineTextField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "maxLength")]
		public int? MaxLength { get; set; }

		[JsonProperty(PropertyName = "visibleLineNumber")]
		public int? VisibleLineNumber { get; set; }
	}
}