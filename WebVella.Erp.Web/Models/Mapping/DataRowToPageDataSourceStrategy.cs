using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.Mapping;

namespace WebVella.Erp.Web.Models.Mapping;

internal class DataRowToPageDataSourceStrategy : IMapStrategy<DataRow, PageDataSource>
{
	public PageDataSource Map(DataRow data)
	{
		if (data == null)
			return null;

		PageDataSource model = new PageDataSource();
		model.Id = new Guid(data["id"].ToString());
		model.Name = (string)data["name"];
		model.PageId = (Guid)data["page_id"];
		model.DataSourceId = (Guid)data["data_source_id"];

		if (!string.IsNullOrWhiteSpace((string)data["parameters"]))
			model.Parameters = JsonConvert.DeserializeObject<List<DataSourceParameter>>((string)data["parameters"]);
		else
			model.Parameters = new List<DataSourceParameter>();

		return model;
	}
}
