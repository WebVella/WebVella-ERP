using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using Newtonsoft.Json.Linq;
using WebVella.Erp.Api;
using Newtonsoft.Json;
using System.Data;

namespace WebVella.Erp.Web.Models.AutoMapper.Profiles
{
	public class PageDataSourceProfile : Profile
	{
		public PageDataSourceProfile()
		{
			CreateMap<DataRow, PageDataSource>().ConvertUsing(source => DataRowToPageDataSourceConvert(source));
		}

		private static PageDataSource DataRowToPageDataSourceConvert(DataRow data)
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
}
