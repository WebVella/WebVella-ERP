using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;

namespace WebVella.Erp.Eql
{
	public class EqlCommand
	{
		/// <summary>
		/// Eql text
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// DbConnection object
		/// </summary>
		public DbConnection Connection { get; private set; }

		/// <summary>
		/// NpgsqlConnection object
		/// </summary>
		public NpgsqlConnection NpgConnection { get; private set; }

		/// <summary>
		/// NpgsqlConnection object
		/// </summary>
		public NpgsqlTransaction NpgTransaction { get; private set; }

		/// <summary>
		/// List of EqlParameters
		/// </summary>
		public List<EqlParameter> Parameters { get; private set; } = new List<EqlParameter>();

		/// <summary>
		/// EqlSettings object
		/// </summary>
		public EqlSettings Settings { get; private set; } = new EqlSettings();

		private DbContext suppliedContext = null;
		public DbContext CurrentContext
		{
			get
			{
				if (suppliedContext != null)
					return suppliedContext;
				else
					return DbContext.Current;
			}
			set
			{
				suppliedContext = value;
			}
		}

		/// <summary>
		/// Creates command
		/// </summary>
		public EqlCommand(string text, params EqlParameter[] parameters)
		{
			Text = text;

			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentException("Command text cannot be null or empty.");

			NpgConnection = null;

			Connection = null;

			if (parameters != null && parameters.Length > 0)
				Parameters.AddRange(parameters);
		}

		/// <summary>
		/// Creates command
		/// </summary>
		public EqlCommand(string text, EqlSettings settings, params EqlParameter[] parameters) : this(text, parameters)
		{
			if (settings != null)
				Settings = settings;
		}

		/// <summary>
		/// Creates command
		/// </summary>
		public EqlCommand(string text, DbContext currentContext, params EqlParameter[] parameters) : this(text, parameters)
		{
			if (currentContext != null)
				suppliedContext = currentContext;
		}

		/// <summary>
		/// Creates command
		/// </summary>
		public EqlCommand(string text, DbContext currentContext, EqlSettings settings, params EqlParameter[] parameters) : this(text, currentContext, parameters)
		{
			if (settings != null)
				Settings = settings;
		}

		/// <summary>
		/// Creates command
		/// </summary>
		public EqlCommand(string text, List<EqlParameter> parameters = null, DbContext currentContext = null)
		{
			if (currentContext != null)
				suppliedContext = currentContext;
			Text = text;

			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentException("Command text cannot be null or empty.");

			NpgConnection = null;

			Connection = null;

			if (parameters != null)
				Parameters.AddRange(parameters);
		}

		public EqlCommand(string text, EqlSettings settings, List<EqlParameter> parameters = null, DbContext currentContext = null)
		: this(text, parameters, currentContext)
		{
			if (settings != null)
				Settings = settings;
		}

		/// <summary>
		/// Creates command
		/// </summary>
		/// <param name="text"></param>
		/// <param name="parameters"></param>
		public EqlCommand(string text, DbConnection connection, List<EqlParameter> parameters = null, DbContext currentContext = null)
		{
			if (currentContext != null)
				suppliedContext = currentContext;
			Text = text;

			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentException("Command text cannot be null or empty.");

			NpgConnection = null;

			Connection = connection;

			if (connection == null)
				throw new ArgumentNullException(nameof(connection));

			if (parameters != null)
				Parameters.AddRange(parameters);
		}

		/// <summary>
		/// Creates command
		/// </summary>
		/// <param name="text"></param>
		/// <param name="parameters"></param>
		public EqlCommand(string text, NpgsqlConnection connection, NpgsqlTransaction transaction = null, List<EqlParameter> parameters = null, DbContext currentContext = null)
		{
			if (currentContext != null)
				suppliedContext = currentContext;

			Text = text;

			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentException("Command text cannot be null or empty.");

			Connection = null;

			NpgConnection = connection;
			NpgTransaction = transaction;

			if (connection == null)
				throw new ArgumentNullException(nameof(connection));

			if (parameters != null)
				Parameters.AddRange(parameters);
		}

