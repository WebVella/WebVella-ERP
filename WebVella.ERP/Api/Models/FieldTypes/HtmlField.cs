using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class HtmlField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.HtmlField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }
    }
}