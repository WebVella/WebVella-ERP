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

    public class HtmlFieldMeta : HtmlField
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

		public HtmlFieldMeta(Guid entityId, string entityName, HtmlField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
		}
	}
}