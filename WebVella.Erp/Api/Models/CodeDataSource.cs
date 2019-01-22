using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebVella.Erp.Api.Models
{
	public abstract class CodeDataSource : DataSourceBase
	{
		[JsonProperty(PropertyName = "type")]
		public override DataSourceType Type { get { return DataSourceType.CODE; } }

		[JsonProperty(PropertyName = "result_model")]
		public override string ResultModel { get; set; } = "object";

		public abstract object Execute(Dictionary<string, object> arguments);
	}
}
