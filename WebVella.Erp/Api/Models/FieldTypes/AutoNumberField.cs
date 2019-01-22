using System;
using Newtonsoft.Json;
using System.Linq;

namespace WebVella.Erp.Api.Models
{
	public class InputAutoNumberField : InputField
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.AutoNumberField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public decimal? DefaultValue { get; set; }

		[JsonProperty(PropertyName = "displayFormat")]
		public string DisplayFormat { get; set; }

		[JsonProperty(PropertyName = "startingNumber")]
		public decimal? StartingNumber { get; set; }
	}

	[Serializable]
	public class AutoNumberField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.AutoNumberField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public decimal? DefaultValue { get; set; }

        [JsonProperty(PropertyName = "displayFormat")]
        public string DisplayFormat { get; set; }

        [JsonProperty(PropertyName = "startingNumber")]
        public decimal? StartingNumber { get; set; }
    }
}