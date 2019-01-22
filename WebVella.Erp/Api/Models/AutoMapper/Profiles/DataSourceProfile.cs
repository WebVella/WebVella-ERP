using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace WebVella.Erp.Api.Models.AutoMapper.Profiles
{
	public class DataSourceProfile : Profile
	{
		public DataSourceProfile()
		{
			CreateMap<DataRow, DatabaseDataSource>().ConvertUsing(source => DataRowToModelConvert(source));
		}

		private static DatabaseDataSource DataRowToModelConvert(DataRow inputObj)
		{
			
			if (inputObj == null)
				return null;

			var outputObj = new DatabaseDataSource();
			outputObj.Id = (Guid)inputObj["id"];
			outputObj.Name = (string)inputObj["name"];
			outputObj.Description = (string)inputObj["description"];
			outputObj.Weight = (int)inputObj["weight"];
			outputObj.EqlText = (string)inputObj["eql_text"];
			outputObj.SqlText = (string)inputObj["sql_text"];
			outputObj.Parameters.AddRange( JsonConvert.DeserializeObject<List<DataSourceParameter>>((string)inputObj["parameters_json"]).ToArray() );
			outputObj.Fields.AddRange(JsonConvert.DeserializeObject<List<DataSourceModelFieldMeta>>((string)inputObj["fields_json"]).ToArray());
			outputObj.EntityName = (string)inputObj["entity_name"];

			//clean data source parameters from leading @ character
			foreach (var par in outputObj.Parameters)
				if (par.Name.StartsWith("@"))
					par.Name = par.Name.Substring(1);

			return outputObj;
		}

	}
}
