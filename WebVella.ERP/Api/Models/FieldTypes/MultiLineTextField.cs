using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
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