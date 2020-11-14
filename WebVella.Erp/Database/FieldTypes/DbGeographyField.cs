using Newtonsoft.Json;

namespace WebVella.Erp.Database
{
	// Not supported at the moment
	public class DbGeographyField : DbBaseField
	{
		[JsonProperty(PropertyName = "default_value")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "max_length")]
		public int? MaxLength { get; set; }

		[JsonProperty(PropertyName = "visible_line_number")]
		public int? VisibleLineNumber { get; set; }

		[JsonProperty(PropertyName = "format")]
		public DbGeographyFieldFormat? Format { get; set; }

		[JsonProperty(PropertyName = "srid")]
		public int SRID { get; set; } = ErpSettings.DefaultSRID;
	}

	public enum DbGeographyFieldFormat
	{
		GeoJSON = 1, // STAsGeoJson
		Text = 2, // STAsText
	}
}
