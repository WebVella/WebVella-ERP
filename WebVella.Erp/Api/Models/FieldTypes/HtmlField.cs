using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
    public class InputHtmlField : InputField
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.HtmlField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }
    }

	[Serializable]
	public class HtmlField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.HtmlField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public string DefaultValue { get; set; }
	}
}