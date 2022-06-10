using Newtonsoft.Json;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;

namespace WebVella.Erp.Web.Models
{
	public class EqlQuery
	{
		[JsonProperty(PropertyName = "eql")]
		public string Eql { get; set; }

		[JsonProperty(PropertyName = "parameters")]
		public List<EqlParameter> Parameters { get; set; } = new();
	}

	public class EqlDataSourceQuery
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "parameters")]
		public List<EqlParameter> Parameters { get; set; } = new();
	}
}
