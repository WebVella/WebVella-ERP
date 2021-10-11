using Newtonsoft.Json;

namespace WebVella.Erp.Eql
{
	public class EqlSettings
	{
		[JsonProperty(PropertyName = "include_total")]
		public bool IncludeTotal { get; private set; } = true;

		[JsonProperty(PropertyName = "distinct")]
		public bool Distinct { get; internal set; } = false;
	}
}
