using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.Erp.Api.Models
{
	public abstract class DataSourceBase
	{
		[JsonProperty(PropertyName = "id")]
		public virtual Guid Id { get; set; }

		[JsonProperty(PropertyName = "type")]
		public virtual DataSourceType Type { get; private set; }

		[JsonProperty(PropertyName = "name")]
		public virtual string Name { get; set; } 

		[JsonProperty(PropertyName = "description")]
		public virtual string Description { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "weight")]
		public virtual int Weight { get; set; } = 10;

		[JsonProperty(PropertyName = "entity_name")]
		public virtual string EntityName { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "fields")]
		public virtual List<DataSourceModelFieldMeta> Fields { get; private set; } = new List<DataSourceModelFieldMeta>();

		[JsonProperty(PropertyName = "parameters")]
		public virtual List<DataSourceParameter> Parameters { get; private set; } = new List<DataSourceParameter>();

		[JsonProperty(PropertyName = "result_model")]
		public virtual string ResultModel { get; set; } = "object";

		public override string ToString()
		{
			return Name;
		}
	}
}
