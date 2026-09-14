using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DataRowToDatabaseDataSourceStrategy : IMapStrategy<DataRow, DatabaseDataSource>
{
	public DatabaseDataSource Map(DataRow source)
	{
		if (source == null)
			return null;

		var dest = new DatabaseDataSource();
		dest.Id = (System.Guid)source["id"];
		dest.Name = (string)source["name"];
		dest.Description = (string)source["description"];
		dest.Weight = (int)source["weight"];
		dest.ReturnTotal = (bool)source["return_total"];
		dest.EqlText = (string)source["eql_text"];
		dest.SqlText = (string)source["sql_text"];
		dest.Parameters.AddRange(JsonConvert.DeserializeObject<List<DataSourceParameter>>((string)source["parameters_json"]).ToArray());
		dest.Fields.AddRange(JsonConvert.DeserializeObject<List<DataSourceModelFieldMeta>>((string)source["fields_json"]).ToArray());
		dest.EntityName = (string)source["entity_name"];

		foreach (var par in dest.Parameters)
			if (par.Name.StartsWith("@"))
				par.Name = par.Name.Substring(1);

		return dest;
	}
}
