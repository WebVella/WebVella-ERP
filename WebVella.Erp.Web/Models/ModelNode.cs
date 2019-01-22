using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public class ModelNode
	{
		[JsonProperty("page_datasource_id")]
		public Guid PageDataSourceId { get; set; } = Guid.Empty;//Manual input or from manually set attribute

		[JsonProperty("page_datasource_name")]
		public string PageDataSourceName { get; set; } = "";//Manual input or from manually set attribute

		[JsonProperty("entity_name")]
		public string EntityName { get; set; } = "";//Manual input or from manually set attribute

		[JsonProperty("data_type")]
		public string DataType { get; set; } = "";//Manual input or from manually set attribute

		[JsonProperty("nodes")]
		public List<ModelNode> Nodes { get; set; } = new List<ModelNode>();//Manual input or from manually set attribute

		[JsonProperty("tags")]
		public List<string> Tags { get; set; } = new List<string>();//system,database,code

		[JsonProperty("data_source_id")]
		public Guid? DataSourceId { get; set; } = null;

		[JsonProperty("params")]
		public List<DataSourceParameter> Params { get; set; } = new List<DataSourceParameter>();

	}
}