		/// <summary>
		/// Executes the command to database
		/// </summary>
		/// <returns></returns>
		public EntityRecordList Execute()
		{
			EqlBuilder eqlBuilder = new EqlBuilder(Text, CurrentContext, Settings);
			var eqlBuildResult = eqlBuilder.Build(Parameters);

			if (eqlBuildResult.Errors.Count > 0)
				throw new EqlException(eqlBuildResult.Errors);

			if (CurrentContext == null)
				throw new EqlException("DbContext need to be created.");

			EntityRecordList result = new EntityRecordList();

			DataTable dt = new DataTable();
			var npgsParameters = eqlBuildResult.Parameters.Select(x => x.ToNpgsqlParameter()).ToList();
			NpgsqlCommand command = null;

			if (Connection != null)
				command = Connection.CreateCommand(eqlBuildResult.Sql, parameters: npgsParameters);
			else if (NpgConnection != null)
			{
				if (NpgTransaction != null)
					command = new NpgsqlCommand(eqlBuildResult.Sql, NpgConnection, NpgTransaction);
				else
					command = new NpgsqlCommand(eqlBuildResult.Sql, NpgConnection);
				command.Parameters.AddRange(npgsParameters.ToArray());
			}
			else
			{
				if (CurrentContext == null)
					throw new EqlException("DbContext needs to be initialized before using EqlCommand without supplying connection.");

				using (var connection = CurrentContext.CreateConnection())
				{
					command = connection.CreateCommand(eqlBuildResult.Sql, parameters: npgsParameters);
					command.CommandTimeout = 600;
					new NpgsqlDataAdapter(command).Fill(dt);

					foreach (DataRow dr in dt.Rows)
					{
						var jObj = JObject.Parse((string)dr[0]);
						if (result.TotalCount == 0 && jObj.ContainsKey("___total_count___"))
							result.TotalCount = int.Parse(((JValue)jObj["___total_count___"]).ToString());
						result.Add(ConvertJObjectToEntityRecord(jObj, eqlBuildResult.Meta));
					}

					return result;
				}
			}

			command.CommandTimeout = 600;
			new NpgsqlDataAdapter(command).Fill(dt);
			foreach (DataRow dr in dt.Rows)
			{
				var jObj = JObject.Parse((string)dr[0]);
				if (result.TotalCount == 0 && jObj.ContainsKey("___total_count___"))
					result.TotalCount = int.Parse(((JValue)jObj["___total_count___"]).ToString());
				result.Add(ConvertJObjectToEntityRecord(jObj, eqlBuildResult.Meta));
			}

			return result;
		}

		/// <summary>
		/// Gets field meta
		/// </summary>
		/// <returns></returns>
		public List<EqlFieldMeta> GetMeta()
		{
			EqlBuilder eqlBuilder = new EqlBuilder(Text, CurrentContext, Settings);
			var eqlBuildResult = eqlBuilder.Build(Parameters);

			if (eqlBuildResult.Errors.Count > 0)
				throw new EqlException(eqlBuildResult.Errors);

			return eqlBuildResult.Meta;
		}

		/// <summary>
		/// Gets sql
		/// </summary>
		/// <returns></returns>
		public string GetSql()
		{
			EqlBuilder eqlBuilder = new EqlBuilder(Text, CurrentContext, Settings);
			var eqlBuildResult = eqlBuilder.Build(Parameters);

			if (eqlBuildResult.Errors.Count > 0)
				throw new EqlException(eqlBuildResult.Errors);

			return eqlBuildResult.Sql;
		}

		private EntityRecord ConvertJObjectToEntityRecord(JObject jObj, List<EqlFieldMeta> fieldMeta)
		{
			EntityRecord record = new EntityRecord();
			foreach (EqlFieldMeta meta in fieldMeta)
			{
				if (meta.Field != null)
				{
					record[meta.Field.Name] = DbRecordRepository.ExtractFieldValue(jObj[meta.Field.Name], meta.Field);
				}
				else if (meta.Relation != null)
				{
					List<EntityRecord> relRecords = new List<EntityRecord>();
					JArray relatedJsonRecords = jObj[meta.Name].Value<JArray>();
					foreach (JObject relatedObj in relatedJsonRecords)
						relRecords.Add(ConvertJObjectToEntityRecord(relatedObj, meta.Children));

					record[meta.Name] = relRecords;
				}
			}
			return record;
		}
	}
}
