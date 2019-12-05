using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public class DataSourceVariable
	{
		[JsonProperty(PropertyName = "type")]
		public DataSourceVariableType Type { get; set; } = DataSourceVariableType.DATASOURCE;

		[JsonProperty(PropertyName = "string")]
		public string String { get; set; }

		[JsonProperty(PropertyName = "default")]
		public string Default { get; set; }
	}

	public enum DataSourceVariableType
	{
		DATASOURCE = 0,
		CODE = 1,
		HTML = 2,
		SNIPPET = 3
	}

}
