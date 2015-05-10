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

    public class ImageFieldMeta : ImageField
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

		public ImageFieldMeta(Guid entityId, string entityName, ImageField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue= field.DefaultValue;
		}
	}
}