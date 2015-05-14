using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models
{
	public class MultiSelectField : Field
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.MultiSelectField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public IEnumerable<string> DefaultValue { get; set; }

        [JsonProperty(PropertyName = "options")]
        public List<MultiSelectFieldOption> Options { get; set; }

        public MultiSelectField()
        {
        }

        public MultiSelectField(Field field) : base(field)
        {
        }

        public static List<MultiSelectFieldOption> ConvertOptions(IList<IStorageMultiSelectFieldOption> storageOptions)
        {
            List<MultiSelectFieldOption> options = new List<MultiSelectFieldOption>();

            foreach (var storageOption in storageOptions)
            {
                MultiSelectFieldOption option = new MultiSelectFieldOption(storageOption);
                options.Add(option);
            }

            return options;
        }
    }

    public class MultiSelectFieldOption
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        public MultiSelectFieldOption()
        {

        }

        public MultiSelectFieldOption(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public MultiSelectFieldOption(IStorageMultiSelectFieldOption option) : this(option.Key, option.Value)
        {
        }
    }

    public class MultiSelectFieldMeta : MultiSelectField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public MultiSelectFieldMeta(Guid entityId, string entityName, MultiSelectField field, string parentFieldName = null) : base(field)
        {
            EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
			Options = field.Options;
            ParentFieldName = parentFieldName;
        }
	}
}