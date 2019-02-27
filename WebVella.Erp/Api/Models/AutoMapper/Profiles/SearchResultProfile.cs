using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace WebVella.Erp.Api.Models.AutoMapper.Profiles
{
	internal class SearchResultProfile : Profile
	{
		public SearchResultProfile()
		{
			CreateMap<DataRow, SearchResult>().ConvertUsing(source => DataRowToModelConvert(source));
		}

		private static SearchResult DataRowToModelConvert(DataRow rec)
		{
			if (rec == null)
				return null;

			HashSet<string> columnNames = new HashSet<string>();
			foreach (DataColumn column in rec.Table.Columns)
				columnNames.Add(column.ColumnName);
			
			SearchResult model = new SearchResult();
			if (columnNames.Contains("id"))
				model.Id = (Guid)rec["id"];
			if( columnNames.Contains("entities"))
				model.Entities = string.IsNullOrWhiteSpace((string)rec["entities"]) ? new List<Guid>() : JsonConvert.DeserializeObject<List<Guid>>((string)rec["entities"]);
			if (columnNames.Contains("apps"))
				model.Apps = string.IsNullOrWhiteSpace((string)rec["apps"]) ? new List<Guid>() : JsonConvert.DeserializeObject<List<Guid>>((string)rec["apps"]);
			if (columnNames.Contains("records"))
				model.Records = string.IsNullOrWhiteSpace((string)rec["records"]) ? new List<Guid>() : JsonConvert.DeserializeObject<List<Guid>>((string)rec["records"]);
			if (columnNames.Contains("content"))
				model.Content = (string)rec["content"];
			if (columnNames.Contains("stem_content"))
				model.StemContent = (string)rec["stem_content"];
			if (columnNames.Contains("snippet"))
				model.Snippet = (string)rec["snippet"];
			if (columnNames.Contains("url"))
				model.Url = (string)rec["url"];
			if (columnNames.Contains("aux_data"))
				model.AuxData = (string)rec["aux_data"];
			if (columnNames.Contains("timestamp"))
				model.Timestamp = (DateTime)rec["timestamp"];
			return model;
		}

	}
}
