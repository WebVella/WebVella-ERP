using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Erp.Api.Models
{
	public class Currency
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; } = "";

		[JsonProperty(PropertyName = "alternate_symbols")]
		public List<string> AlternateSymbols { get; set; } = new List<string>();

		[JsonProperty(PropertyName = "decimal_mark")]
		public string DecimalMark { get; set; } = "";

		[JsonProperty(PropertyName = "disambiguate_symbol")]
		public string DisambiguateSymbol { get; set; } = "";

		[JsonProperty(PropertyName = "html_entity")]
		public string html_entity { get; set; } = "";

		[JsonProperty(PropertyName = "iso_code")]
		public string IsoCode { get; set; } = "";

		[JsonProperty(PropertyName = "iso_numeric")]
		public string IsoNumeric { get; set; } = "";

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; } = "";

		[JsonProperty(PropertyName = "priority")]
		public int Priority { get; set; } = 100;

		[JsonProperty(PropertyName = "smallest_denomination")]
		public int SmallestDenomination { get; set; } = 1;

		[JsonProperty(PropertyName = "subunit")]
		public string SubUnit { get; set; } = "";

		[JsonProperty(PropertyName = "subunit_to_unit")]
		public int SubUnitToUnit { get; set; } = 100;

		[JsonProperty(PropertyName = "symbol")]
		public string Symbol { get; set; } = "";

		[JsonProperty(PropertyName = "symbol_first")]
		public bool SymbolFirst { get; set; } = false;

		[JsonProperty(PropertyName = "thousands_separator")]
		public string ThousandsSeparator { get; set; } = "";
	}
}
