using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models
{
	public class InputMultiSelectField : InputField
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.MultiSelectField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public IEnumerable<string> DefaultValue { get; set; }

		[JsonProperty(PropertyName = "options")]
		public List<SelectOption> Options { get; set; }
	}

	[Serializable]
	public class MultiSelectField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.MultiSelectField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public IEnumerable<string> DefaultValue { get; set; }

		[JsonProperty(PropertyName = "options")]
		public List<SelectOption> Options { get; set; }
	}
}