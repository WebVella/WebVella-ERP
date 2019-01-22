using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models
{
	public class InputSelectField : InputField
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.SelectField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "options")]
		public List<SelectOption> Options { get; set; }
	}

	[Serializable]
	public class SelectField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.SelectField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "options")]
		public List<SelectOption> Options { get; set; }
	}

	[Serializable]
	public class SelectOption
	{
		[JsonProperty(PropertyName = "value")]
		public string Value { get; set; } = "";

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; } = "";

		[JsonProperty(PropertyName = "icon_class")]
		public string IconClass { get; set; } = "";

		[JsonProperty(PropertyName = "color")]
		public string Color { get; set; } = "";

		public SelectOption()
		{

		}

		public SelectOption(string value, string label)
		{
			Value = value;
			Label = label;
		}

		public SelectOption(string value, string label, string iconClass, string color)
		{
			Value = value;
			Label = label;
			IconClass = iconClass;
			Color = color;
		}

		public SelectOption(SelectOption option) : this(option.Value, option.Label)
		{
		}
	}
}