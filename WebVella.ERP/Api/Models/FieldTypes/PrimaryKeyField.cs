using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class PrimaryKeyField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.PrimaryKeyField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public Guid? DefaultValue { get; set; }
    }

    public class PrimaryKeyFieldMeta : PrimaryKeyField
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

		public PrimaryKeyFieldMeta(Guid entityId, string entityName, PrimaryKeyField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
		}
	}
}