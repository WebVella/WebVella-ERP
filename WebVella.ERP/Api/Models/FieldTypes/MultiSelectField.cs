using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models
{
	public class InputMultiSelectField : InputField
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.MultiSelectField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public IEnumerable<string> DefaultValue { get; set; }

        [JsonProperty(PropertyName = "options")]
        public List<MultiSelectFieldOption> Options { get; set; }
    }

	public class MultiSelectField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.MultiSelectField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public IEnumerable<string> DefaultValue { get; set; }

		[JsonProperty(PropertyName = "options")]
		public List<MultiSelectFieldOption> Options { get; set; }
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
}