using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
	public class InputGeographyField : InputField
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.GeographyField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "maxLength")]
		public int? MaxLength { get; set; }

		[JsonProperty(PropertyName = "visibleLineNumber")]
		public int? VisibleLineNumber { get; set; }

		[JsonProperty(PropertyName = "format")]
		public GeographyFieldFormat? Format { get; set; }

		[JsonProperty(PropertyName = "srid")]
		public int SRID { get; set; } = ErpSettings.DefaultSRID;
	}

	[Serializable]
	public class GeographyField : Field
	{
		[JsonProperty(PropertyName = "fieldType")]
		public static FieldType FieldType { get { return FieldType.GeographyField; } }

		[JsonProperty(PropertyName = "defaultValue")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "maxLength")]
		public int? MaxLength { get; set; }

		[JsonProperty(PropertyName = "visibleLineNumber")]
		public int? VisibleLineNumber { get; set; }

		[JsonProperty(PropertyName = "format")]
		public GeographyFieldFormat? Format { get; set; }

		[JsonProperty(PropertyName = "srid")]
		public int SRID { get; set; } = ErpSettings.DefaultSRID;
	}
	[Serializable]
	public enum GeographyFieldFormat
	{
		GeoJSON = 1, // ST_AsGeoJSON, default
		Text = 2, // STAsText
	}
}
