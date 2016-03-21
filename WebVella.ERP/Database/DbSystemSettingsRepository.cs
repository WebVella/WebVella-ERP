using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Database
{
	public class DbSystemSettingsRepository
	{
		public DbSystemSettings Read()
		{
			DbSystemSettings setting = null;

			using (DbConnection con = DbContext.Current.CreateConnection())
			{
				NpgsqlCommand command = con.CreateCommand("SELECT * FROM system_settings;");

				using (var reader = command.ExecuteReader())
				{
					if (reader != null && reader.Read())
					{
						setting = new DbSystemSettings();
						setting.Id = (Guid)reader["id"];
						setting.Version = (int)reader["version"];
					}
					reader.Close();
				}
			}

			return setting;
		}

		public bool Save(DbSystemSettings systemSettings)
		{
			if (systemSettings == null)
				throw new ArgumentNullException("systemSettings");

			using (DbConnection con = DbContext.Current.CreateConnection())
			{
				NpgsqlCommand command = con.CreateCommand("UPDATE system_settings SET version=@version WHERE id=@id;");

				var parameter = command.CreateParameter() as NpgsqlParameter;
				parameter.ParameterName = "version";
				parameter.Value = systemSettings.Version;
				parameter.NpgsqlDbType = NpgsqlDbType.Integer;
				command.Parameters.Add(parameter);

				var parameterId = command.CreateParameter() as NpgsqlParameter;
				parameterId.ParameterName = "id";
				parameterId.Value = systemSettings.Id;
				parameterId.NpgsqlDbType = NpgsqlDbType.Uuid;
				command.Parameters.Add(parameterId);

				return command.ExecuteNonQuery() > 0;
			}
		}
	}
}
