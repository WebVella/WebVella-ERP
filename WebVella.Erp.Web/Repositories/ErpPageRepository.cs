using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Repositories
{
	internal class ErpPageRepository : BaseDbRepository
	{
		public ErpPageRepository(string conString) : base(conString) { }

		/// <summary>
		/// Gets list of all pages as json
		/// </summary>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public List<JToken> GetAllPages(NpgsqlTransaction transaction = null)
		{
			#region <--- sql --->
			const string sql = @"SELECT   row_to_json(d) FROM (	SELECT page.* FROM app_page page ) d;";
			#endregion

			NpgsqlCommand command = new NpgsqlCommand(sql);
			command.CommandType = CommandType.Text;

			DataTable resultTable;
			if (transaction != null)
				resultTable = ExecuteSqlQueryCommand(transaction, command);
			else
				resultTable = ExecuteSqlQueryCommand(command);

			List<JToken> result = new List<JToken>();
			foreach (DataRow dr in resultTable.Rows)
				result.Add(JToken.Parse((string)dr[0]));

			return result;
		}

		/// <summary>
		/// Gets record by id
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public JToken GetById(Guid id, NpgsqlTransaction transaction = null)
		{
			#region <--- sql --->
			const string sql = @"SELECT   row_to_json(d)  FROM ( SELECT page.* FROM app_page page where id = @id ) d;";
			#endregion

			NpgsqlCommand command = new NpgsqlCommand(sql);
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@id", id));

			DataTable resultTable;
			if (transaction != null)
				resultTable = ExecuteSqlQueryCommand(transaction, command);
			else
				resultTable = ExecuteSqlQueryCommand(command);

			if (resultTable.Rows.Count == 1)
				return JToken.Parse((string)resultTable.Rows[0][0]);

			return null;
		}

		/// <summary>
		/// Gets record by type
		/// </summary>
		/// <param name="type"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public DataTable GetByType(PageType type, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.app_page WHERE type = @type");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@type", (int)type));

			if (transaction != null)
				return ExecuteSqlQueryCommand(transaction, command);
			else
				return ExecuteSqlQueryCommand(command);
		}

		/// <summary>
		/// Gets application pages table
		/// </summary>
		/// <param name="appId"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public DataTable GetApplicationPages(Guid appId, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.app_page WHERE app_id = @app_id");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@app_id", appId));

			if (transaction != null)
				return ExecuteSqlQueryCommand(transaction, command);
			else
				return ExecuteSqlQueryCommand(command);
		}

		/// <summary>
		/// Inserts new page
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="iconClass"></param>
		/// <param name="system"></param>
		/// <param name="weight"></param>
		/// <param name="type"></param>
		/// <param name="appId"></param>
		/// <param name="entityId"></param>
		/// <param name="nodeId"></param>
		/// <param name="areaId"></param>
		/// <param name="isRazorBody"></param>
		/// <param name="razorBody"></param>
		/// <param name="layout"></param>
		/// <param name="transaction"></param>
		public void Insert(Guid id, string name, string label, string labelTranslations, string iconClass, bool system,	int weight, 
				int type, Guid? appId, Guid? entityId, Guid? nodeId, Guid? areaId, bool isRazorBody, string razorBody, string layout,
			NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand(
			  "INSERT INTO public.app_page (id,name,label,label_translations,system,type,icon_class,weight,app_id,entity_id,area_id,node_id,is_razor_body,razor_body,layout) " +
			  "VALUES(@id,@name,@label,@label_translations,@system,@type,@icon_class,@weight,@app_id,@entity_id,@area_id,@node_id,@is_razor_body,@razor_body,@layout)");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@name", name));
			command.Parameters.Add(new NpgsqlParameter("@label", (object)label ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@label_translations", (object)labelTranslations ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@icon_class", (object)iconClass ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@weight", weight));
			command.Parameters.Add(new NpgsqlParameter("@system", system));
			command.Parameters.Add(new NpgsqlParameter("@type", type));
			command.Parameters.Add(new NpgsqlParameter("@app_id", (object)appId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@entity_id", (object)entityId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@node_id", (object)nodeId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@area_id", (object)areaId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@is_razor_body", isRazorBody));
			command.Parameters.Add(new NpgsqlParameter("@razor_body", (object)razorBody ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@layout", (layout ?? string.Empty).Trim()));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Updates existing page
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="iconClass"></param>
		/// <param name="system"></param>
		/// <param name="weight"></param>
		/// <param name="type"></param>
		/// <param name="appId"></param>
		/// <param name="entityId"></param>
		/// <param name="nodeId"></param>
		/// <param name="areaId"></param>
		/// <param name="isRazorBody"></param>
		/// <param name="razorBody"></param>
		/// <param name="layout"></param>
		/// <param name="transaction"></param>
		public void Update(Guid id, string name, string label, string labelTranslations, string iconClass, bool system,	int weight,
				int type, Guid? appId, Guid? entityId, Guid? nodeId, Guid? areaId, bool isRazorBody, string razorBody, string layout,
			NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand(
				"UPDATE public.app_page SET " +
				"name = @name, label = @label, label_translations = @label_translations, system = @system," +
				"type = @type,icon_class = @icon_class, weight = @weight, app_id = @app_id, entity_id = @entity_id, " +
				"area_id = @area_id, node_id = @node_id, is_razor_body = @is_razor_body, razor_body = @razor_body, layout = @layout " +
				"WHERE id = @id");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@name", name));
			command.Parameters.Add(new NpgsqlParameter("@label", (object)label ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@label_translations", (object)labelTranslations ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@icon_class", (object)iconClass ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@weight", weight));
			command.Parameters.Add(new NpgsqlParameter("@system", system));
			command.Parameters.Add(new NpgsqlParameter("@type", type));
			command.Parameters.Add(new NpgsqlParameter("@app_id", (object)appId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@entity_id", (object)entityId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@node_id", (object)nodeId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@area_id", (object)areaId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@is_razor_body", isRazorBody));
			command.Parameters.Add(new NpgsqlParameter("@razor_body", (object)razorBody ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@layout", (layout??string.Empty).Trim()));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Deletes page record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		public void Delete(Guid id, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.app_page WHERE id = @id");
			command.Parameters.Add(new NpgsqlParameter("@id", id));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Unbind sitemap node from all pages
		/// This method is used before page delete
		/// </summary>
		/// <param name="sitemapNodeId"></param>
		/// <param name="transaction"></param>
		public void UnbindPagesFromSitemapNode(Guid sitemapNodeId, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("UPDATE app_page SET node_id = NULL where node_id = @node_id");
			command.Parameters.Add(new NpgsqlParameter("@node_id", sitemapNodeId));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}


		/// <summary>
		/// Unbind sitemap area from all pages
		/// This method is used before page delete
		/// </summary>
		/// <param name="areaId"></param>
		/// <param name="transaction"></param>
		public void UnbindPagesFromSitemapArea(Guid areaId, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("UPDATE app_page SET area_id = NULL where area_id = @area_id");
			command.Parameters.Add(new NpgsqlParameter("@area_id", areaId));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}
	}
}
