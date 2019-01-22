using Npgsql;
using System;
using System.Data;

namespace WebVella.Erp.Database
{
	public class DbDataSourceRepository
	{
		/// <summary>
		/// Gets data source record by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public DataRow Get(Guid id)
		{
			using (DbConnection con = DbContext.Current.CreateConnection())
			{
				var command = con.CreateCommand(@"SELECT * FROM public.data_source WHERE id = @id");
				command.Parameters.Add(new NpgsqlParameter("@id", id));
				DataTable dt = new DataTable();
				new NpgsqlDataAdapter(command).Fill(dt);

				if (dt.Rows.Count > 0)
					return dt.Rows[0];

				return null;
			}
		}

		/// <summary>
		/// Gets data source record by name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public DataRow Get(string name)
		{
			using (DbConnection con = DbContext.Current.CreateConnection())
			{
				var command = con.CreateCommand(@"SELECT * FROM public.data_source WHERE name = @name");
				command.Parameters.Add(new NpgsqlParameter("@name", name));
				DataTable dt = new DataTable();
				new NpgsqlDataAdapter(command).Fill(dt);

				if (dt.Rows.Count > 0)
					return dt.Rows[0];

				return null;
			}
		}

		/// <summary>
		/// Gets all data source records
		/// </summary>
		/// <returns></returns>
		public DataTable GetAll()
		{
			using (DbConnection con = DbContext.Current.CreateConnection())
			{
				var command = con.CreateCommand(@"SELECT * FROM public.data_source");
				DataTable dt = new DataTable();
				new NpgsqlDataAdapter(command).Fill(dt);
				return dt;
			}
		}

		/// <summary>
		/// Creates new data source record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <param name="weight"></param>
		/// <param name="eqlTest"></param>
		/// <param name="sqlText"></param>
		/// <param name="parametersJson"></param>
		/// <param name="fieldsJson"></param>
		/// <param name="entityName"></param>
		/// <returns></returns>
		public bool Create(Guid id, string name, string description, int weight, string eqlTest,
							string sqlText, string parametersJson, string fieldsJson, string entityName)
		{
			using (DbConnection con = DbContext.Current.CreateConnection())
			{
				const string sql = @"INSERT INTO public.data_source(id, name, description, weight, eql_text, sql_text, parameters_json, fields_json, entity_name)
							VALUES (@id, @name, @description, @weight, @eql_text, @sql_text, @parameters_json, @fields_json, @entity_name)";

				var command = con.CreateCommand(sql);
				command.Parameters.Add(new NpgsqlParameter("@id", id));
				command.Parameters.Add(new NpgsqlParameter("@name", name));
				command.Parameters.Add(new NpgsqlParameter("@description", description ?? ""));
				command.Parameters.Add(new NpgsqlParameter("@weight", weight));
				command.Parameters.Add(new NpgsqlParameter("@eql_text", eqlTest));
				command.Parameters.Add(new NpgsqlParameter("@sql_text", sqlText));
				command.Parameters.Add(new NpgsqlParameter("@parameters_json", parametersJson));
				command.Parameters.Add(new NpgsqlParameter("@fields_json", fieldsJson));
				command.Parameters.Add(new NpgsqlParameter("@entity_name", entityName));
				return command.ExecuteNonQuery() > 0;
			}
		}

		/// <summary>
		/// Updates data source record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <param name="weight"></param>
		/// <param name="eqlTest"></param>
		/// <param name="sqlText"></param>
		/// <param name="parametersJson"></param>
		/// <param name="fieldsJson"></param>
		/// <param name="entityName"></param>
		/// <returns></returns>
		public bool Update(Guid id, string name, string description, int weight, string eqlTest,
							string sqlText, string parametersJson, string fieldsJson, string entityName)
		{
			using (DbConnection con = DbContext.Current.CreateConnection())
			{
				const string sql = @"UPDATE public.data_source SET
							name = @name, description = @description, weight= @weight, eql_text = @eql_text, 
							sql_text = @sql_text, parameters_json= @parameters_json, fields_json= @fields_json, entity_name=@entity_name
							WHERE id = @id";

				var command = con.CreateCommand(sql);
				command.Parameters.Add(new NpgsqlParameter("@id", id));
				command.Parameters.Add(new NpgsqlParameter("@name", name));
				command.Parameters.Add(new NpgsqlParameter("@description", description ?? ""));
				command.Parameters.Add(new NpgsqlParameter("@weight", weight));
				command.Parameters.Add(new NpgsqlParameter("@eql_text", eqlTest));
				command.Parameters.Add(new NpgsqlParameter("@sql_text", sqlText));
				command.Parameters.Add(new NpgsqlParameter("@parameters_json", parametersJson));
				command.Parameters.Add(new NpgsqlParameter("@fields_json", fieldsJson));
				command.Parameters.Add(new NpgsqlParameter("@entity_name", entityName));
				return command.ExecuteNonQuery() > 0;
			}
		}

		/// <summary>
		/// Deletes existing data source record
		/// </summary>
		/// <param name="id"></param>
		public void Delete(Guid id)
		{
			using (DbConnection con = DbContext.Current.CreateConnection())
			{
				var command = con.CreateCommand(@"DELETE FROM public.data_source WHERE id = @id");
				command.Parameters.Add(new NpgsqlParameter("@id", id));
				command.ExecuteNonQuery();
			}
		}
	}
}
