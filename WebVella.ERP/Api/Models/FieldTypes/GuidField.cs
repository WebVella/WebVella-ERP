using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
    public class InputGuidField : InputField
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.GuidField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public Guid? DefaultValue { get; set; }

        [JsonProperty(PropertyName = "generateNewId")]
        public bool? GenerateNewId { get; set; }

        public InputGuidField()
        {
        }

        public InputGuidField(InputField field) : base(field)
        {
        }
    }

	public class GuidField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.GuidField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public Guid? DefaultValue { get; set; }

		[JsonProperty(PropertyName = "generateNewId")]
		public bool? GenerateNewId { get; set; }

		public GuidField()
		{
		}

		public GuidField(Field field) : base(field)
		{
		}
	}
}