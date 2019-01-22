using WebVella.Erp.Api;
using Newtonsoft.Json;

namespace WebVella.Erp.Database
{
	public class DbCurrencyField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public decimal? DefaultValue { get; set; }

		[JsonProperty(PropertyName = "min_value")]
		public decimal? MinValue { get; set; }

		[JsonProperty(PropertyName = "max_value")]
		public decimal? MaxValue { get; set; }

		[JsonProperty(PropertyName = "currency")]
		public DbCurrencyType Currency { get; set; }
    }

	public class DbCurrencyType
	{
		[JsonProperty(PropertyName = "symbol")]
		public string Symbol { get; set; }

		[JsonProperty(PropertyName = "symbol_native")]
		public string SymbolNative { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "name_plural")]
		public string NamePlural { get; set; }

		[JsonProperty(PropertyName = "code")]
		public string Code { get; set; }

		[JsonProperty(PropertyName = "decimal_digits")]
		public int DecimalDigits { get; set; }

		[JsonProperty(PropertyName = "rounding")]
		public int Rounding { get; set; }

		[JsonProperty(PropertyName = "symbol_placement")]
		public CurrencySymbolPlacement SymbolPlacement { get; set; }
	}
}