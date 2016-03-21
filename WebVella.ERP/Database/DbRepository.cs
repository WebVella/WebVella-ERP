using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Database.Models;

namespace WebVella.ERP.Database
{
	public static class DbRepository
	{
		public static void CreateTable(string name)
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				string sql = $"CREATE TABLE {name} ();";

				NpgsqlCommand command = connection.CreateCommand(sql);

				command.ExecuteNonQuery();
			}
		}

		public static void RenameTable(string name, string newName)
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				string sql = $"ALTER TABLE {name} RENAME TO {newName};";

				NpgsqlCommand command = connection.CreateCommand(sql);

				command.ExecuteNonQuery();
			}
		}

		public static void DeleteTable(string name, bool cascade = false)
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				string cascadeCommand = cascade ? " CASCADE" : "";
				string sql = $"DROP TABLE IF EXISTS {name}{cascadeCommand};";

				NpgsqlCommand command = connection.CreateCommand(sql);

				command.ExecuteNonQuery();
			}
		}

		public static void CreateColumn(string tableName, string name, FieldType type, bool isPrimaryKey, string defaultValue, bool isNullable = false)
		{
			string pgType = DbTypeConverter.ConvertToDatabaseSqlType(type);
			CreateColumn(tableName, name, pgType, isPrimaryKey, defaultValue, isNullable);
		}

		public static void CreateColumn(string tableName, string name, string type, bool isPrimaryKey, string defaultValue, bool isNullable = false)
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				string canBeNull = isNullable && !isPrimaryKey ? "NULL" : "NOT NULL";
				string sql = $"ALTER TABLE {tableName} ADD COLUMN {name} {type} {canBeNull};";

				if (isPrimaryKey)
					sql += $"\r\n ALTER TABLE {tableName} ADD PRIMARY KEY ({name});";

				if (!string.IsNullOrWhiteSpace(defaultValue) && (defaultValue != "'00000000-0000-0000-0000-000000000000'"))
					sql += $"\r\n ALTER TABLE {tableName} ALTER COLUMN {name} SET DEFAULT {defaultValue};";

				NpgsqlCommand command = connection.CreateCommand(sql);

				command.ExecuteNonQuery();
			}
		}

		public static void RenameColumn(string tableName, string name, string newName)
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				string sql = $"ALTER TABLE {tableName} RENAME COLUMN {name} TO {newName};";

				NpgsqlCommand command = connection.CreateCommand(sql);

				command.ExecuteNonQuery();
			}
		}

		public static void DeleteColumn(string tableName, string name)
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				string sql = $"ALTER TABLE {tableName} DROP COLUMN IF EXISTS {name};";

				NpgsqlCommand command = connection.CreateCommand(sql);

				command.ExecuteNonQuery();
			}
		}

		public static void SetPrimaryKey(string tableName, List<string> columns)
		{
			if (columns.Count == 0)
				return;

			string keyNames = "";
			foreach (var col in columns)
			{
				keyNames += col + ", ";
			}
			keyNames = keyNames.Remove(keyNames.Length - 2, 2);

			using (var connection = DbContext.Current.CreateConnection())
			{
				string sql = $"ALTER TABLE {tableName} ADD PRIMARY KEY ({keyNames});";

				NpgsqlCommand command = connection.CreateCommand(sql);

				command.ExecuteNonQuery();
			}
		}

		public static void SetUniqueConstraint(string constraintName, string tableName, List<string> columns)
		{
			if (columns.Count == 0)
				return;

			string colNames = "";
			foreach (var col in columns)
			{
				colNames += col + ", ";
			}
			colNames = colNames.Remove(colNames.Length - 2, 2);

			using (var connection = DbContext.Current.CreateConnection())
			{
				string sql = $"ALTER TABLE {tableName} ADD CONSTRAINT {constraintName} UNIQUE ({colNames});";

				NpgsqlCommand command = connection.CreateCommand(sql);

				command.ExecuteNonQuery();
			}
		}

		public static void CreateRelation(string relName, string originTableName, string originFieldName, string targetTableName, string targetFieldName)
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				string sql = $"ALTER TABLE {targetTableName} ADD CONSTRAINT {relName} FOREIGN KEY ({targetFieldName}) REFERENCES {originTableName} ({originFieldName});";

				NpgsqlCommand command = connection.CreateCommand(sql);

				command.ExecuteNonQuery();
			}
		}

		public static void CreateNtoNRelation(string relName, string originTableName, string originFieldName, string targetTableName, string targetFieldName)
		{
			string relTableName = $"rel_{relName}";
			CreateTable(relTableName);
			CreateColumn(relTableName, "origin_id", FieldType.GuidField, false, null, false);
			CreateColumn(relTableName, "target_id", FieldType.GuidField, false, null, false);
			SetPrimaryKey(relTableName, new List<string> { "origin_id", "target_id" });
			CreateRelation($"{relName}_origin", originTableName, originFieldName, relTableName, "origin_id");
			CreateRelation($"{relName}_target", targetTableName, targetFieldName, relTableName, "target_id");
		}

		public static void RenameRelation(string tableName, string name, string newName)
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				string sql = $"ALTER TABLE {tableName} RENAME CONSTRAINT {name} TO {newName};";

				NpgsqlCommand command = connection.CreateCommand(sql);

				command.ExecuteNonQuery();
			}
		}

		public static void DeleteRelation(string relName, string tableName)
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				string sql = $"ALTER TABLE {tableName} DROP CONSTRAINT IF EXISTS {relName};";

				NpgsqlCommand command = connection.CreateCommand(sql);

				command.ExecuteNonQuery();
			}
		}

		public static void DeleteNtoNRelation(string relName, string originTableName, string targetTableName)
		{
			string relTableName = $"rel_{relName}";

			DeleteRelation($"{relName}_origin", originTableName);
			DeleteRelation($"{relName}_target", targetTableName);
			DeleteRelation($"{relName}_origin", relTableName);
			DeleteRelation($"{relName}_target", relTableName);
			DeleteTable(relTableName, false);
		}

		public static bool InsertRecord(string tableName, List<DbParameter> parameters)
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				NpgsqlCommand command = connection.CreateCommand("");

				string columns = "";
				string values = "";
				foreach (var param in parameters)
				{

					var parameter = command.CreateParameter() as NpgsqlParameter;
					parameter.ParameterName = param.Name;
					parameter.Value = param.Value;
					parameter.NpgsqlDbType = param.Type;
					command.Parameters.Add(parameter);

					columns += $"{param.Name}, ";
					values += $"@{param.Name}, ";
				}

				columns = columns.Remove(columns.Length - 2, 2);
				values = values.Remove(values.Length - 2, 2);

				command.CommandText = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

				return command.ExecuteNonQuery() > 0;
			}
		}

		public static bool UpdateRecord(string tableName, List<DbParameter> parameters)
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				NpgsqlCommand command = connection.CreateCommand("");

				string values = "";
				foreach (var param in parameters)
				{

					var parameter = command.CreateParameter() as NpgsqlParameter;
					parameter.ParameterName = param.Name;
					parameter.Value = param.Value;
					parameter.NpgsqlDbType = param.Type;
					command.Parameters.Add(parameter);

					values += $"{param.Name}=@{param.Name}, ";
				}

				values = values.Remove(values.Length - 2, 2);

				command.CommandText = $"UPDATE {tableName} SET {values} WHERE id=@id";

				return command.ExecuteNonQuery() > 0;
			}
		}

		public static bool DeleteRecord(string tableName, Guid id)
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				NpgsqlCommand command = connection.CreateCommand("");

				var parameter = command.CreateParameter() as NpgsqlParameter;
				parameter.ParameterName = "id";
				parameter.Value = id;
				parameter.NpgsqlDbType = NpgsqlDbType.Uuid;
				command.Parameters.Add(parameter);

				command.CommandText = $"DELETE FROM {tableName} WHERE id=@id";

				return command.ExecuteNonQuery() > 0;
			}
		}
	}
}
