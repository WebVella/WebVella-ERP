using Npgsql;
using System;
using System.Data;

namespace WebVella.Erp.Web.Repositories
{
	internal class PageDataSourceRepository : BaseDbRepository
	{
		public PageDataSourceRepository(string conString) : base(conString) { }

		/// <summary>
		/// Gets page data source record for specified id
		/// </summary>
		/// <param name="dataSourceId"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public DataRow GetById(Guid id, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM app_page_data_source WHERE id = @id");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@id", id));

			DataTable resultTable;
			if (transaction != null)
				resultTable = ExecuteSqlQueryCommand(transaction, command);
			else
				resultTable = ExecuteSqlQueryCommand(command);

			if (resultTable.Rows.Count == 0)
				return null;

			return resultTable.Rows[0];
		}

		/// <summary>
		/// Gets page data source records for specified page id
		/// </summary>
		/// <param name="dataSourceId"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public DataTable GetByPageId(Guid pageId, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM app_page_data_source WHERE page_id = @page_id");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@page_id", pageId));

			if (transaction != null)
				return ExecuteSqlQueryCommand(transaction, command);
			else
				return ExecuteSqlQueryCommand(command);
		}

		/// <summary>
		/// Gets page data source records for specified data source id
		/// </summary>
		/// <param name="dataSourceId"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public DataTable GetByDataSourceId(Guid dataSourceId, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM app_page_data_source WHERE data_source_id = @data_source_id");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@data_source_id", dataSourceId));

			if (transaction != null)
				return ExecuteSqlQueryCommand(transaction, command);
			else
				return ExecuteSqlQueryCommand(command);
		}

		/// <summary>
		/// Inserts new page data source record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="pageId"></param>
		/// <param name="dataSourceId"></param>
		/// <param name="name"></param>
		/// <param name="parametersJson"></param>
		/// <param name="transaction"></param>
		public void Insert(Guid id, Guid pageId, Guid dataSourceId, string name, string parametersJson, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand(
					"INSERT INTO app_page_data_source (id,page_id,data_source_id,name,parameters) " +
					"VALUES(@id,@page_id,@data_source_id,@name,@parameters)");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@page_id", pageId));
			command.Parameters.Add(new NpgsqlParameter("@data_source_id", dataSourceId));
			command.Parameters.Add(new NpgsqlParameter("@name", name));
			command.Parameters.Add(new NpgsqlParameter("@parameters", parametersJson ));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Update page data source record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="pageId"></param>
		/// <param name="dataSourceId"></param>
		/// <param name="name"></param>
		/// <param name="parametersJson"></param>
		/// <param name="transaction"></param>
		public void Update(Guid id, Guid pageId, Guid dataSourceId, string name, string parametersJson, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand(
					"UPDATE app_page_data_source " +
					"SET name = @name, parameters = @parameters, page_id = @page_id, data_source_id = @data_source_id" +
					" WHERE id = @id");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@page_id", pageId));
			command.Parameters.Add(new NpgsqlParameter("@data_source_id", dataSourceId));
			command.Parameters.Add(new NpgsqlParameter("@name", name));
			command.Parameters.Add(new NpgsqlParameter("@parameters", parametersJson));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Deletes page data source record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		public void Delete(Guid id, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("DELETE FROM app_page_data_source WHERE id = @id");
			command.Parameters.Add(new NpgsqlParameter("@id", id));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}
	}
}
