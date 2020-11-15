using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Database
{
	public class DbSystemSettingsRepository
	{
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
		public DbSystemSettingsRepository(DbContext currentContext)
		{
			if (currentContext != null)
				suppliedContext = currentContext;
		}
		public DbSystemSettings Read()
		{
			DbSystemSettings setting = null;

			using (DbConnection con = CurrentContext.CreateConnection())
			{
				try
				{
					con.BeginTransaction();
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
					con.CommitTransaction();
				}
				catch (Exception ex)
				{
					if (con != null)
						con.RollbackTransaction();

					if (!ex.Message.Contains("does not exist"))
						throw;
				}
			}
			return setting;
		}

		public bool Save(DbSystemSettings systemSettings)
		{
			if (systemSettings == null)
				throw new ArgumentNullException("systemSettings");

			using (DbConnection con = CurrentContext.CreateConnection())
			{
				bool recordExists = false;
				NpgsqlCommand command = con.CreateCommand("SELECT COUNT(*) FROM system_settings WHERE id=@id;");
				var parameterId = command.CreateParameter();
				parameterId.ParameterName = "id";
				parameterId.Value = systemSettings.Id;
				parameterId.NpgsqlDbType = NpgsqlDbType.Uuid;
				command.Parameters.Add(parameterId);

				recordExists = ((long)command.ExecuteScalar()) > 0;

				if (recordExists)
					command = con.CreateCommand("UPDATE system_settings SET version=@version WHERE id=@id;");
				else
					command = con.CreateCommand("INSERT INTO system_settings (id, version) VALUES( @id,@version)");

				var parameter = command.CreateParameter();
				parameter.ParameterName = "version";
				parameter.Value = systemSettings.Version;
				parameter.NpgsqlDbType = NpgsqlDbType.Integer;
				command.Parameters.Add(parameter);

				parameterId = command.CreateParameter();
				parameterId.ParameterName = "id";
				parameterId.Value = systemSettings.Id;
				parameterId.NpgsqlDbType = NpgsqlDbType.Uuid;
				command.Parameters.Add(parameterId);

				return command.ExecuteNonQuery() > 0;
			}
		}
	}
}
