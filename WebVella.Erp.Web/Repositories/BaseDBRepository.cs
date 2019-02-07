using Npgsql;
using System.Data;

namespace WebVella.Erp.Web.Repositories
{
	internal class BaseDbRepository
	{
		protected string connectionString;

		public BaseDbRepository( string conString )
		{
			connectionString = conString;
		}

		protected DataTable ExecuteSqlQueryCommand(NpgsqlCommand command)
		{
			using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
			{
				try
				{
					con.Open();
					command.Connection = con;
					DataTable dataTable = new DataTable();
					new NpgsqlDataAdapter(command).Fill(dataTable);
					return dataTable;
				}
				finally
				{
					if (con != null)
						con.Close();
				}
			}
		}

		protected void ExecuteSqlNonQueryCommands(params NpgsqlCommand[] commands)
		{
			using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
			{
				NpgsqlTransaction transaction = null;
				try
				{
					con.Open();
					transaction = con.BeginTransaction();

					foreach (var command in commands)
					{
						command.Connection = con;
						command.Transaction = transaction;
						command.ExecuteNonQuery();
					}

					transaction.Commit();
				}
				catch
				{
					if (transaction != null)
						transaction.Rollback();
					throw;
				}
				finally
				{
					if (con != null)
						con.Close();
				}
			}
		}

		protected DataTable ExecuteSqlQueryCommand(NpgsqlTransaction transaction, NpgsqlCommand command)
		{
			command.Connection = transaction.Connection;
			command.Transaction = transaction;
			DataTable dataTable = new DataTable();
			new NpgsqlDataAdapter(command).Fill(dataTable);
			return dataTable;

		}

		protected void ExecuteSqlNonQueryCommands(NpgsqlTransaction transaction, params NpgsqlCommand[] commands)
		{
			foreach (var command in commands)
			{
				command.Connection = transaction.Connection;
				command.Transaction = transaction;
				command.ExecuteNonQuery();
			}

		}
	}
}
