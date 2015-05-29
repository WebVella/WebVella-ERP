using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models
{
    public class InputSelectField : InputField
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.SelectField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty(PropertyName = "options")]
        public List<SelectFieldOption> Options { get; set; }
	}

	public class SelectField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.SelectField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "options")]
		public List<SelectFieldOption> Options { get; set; }
	}

	public class SelectFieldOption
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        public SelectFieldOption()
        {

        }

        public SelectFieldOption(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public SelectFieldOption(IStorageSelectFieldOption option) : this(option.Key, option.Value)
        {
        }
    }
}