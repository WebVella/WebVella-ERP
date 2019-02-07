using Npgsql;
using System;
using System.Data;

namespace WebVella.Erp.Web.Repositories
{
	internal class PageBodyNodeRepository : BaseDbRepository
	{
		public PageBodyNodeRepository(string conString) : base(conString) { }

		/// <summary>
		/// Gets all nodes
		/// </summary>
		/// <param name="pageId"></param>
		/// <returns></returns>
		public DataTable GetAllBodyNodes()
		{
			NpgsqlCommand command = new NpgsqlCommand(@"SELECT * FROM app_page_body_node");
			command.CommandType = CommandType.Text;
			return ExecuteSqlQueryCommand(command);
		}


		/// <summary>
		/// Gets all page nodes
		/// </summary>
		/// <param name="pageId"></param>
		/// <returns></returns>
		public DataTable GetPageBodyNodes(Guid pageId)
		{
			NpgsqlCommand command = new NpgsqlCommand(@"SELECT * FROM app_page_body_node WHERE page_id = @page_id ORDER BY weight");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@page_id", pageId));

			return ExecuteSqlQueryCommand(command);
		}

		/// <summary>
		/// Gets root layout node by id
		/// </summary>
		/// <param name="appId"></param>
		/// <returns></returns>
		public DataRow GetById(Guid id, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.app_page_body_node WHERE id = @id");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@id", id));

			DataTable resultTable;

			if (transaction != null)
				resultTable = ExecuteSqlQueryCommand(transaction, command);
			else
				resultTable = ExecuteSqlQueryCommand(command);

			if (resultTable.Rows.Count == 1)
				return resultTable.Rows[0];

			return null;
		}

		/// <summary>
		/// Gets root page body nodes
		/// </summary>
		/// <param name="appId"></param>
		/// <returns></returns>
		public DataTable GetPageRootBodyNode(Guid pageId, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.app_page_body_node WHERE page_id = @page_id and parent_id is null");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@page_id", pageId));

			DataTable resultTable;

			if (transaction != null)
				resultTable = ExecuteSqlQueryCommand(transaction, command);
			else
				resultTable = ExecuteSqlQueryCommand(command);

			return resultTable;
		}

		/// <summary>
		/// Get body nodes for same parent
		/// </summary>
		/// <param name="parentId"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public DataTable GetPageBodyNodesByParentId(Guid? parentId, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.app_page_body_node WHERE parent_id = @parent_id");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@parent_id", parentId));

			if (transaction != null)
				return ExecuteSqlQueryCommand(transaction, command);
			else
				return ExecuteSqlQueryCommand(command);
		}

		/// <summary>
		/// Inserts new page body node record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="parentId"></param>
		/// <param name="pageId"></param>
		/// <param name="nodeId"></param>
		/// <param name="weight"></param>
		/// <param name="componentName"></param>
		/// <param name="containerId"></param>
		/// <param name="options"></param>
		/// <param name="transaction"></param>
		public void Insert(Guid id, Guid? parentId, Guid pageId, Guid? nodeId, int weight, string componentName,
			string containerId, string options, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand(
			  "INSERT INTO public.app_page_body_node (id,parent_id,page_id,node_id,weight,component_name,container_id, options) " +
			  "VALUES(@id,@parent_id,@page_id,@node_id,@weight,@component_name,@container_id,@options)");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@parent_id", (object)parentId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@page_id", pageId));
			command.Parameters.Add(new NpgsqlParameter("@node_id", (object)nodeId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@weight", weight));
			command.Parameters.Add(new NpgsqlParameter("@component_name", (object)componentName ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@container_id", (object)containerId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@options", (object)options ?? DBNull.Value));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Updates existing page body node record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="parentId"></param>
		/// <param name="pageId"></param>
		/// <param name="nodeId"></param>
		/// <param name="weight"></param>
		/// <param name="componentName"></param>
		/// <param name="containerId"></param>
		/// <param name="options"></param>
		/// <param name="transaction"></param>
		public void Update(Guid id, Guid? parentId, Guid pageId, Guid? nodeId, int weight, string componentName,
				string containerId, string options, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand(
			  "UPDATE public.app_page_body_node SET " +
				"parent_id = @parent_id, page_id = @page_id, node_id = @node_id, weight = @weight," +
				"component_name = @component_name, container_id = @container_id, options = @options  " +
			  "WHERE id = @id");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@parent_id", (object)parentId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@page_id", pageId));
			command.Parameters.Add(new NpgsqlParameter("@node_id", (object)nodeId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@weight", weight));
			command.Parameters.Add(new NpgsqlParameter("@component_name", (object)componentName ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@container_id", (object)containerId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@options", (object)options ?? DBNull.Value));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Updates existing page body node options
		/// </summary>
		/// <param name="id"></param>
		/// <param name="options"></param>
		/// <param name="transaction"></param>
		public void Update(Guid id, string options, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand(
			  "UPDATE public.app_page_body_node SET options = @options WHERE id = @id");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@options", (object)options ?? DBNull.Value));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Deletes page body node record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		public void Delete(Guid id, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.app_page_body_node WHERE id = @id");
			command.Parameters.Add(new NpgsqlParameter("@id", id));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}
	}
}
