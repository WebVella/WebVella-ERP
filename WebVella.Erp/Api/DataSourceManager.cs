using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;
using WebVella.Erp.Api.Models.AutoMapper;
using Newtonsoft.Json;
using WebVella.Erp.Eql;
using System.Linq;
using WebVella.Erp.Exceptions;

namespace WebVella.Erp.Api
{
	public class DataSourceManager
	{
		private DbDataSourceRepository rep = new DbDataSourceRepository();
		private static List<CodeDataSource> codeDataSources = new List<CodeDataSource>();

		#region <=== Cache Related ===>

		private const string CACHE_KEY = "DATASOURCES";

		private static IMemoryCache cache;

		static DataSourceManager()
		{
			var cacheOptions = new MemoryCacheOptions();
			cacheOptions.ExpirationScanFrequency = TimeSpan.FromHours(1);
			cache = new MemoryCache(cacheOptions);

			InitCodeDataSources();
		}

		private static void AddToCache(List<DataSourceBase> dataSources)
		{
			var options = new MemoryCacheEntryOptions();
			options.SetAbsoluteExpiration(TimeSpan.FromHours(1));
			cache.Set(CACHE_KEY, dataSources, options);
		}

		private static List<DataSourceBase> GetFromCache()
		{
			object result = null;
			bool found = cache.TryGetValue(CACHE_KEY, out result);
			return result as List<DataSourceBase>;
		}

		internal static void RemoveFromCache()
		{
			cache.Remove(CACHE_KEY);
		} 

		#endregion

		private static void InitCodeDataSources()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
							.Where(a => !(a.FullName.ToLowerInvariant().StartsWith("microsoft.")
								|| a.FullName.ToLowerInvariant().StartsWith("system.")));

