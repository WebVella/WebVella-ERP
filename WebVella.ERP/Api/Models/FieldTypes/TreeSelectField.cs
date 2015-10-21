using System;
using Newtonsoft.Json;
using System.Linq;

namespace WebVella.ERP.Api.Models
{
	public class InputTreeSelectField : InputField
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.TreeSelectField; } }
	}

	public class TreeSelectField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.TreeSelectField; } }
    }
}