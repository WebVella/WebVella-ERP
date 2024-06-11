using Newtonsoft.Json;

namespace WebVella.Erp.Api.Models
{
	public class DatabaseDataSource : DataSourceBase
	{
		[JsonProperty(PropertyName = "type")]
		public override DataSourceType Type { get { return DataSourceType.DATABASE; } }

		[JsonProperty(PropertyName = "eql_text")]
		public string EqlText { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "sql_text")]
		public string SqlText { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "result_model")]
		public override string ResultModel { get; set; } = "EntityRecordList";
	}
}
