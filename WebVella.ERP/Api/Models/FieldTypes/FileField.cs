using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class FileField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.FileField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }
    }
}