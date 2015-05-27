using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
	public class InputCheckboxField : InputField
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.CheckboxField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public bool? DefaultValue { get; set; }
	}

	public class CheckboxField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.CheckboxField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public bool? DefaultValue { get; set; }
    }
}