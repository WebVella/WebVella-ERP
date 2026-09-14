using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DataRowToSearchResultStrategy : IMapStrategy<DataRow, SearchResult>
{
	public SearchResult Map(DataRow source)
	{
		if (source == null)
			return null;

		var columnNames = new HashSet<string>();
		foreach (System.Data.DataColumn column in source.Table.Columns)
			columnNames.Add(column.ColumnName);

		var dest = new SearchResult();

		if (columnNames.Contains("id"))
			dest.Id = (Guid)source["id"];

		if (columnNames.Contains("entities"))
			dest.Entities = string.IsNullOrWhiteSpace((string)source["entities"])
				? []
				: JsonConvert.DeserializeObject<List<Guid>>((string)source["entities"]);

		if (columnNames.Contains("apps"))
			dest.Apps = string.IsNullOrWhiteSpace((string)source["apps"])
				? []
				: JsonConvert.DeserializeObject<List<Guid>>((string)source["apps"]);

		if (columnNames.Contains("records"))
			dest.Records = string.IsNullOrWhiteSpace((string)source["records"])
				? []
				: JsonConvert.DeserializeObject<List<Guid>>((string)source["records"]);

		if (columnNames.Contains("content"))
			dest.Content = (string)source["content"];

		if (columnNames.Contains("stem_content"))
			dest.StemContent = (string)source["stem_content"];

		if (columnNames.Contains("snippet"))
			dest.Snippet = (string)source["snippet"];

		if (columnNames.Contains("url"))
			dest.Url = (string)source["url"];

		if (columnNames.Contains("aux_data"))
			dest.AuxData = (string)source["aux_data"];

		if (columnNames.Contains("timestamp"))
			dest.Timestamp = (DateTime)source["timestamp"];

		return dest;
	}
}
