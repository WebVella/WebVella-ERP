using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class ImageField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.ImageField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }
    }
}