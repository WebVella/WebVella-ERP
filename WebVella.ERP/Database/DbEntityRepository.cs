using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Database.Models;

namespace WebVella.ERP.Database
{
	public class DbEntityRepository
	{
		static object lockObj = new object();
		static List<DbEntity> entityCache = new List<DbEntity>();
		internal const string RECORD_COLLECTION_PREFIX = "rec_";

		public bool Create(DbEntity entity)
		{
			lock (lockObj)
			{
				using (DbConnection con = DbContext.Current.CreateConnection())
				{
					con.BeginTransaction();

					try
					{
						List<DbParameter> parameters = new List<DbParameter>();

						JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

						DbParameter parameterId = new DbParameter();
						parameterId.Name = "id";
						parameterId.Value = entity.Id;
						parameterId.Type = NpgsqlDbType.Uuid;
						parameters.Add(parameterId);

						DbParameter parameterJson = new DbParameter();
						parameterJson.Name = "json";
						parameterJson.Value = JsonConvert.SerializeObject(entity, settings);
						parameterJson.Type = NpgsqlDbType.Json;
						parameters.Add(parameterJson);

						string tableName = RECORD_COLLECTION_PREFIX + entity.Name;
						
						DbRepository.CreateTable(tableName);
						foreach (var field in entity.Fields)
						{
							bool isPrimaryKey = field.Name.ToLowerInvariant() == "id";
							FieldType fieldType = field.GetFieldType();
							DbRepository.CreateColumn(tableName, field.Name, fieldType, isPrimaryKey, field.GetDefaultValue(), !field.Required);
						}

						bool result = DbRepository.InsertRecord("entities", parameters);

						if(!result)
							throw new Exception("Entity record was not added!");

						entityCache = new List<DbEntity>();

						con.CommitTransaction();

						return result;
					}
					catch(Exception e)
					{
						con.RollbackTransaction();
					}
				}
				return false;
			}
		}

		public bool Update(DbEntity entity)
		{
			lock (lockObj)
			{

				using (DbConnection con = DbContext.Current.CreateConnection())
				{
					NpgsqlCommand command = con.CreateCommand("UPDATE entities SET json=@json WHERE id=@id;");

					JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

					var parameter = command.CreateParameter() as NpgsqlParameter;
					parameter.ParameterName = "json";
					parameter.Value = JsonConvert.SerializeObject(entity, settings);
					parameter.NpgsqlDbType = NpgsqlDbType.Json;
					command.Parameters.Add(parameter);

					var parameterId = command.CreateParameter() as NpgsqlParameter;
					parameterId.ParameterName = "id";
					parameterId.Value = entity.Id;
					parameterId.NpgsqlDbType = NpgsqlDbType.Uuid;
					command.Parameters.Add(parameterId);

					entityCache = new List<DbEntity>();

					return command.ExecuteNonQuery() > 0;
				}
			}
		}

		public DbEntity Read(Guid entityId)
		{
			List<DbEntity> entities = Read();
			return entities.FirstOrDefault(e => e.Id == entityId);
		}

		public DbEntity Read(string entityName)
		{
			List<DbEntity> entities = Read();
			return entities.FirstOrDefault(e => e.Name.ToLowerInvariant() == entityName.ToLowerInvariant());
		}

		public List<DbEntity> Read()
		{
			lock (lockObj)
			{
				if (entityCache.Any())
					return entityCache;

				Debug.WriteLine("READ ENTITIIES:" + Thread.CurrentThread.ManagedThreadId);
				using (DbConnection con = DbContext.Current.CreateConnection())
				{
					NpgsqlCommand command = con.CreateCommand("SELECT json FROM entities;");

					using (NpgsqlDataReader reader = command.ExecuteReader())
					{

						JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
						List<DbEntity> entities = new List<DbEntity>();
						while (reader.Read())
						{
							DbEntity entity = JsonConvert.DeserializeObject<DbEntity>(reader[0].ToString(), settings);
							entities.Add(entity);
						}

						entityCache = new List<DbEntity>(entities.ToArray());

						reader.Close();
						return entities;
					}
				}
			}
		}

		public bool Delete(Guid entityId)
		{
			lock (lockObj)
			{
				using (DbConnection con = DbContext.Current.CreateConnection())
				{
					NpgsqlCommand command = con.CreateCommand("DELETE FROM entities WHERE id=@id;");

					var parameterId = command.CreateParameter() as NpgsqlParameter;
					parameterId.ParameterName = "id";
					parameterId.Value = entityId;
					parameterId.NpgsqlDbType = NpgsqlDbType.Uuid;
					command.Parameters.Add(parameterId);

					entityCache = new List<DbEntity>();

					return command.ExecuteNonQuery() > 0;
				}
			}
		}
	}
}