			foreach (var assembly in assemblies)
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (type.IsSubclassOf(typeof(CodeDataSource)))
					{
						if (type.IsAbstract)
							continue;

						var instance = (CodeDataSource)Activator.CreateInstance(type);

						if (codeDataSources.Any(x => x.Id == instance.Id))
							throw new Exception($"Multiple code data sources with same ID ('{instance.Id}'). This is not allowed.");

						codeDataSources.Add(instance);
					}
				}
			}
		}

		public DataSourceBase Get(Guid id)
		{
			return GetAll().SingleOrDefault(x => x.Id == id);
		}

		public List<DataSourceBase> GetAll()
		{
			var cached = GetFromCache();
			if (cached != null)
				return cached;

			List<DataSourceBase> result = new List<DataSourceBase>();
			result.AddRange(codeDataSources);

			DataTable dt = rep.GetAll();
			foreach (DataRow row in dt.Rows)
			{
				var ds = (DataSourceBase)row.MapTo<DatabaseDataSource>();
				if (result.Any(x => x.Id == ds.Id))
					throw new Exception($"Database data source have same  ID ('{ds.Id}') as already existing code data source. This is not allowed.");
				result.Add(ds);
			}

			AddToCache(result);
			return result;
		}

		private DatabaseDataSource GetDatabaseDataSourceById(Guid id)
		{
			DataRow dr = rep.Get(id);
			if (dr == null)
				return null;

			return dr.MapTo<DatabaseDataSource>();
		}

		private DataSourceBase GetDatabaseDataSourceByName(string name)
		{
			DataRow dr = rep.Get(name);
			if (dr == null)
				return null;

			return dr.MapTo<DatabaseDataSource>();
		}

		public DatabaseDataSource Create(string name, string description, int weight, string eql, string parameters)
		{
			ValidationException validation = new ValidationException();

			List<DataSourceParameter> dsParams = ProcessParametersText(parameters);
			List<EqlParameter> eqlParams = new List<EqlParameter>();
			foreach (var dsPar in dsParams)
				eqlParams.Add(ConvertDataSourceParameterToEqlParameter(dsPar));

			EqlBuilder builder = new EqlBuilder(eql);
			var result = builder.Build(eqlParams);
			if (result.Errors.Count > 0)
			{
				foreach (var err in result.Errors)
				{
					if (err.Line.HasValue || err.Column.HasValue)
						validation.AddError("eql", $"{err.Message} [{err.Line},{err.Column}]");
					else
						validation.AddError("eql", err.Message);
				}
			}
			validation.CheckAndThrow();

			foreach (var par in result.Parameters)
			{
				if (!eqlParams.Any(x => x.ParameterName == par.ParameterName))
				{
					validation.AddError("parameters", $"Parameter '{par.ParameterName}' is missing.");
				}
			}
			validation.CheckAndThrow();

			DatabaseDataSource ds = new DatabaseDataSource();
			ds.Id = Guid.NewGuid();
			ds.Name = name;
			ds.Description = description;
			ds.EqlText = eql;
			ds.SqlText = result.Sql;
			ds.EntityName = result.FromEntity.Name;
			ds.Parameters.AddRange(dsParams);
			ds.Fields.AddRange(ProcessFieldsMeta(result.Meta));

			if (string.IsNullOrWhiteSpace(ds.Name))
				validation.AddError("name", "Name is required.");
			else if (GetDatabaseDataSourceByName(ds.Name) != null)
				validation.AddError("name", "DataSource record with same name already exists.");

			if (string.IsNullOrWhiteSpace(ds.EqlText))
				validation.AddError("eql", "Eql is required.");

			if (string.IsNullOrWhiteSpace(ds.SqlText))
				validation.AddError("sql", "Sql is required.");

			validation.CheckAndThrow();

			rep.Create(ds.Id, ds.Name, ds.Description, ds.Weight, ds.EqlText, ds.SqlText,
				JsonConvert.SerializeObject(ds.Parameters), JsonConvert.SerializeObject(ds.Fields), ds.EntityName);
			
			RemoveFromCache();

			return rep.Get(ds.Id).MapTo<DatabaseDataSource>();
		}

		public DatabaseDataSource Update(Guid id, string name, string description, int weight, string eql, string parameters)
		{
			ValidationException validation = new ValidationException();

			if (string.IsNullOrWhiteSpace(eql))
				throw new ArgumentException(nameof(eql));

			List<EqlParameter> eqlParams = new List<EqlParameter>();
			List<DataSourceParameter> dsParams = new List<DataSourceParameter>();
			if (!string.IsNullOrWhiteSpace(parameters))
			{
				dsParams = ProcessParametersText(parameters);
				foreach (var dsPar in dsParams)
					eqlParams.Add(ConvertDataSourceParameterToEqlParameter(dsPar));
			}

			EqlBuilder builder = new EqlBuilder(eql);
			var result = builder.Build(eqlParams);
			if (result.Errors.Count > 0)
			{
				foreach (var err in result.Errors)
				{
					if (err.Line.HasValue || err.Column.HasValue)
						validation.AddError("eql", $"{err.Message} [{err.Line},{err.Column}]");
					else
						validation.AddError("eql", err.Message);
				}
			}
			validation.CheckAndThrow();

			foreach (var par in result.Parameters)
			{
				if (!eqlParams.Any(x => x.ParameterName == par.ParameterName))
				{
					validation.AddError("parameters", $"Parameter '{par.ParameterName}' is missing.");
				}
			}
			validation.CheckAndThrow();

			DatabaseDataSource ds = new DatabaseDataSource();
			ds.Id = id;
			ds.Name = name;
			ds.Description = description;
			ds.EqlText = eql;
			ds.SqlText = result.Sql;
			ds.EntityName = result.FromEntity.Name;
			ds.Parameters.AddRange(dsParams);
			ds.Fields.AddRange(ProcessFieldsMeta(result.Meta));

			if (string.IsNullOrWhiteSpace(ds.Name))
				validation.AddError("name", "Name is required.");
			else
			{
				var existingDS = GetDatabaseDataSourceByName(ds.Name);
				if (existingDS != null && existingDS.Id != ds.Id)
					validation.AddError("name", "Another DataSource with same name already exists.");
			}

			if (string.IsNullOrWhiteSpace(ds.EqlText))
				validation.AddError("eql", "Eql is required.");

			if (string.IsNullOrWhiteSpace(ds.SqlText))
				validation.AddError("sql", "Sql is required.");


			validation.CheckAndThrow();

			rep.Update(ds.Id, ds.Name, ds.Description, ds.Weight, ds.EqlText, ds.SqlText,
				JsonConvert.SerializeObject(ds.Parameters), JsonConvert.SerializeObject(ds.Fields), ds.EntityName);

			RemoveFromCache();

			return GetDatabaseDataSourceById(ds.Id);
		}

		private List<DataSourceModelFieldMeta> ProcessFieldsMeta(List<EqlFieldMeta> fields)
		{
			List<DataSourceModelFieldMeta> result = new List<DataSourceModelFieldMeta>();

			if (fields == null)
				return result;

			foreach (var fieldMeta in fields)
			{
				DataSourceModelFieldMeta dsMeta = new DataSourceModelFieldMeta();
				dsMeta.EntityName = string.Empty;
				if (fieldMeta.Relation != null)
				{
					dsMeta.Name = "$" + fieldMeta.Relation.Name;
					dsMeta.Type = FieldType.RelationField;
				}
				if (fieldMeta.Field != null)
				{
					dsMeta.Name = fieldMeta.Field.Name;
					dsMeta.Type = fieldMeta.Field.GetFieldType();
				}

				dsMeta.Children.AddRange(ProcessFieldsMeta(fieldMeta.Children));
				result.Add(dsMeta);
			}

			return result;
		}

		private List<DataSourceParameter> ProcessParametersText(string parameters)
		{
			List<DataSourceParameter> dsParams = new List<DataSourceParameter>();

			if (string.IsNullOrWhiteSpace(parameters))
				return dsParams;

			foreach (var line in parameters.Split("\n", StringSplitOptions.RemoveEmptyEntries))
			{
				var parts = line.Replace("\r", "").Split(",", StringSplitOptions.RemoveEmptyEntries);
				if (parts.Count() < 3 || parts.Count() > 4)
					throw new Exception("Invalid parameter description: " + line);

				DataSourceParameter dsPar = new DataSourceParameter();
				dsPar.Name = parts[0].Trim();
				dsPar.Type = parts[1].ToLowerInvariant().Trim();
				if (string.IsNullOrWhiteSpace(dsPar.Type))
					throw new Exception("Invalid parameter type in: " + line);

				dsPar.Value = parts[2].Trim();
				if(parts.Count() == 4)
				{
					try
					{
						dsPar.IgnoreParseErrors = bool.Parse(parts[3]);
					}
					catch
					{
						dsPar.IgnoreParseErrors = false;
					}
				}
				dsParams.Add(dsPar);
			}
			return dsParams;
		}

		public string ConvertParamsToText(List<DataSourceParameter> parameters)
		{
			var result = "";
			foreach (var param in parameters)
			{
				if( param.IgnoreParseErrors )
					result += $"{param.Name},{param.Type},{param.Value},true" + Environment.NewLine;
				else
					result += $"{param.Name},{param.Type},{param.Value}" + Environment.NewLine;
			}

			return result;
		}

		public EqlParameter ConvertDataSourceParameterToEqlParameter(DataSourceParameter dsParameter)
		{
			var parName = dsParameter.Name;
			if (!parName.StartsWith("@"))
				parName = "@" + parName;

			return new EqlParameter(parName, GetDataSourceParameterValue(dsParameter), dsParameter.Type );
		}


		public object GetDataSourceParameterValue(DataSourceParameter dsParameter)
		{
			switch (dsParameter.Type.ToLower())
			{
				case "guid":
					{
						if (string.IsNullOrWhiteSpace(dsParameter.Value))
							return null;

						if (dsParameter.Value.ToLowerInvariant() == "null")
							return null;

						if (dsParameter.Value.ToLowerInvariant() == "guid.empty")
							return Guid.Empty;
						
						if (Guid.TryParse(dsParameter.Value, out Guid value))
							return value;

						if (dsParameter.IgnoreParseErrors)
							return null;

						throw new Exception($"Invalid Guid value for parameter: " + dsParameter.Name);
					}
				case "int":
					{
						if (string.IsNullOrWhiteSpace(dsParameter.Value))
							return null;

						if (Int32.TryParse(dsParameter.Value, out int value))
							return value;

						if (dsParameter.Value.ToLowerInvariant() == "null")
							return null;

						if (dsParameter.IgnoreParseErrors)
							return null;

						throw new Exception($"Invalid int value for parameter: " + dsParameter.Name);
					}
				case "decimal":
					{
						if (string.IsNullOrWhiteSpace(dsParameter.Value))
							return null;

						if (Decimal.TryParse(dsParameter.Value, out decimal value))
							return value;

						if (dsParameter.IgnoreParseErrors)
							return null;

						throw new Exception($"Invalid decimal value for parameter: " + dsParameter.Name);
					}
				case "date":
					{
						if (string.IsNullOrWhiteSpace(dsParameter.Value))
							return null;

						if (dsParameter.Value.ToLowerInvariant() == "null")
							return null;

						if (dsParameter.Value.ToLowerInvariant() == "now")
							return DateTime.Now;

						if (dsParameter.Value.ToLowerInvariant() == "utc_now")
							return DateTime.UtcNow;

						if (DateTime.TryParse(dsParameter.Value, out DateTime value))
							return value;

						if (dsParameter.IgnoreParseErrors)
							return null;

						throw new Exception($"Invalid datetime value for parameter: " + dsParameter.Name);
					}
				case "text":
					{
						if (dsParameter.Value.ToLowerInvariant() == "null")
							return null;

						if (dsParameter.Value.ToLowerInvariant() == "string.empty")
							return String.Empty;

						if (dsParameter.IgnoreParseErrors)
							return null;

						return dsParameter.Value;
					}
				case "bool":
					{
						if (dsParameter.Value.ToLowerInvariant() == "null")
							return null;

						if (dsParameter.Value.ToLowerInvariant() == "true")
							return true;

						if (dsParameter.Value.ToLowerInvariant() == "false")
							return false;

						if (dsParameter.IgnoreParseErrors)
							return null;

						throw new Exception($"Invalid boolean value for parameter: " + dsParameter.Name);
					}
				default:
					throw new Exception($"Invalid parameter type '{dsParameter.Type}' for '{dsParameter.Name}'");
			}
		}

		public void Delete(Guid id)
		{
			rep.Delete(id);
			RemoveFromCache();
		}

		public EntityRecordList Execute(Guid id, List<EqlParameter> parameters = null)
		{
			var ds = Get(id);
			if (ds == null)
				throw new Exception("DataSource not found.");

			if (parameters == null)
				parameters = new List<EqlParameter>();

			foreach (var par in ds.Parameters)
				if (!(parameters.Any(x => x.ParameterName == par.Name) || parameters.Any(x => x.ParameterName == "@" + par.Name)))
					parameters.Add(new EqlParameter(par.Name, par.Value));

			if (ds is DatabaseDataSource)
				return new EqlCommand(((DatabaseDataSource)ds).EqlText, parameters).Execute();
			else if (ds is CodeDataSource)
			{
				var args = new Dictionary<string, object>();
				foreach (var param in parameters)
				{
					args[param.ParameterName] = param.Value;
				}
				var codeDs = (CodeDataSource)ds;
				return (EntityRecordList)codeDs.Execute(args);
			}
			else
				throw new NotImplementedException();
		}

		public EntityRecordList Execute(string eql, string parameters = null)
		{
			if (string.IsNullOrWhiteSpace(eql))
				throw new ArgumentException(nameof(eql));

			List<EqlParameter> eqlParams = new List<EqlParameter>();
			if (!string.IsNullOrWhiteSpace(parameters))
			{
				List<DataSourceParameter> dsParams = ProcessParametersText(parameters);
				foreach (var dsPar in dsParams)
					eqlParams.Add(ConvertDataSourceParameterToEqlParameter(dsPar));
			}
			return new EqlCommand(eql, eqlParams).Execute();
		}

		public string GenerateSql(string eql, string parameters)
		{
			ValidationException validation = new ValidationException();
			List<DataSourceParameter> dsParams = ProcessParametersText(parameters);
			List<EqlParameter> eqlParams = new List<EqlParameter>();
			foreach (var dsPar in dsParams)
				eqlParams.Add(ConvertDataSourceParameterToEqlParameter(dsPar));

			EqlBuilder builder = new EqlBuilder(eql);
			var result = builder.Build(eqlParams);
			if (result.Errors.Count > 0)
			{
				foreach (var err in result.Errors)
				{
					if (err.Line.HasValue || err.Column.HasValue)
						validation.AddError("eql", $"{err.Message} [{err.Line},{err.Column}]");
					else
						validation.AddError("eql", err.Message);
				}
			}
			validation.CheckAndThrow();

			return result.Sql;
		}
	}
}
