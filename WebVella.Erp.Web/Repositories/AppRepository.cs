using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;

namespace WebVella.Erp.Web.Repositories
{
	internal class AppRepository : BaseDbRepository
	{
		public AppRepository(string conString) : base(conString) { }

		/// <summary>
		/// Gets record by id
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public DataRow GetById(Guid id, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.app WHERE id = @id");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@id", id));

			DataTable dt;
			if (transaction != null)
				dt = ExecuteSqlQueryCommand(transaction, command);
			else
				dt = ExecuteSqlQueryCommand(command);

			if (dt.Rows.Count == 1)
				return dt.Rows[0];

			return null;
		}

		/// <summary>
		/// Gets record by id
		/// </summary>
		/// <param name="name"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public DataRow GetByName(string name, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.app WHERE name = @name");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@name", name));

			DataTable dt;
			if (transaction != null)
				dt = ExecuteSqlQueryCommand(transaction, command);
			else
				dt = ExecuteSqlQueryCommand(command);

			if (dt.Rows.Count == 1)
				return dt.Rows[0];

			return null;
		}

		/// <summary>
		/// Returns all application json
		/// </summary>
		/// <returns></returns>
		public List<JToken> GetAllCompleteAppJson()
		{
			#region <--- sql --->
			const string sql = @"SELECT row_to_json( X ) FROM (
				   SELECT app.id, app.name, app.label, app.description, app.icon_class, app.author, app.color, app.weight, app.access,
       	
       				-- pages
					(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
							SELECT page.*
							FROM app_page page
							WHERE page.app_id = app.id 
						) d ) AS pages,
			
					-- areas
					(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
					 SELECT area.*,
                
							 (SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
							 SELECT area_group.*
							 FROM app_sitemap_area_group area_group
							 WHERE area.id = area_group.area_id ) d ) AS groups ,
            
             				 (SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
							 SELECT area_node.*
							 FROM app_sitemap_area_node area_node
							 WHERE area.id = area_node.area_id 
							 ORDER BY area_node.weight ASC ) d ) AS nodes
                
					 FROM app_sitemap_area area
					 WHERE area.app_id = app.id 
					 ORDER BY area.weight ASC
					 ) d ) AS areas
			   FROM app 
			) X ;";
			#endregion

			NpgsqlCommand command = new NpgsqlCommand(sql);
			command.CommandType = CommandType.Text;

			List<JToken> result = new List<JToken>();
			DataTable resultTable = ExecuteSqlQueryCommand(command);
			foreach (DataRow dr in resultTable.Rows)
				result.Add(JToken.Parse((string)dr[0]));

			return result;
		}

		/// <summary>
		/// Gets application json by specified id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public JToken GetCompleteAppJson(Guid id)
		{
			#region <--- sql --->
			const string sql = @"SELECT row_to_json( X ) FROM (
				   SELECT app.id, app.name, app.label, app.description, app.icon_class, app.author, app.color, app.weight, app.access,
       	
       				-- pages
					(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
							SELECT page.*
							FROM app_page page
							WHERE page.app_id = app.id 
						) d ) AS pages,
			
					-- areas
					(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
					 SELECT area.*,
                
							 (SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
							 SELECT area_group.*
							 FROM app_sitemap_area_group area_group
							 WHERE area.id = area_group.area_id ) d ) AS groups ,
            
             				 (SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
							 SELECT area_node.*
							 FROM app_sitemap_area_node area_node
							 WHERE area.id = area_node.area_id
							 ORDER BY area_node.weight ASC ) d ) AS nodes
                
					 FROM app_sitemap_area area
					 WHERE area.app_id = app.id
					 ORDER BY area.weight ASC
					 ) d ) AS areas
			   FROM app 
			   WHERE app.id = @id 
			) X ;";
			#endregion

			NpgsqlCommand command = new NpgsqlCommand(sql);
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@id", id));

			DataTable resultTable = ExecuteSqlQueryCommand(command);
			if (resultTable.Rows.Count == 0)
				return null;

			return JToken.Parse((string)resultTable.Rows[0][0]);
		}

		/// <summary>
		/// Gets application json by specified name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public JToken GetCompleteAppJsonByName(string name)
		{
			#region <--- sql --->
			const string sql = @"SELECT row_to_json( X ) FROM (
				   SELECT app.id, app.name, app.label, app.description, app.icon_class, app.author, app.color, app.weight, app.access,
       	
       				-- pages
					(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
							SELECT page.*
							FROM app_page page
							WHERE page.app_id = app.id 
						) d ) AS pages,
			
					-- areas
					(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
					 SELECT area.*,
                
							 (SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
							 SELECT area_group.*
							 FROM app_sitemap_area_group area_group
							 WHERE area.id = area_group.area_id ) d ) AS groups ,
            
             				 (SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
							 SELECT area_node.*
							 FROM app_sitemap_area_node area_node
							 WHERE area.id = area_node.area_id ) d ) AS nodes
                
					 FROM app_sitemap_area area
					 WHERE area.app_id = app.id 
					 ) d ) AS areas
			   FROM app 
			   WHERE app.name = @name
			) X ;";
			#endregion

			NpgsqlCommand command = new NpgsqlCommand(sql);
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@name", name));

			DataTable resultTable = ExecuteSqlQueryCommand(command);
			if (resultTable.Rows.Count == 0)
				return null;

			return JToken.Parse((string)resultTable.Rows[0][0]);
		}

		/// <summary>
		/// Gets application id for specified name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Guid? GetAppIdByName(string name)
		{
			NpgsqlCommand command = new NpgsqlCommand(@"SELECT id FROM app  WHERE app.name = @name");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@name", name));

			DataTable resultTable = ExecuteSqlQueryCommand(command);
			if (resultTable.Rows.Count == 0)
				return null;

			return (Guid)resultTable.Rows[0][0];
		}

		/// <summary>
		/// Inserts application record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="description"></param>
		/// <param name="iconClass"></param>
		/// <param name="author"></param>
		/// <param name="color"></param>
		/// <param name="weight"></param>
		/// <param name="access"></param>
		/// <param name="transaction"></param>
		public void InsertApplication(Guid id, string name, string label, string description,
			string iconClass, string author, string color, int weight, List<Guid> access, NpgsqlTransaction transaction = null)
		{

			NpgsqlCommand command = new NpgsqlCommand(
				"INSERT INTO public.app (id,name,label,description,icon_class,author,color,weight,access) " +
				"VALUES(@id,@name,@label,@description,@icon_class,@author,@color,@weight,@access)");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@name", name));
			command.Parameters.Add(new NpgsqlParameter("@label", (object)label ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@description", (object)description ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@icon_class", (object)iconClass ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@author", (object)author ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@color", (object)color ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@weight", weight));

			if (access != null && access.Count > 0)
				command.Parameters.Add("@access", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = access.ToArray();
			else
				command.Parameters.Add("@access", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Updates application record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="description"></param>
		/// <param name="iconClass"></param>
		/// <param name="author"></param>
		/// <param name="color"></param>
		/// <param name="weight"></param>
		/// <param name="access"></param>
		/// <param name="transaction"></param>
		public void UpdateApplication(Guid id, string name, string label, string description,
			string iconClass, string author, string color, int weight, List<Guid> access, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand(
				"UPDATE public.app SET name = @name, label = @label, description = @description," +
				"icon_class = @icon_class, author = @author, color = @color, weight = @weight, access=@access " +
				"WHERE id = @id");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@name", name));
			command.Parameters.Add(new NpgsqlParameter("@label", (object)label ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@description", (object)description ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@icon_class", (object)iconClass ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@author", (object)author ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@color", (object)color ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@weight", weight));

			if (access != null && access.Count > 0)
				command.Parameters.Add("@access", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = access.ToArray();
			else
				command.Parameters.Add("@access", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Deletes application record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		public void DeleteApplication(Guid id, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.app WHERE id = @id");
			command.Parameters.Add(new NpgsqlParameter("@id", id));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}
	}
}
