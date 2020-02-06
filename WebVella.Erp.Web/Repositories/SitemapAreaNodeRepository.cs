using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;

namespace WebVella.Erp.Web.Repositories
{
	internal class SitemapAreaNodeRepository : BaseDbRepository
	{
		
		public SitemapAreaNodeRepository(string conString) : base(conString) { }

		/// <summary>
		/// Returns area node record
		/// </summary>
		/// <param name="nodeId"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public DataRow Get(Guid nodeId, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.app_sitemap_area_node WHERE id = @id");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@id", nodeId));

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
		/// Gets all areaId by groupId
		/// </summary>
		/// <param name="nideId"></param>
		/// <returns></returns>
		public Guid? GetAreaIdByNodeId(Guid nodeId, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT area_id FROM public.app_sitemap_area_node WHERE id = @id");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@id", nodeId));

			DataTable resultTable;
			if (transaction != null)
				resultTable = ExecuteSqlQueryCommand(transaction, command);
			else
				resultTable = ExecuteSqlQueryCommand(command);

			if (resultTable.Rows.Count == 0)
				return null;

			return (Guid)resultTable.Rows[0][0];
		}

		/// <summary>
		/// Gets all sitemap area nodes table for specified area
		/// </summary>
		/// <param name="appId"></param>
		/// <returns></returns>
		public DataTable GetAreaNodes(Guid areaId, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.app_sitemap_area_node WHERE area_id = @area_id");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@area_id", areaId));

			if (transaction != null)
				return ExecuteSqlQueryCommand(transaction, command);
			else
				return ExecuteSqlQueryCommand(command);

		}

		/// <summary>
		/// Inserts new sitemap area node
		/// </summary>
		/// <param name="id"></param>
		/// <param name="areaId"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="iconClass"></param>
		/// <param name="url"></param>
		/// <param name="type"></param>
		/// <param name="entityId"></param>
		/// <param name="weight"></param>
		/// <param name="accessRoles"></param>
		/// <param name="transaction"></param>
		public void Insert(Guid id, Guid areaId, string name, string label, string labelTranslations,
			string iconClass, string url, int type, Guid? entityId, int weight,
			List<Guid> accessRoles, List<Guid> entityListPages = null, List<Guid> entityCreatePages = null,
            List<Guid> entityDetailsPages = null, List<Guid> entityManagePages = null, NpgsqlTransaction transaction = null, Guid? parentId = null)
		{
			NpgsqlCommand command = new NpgsqlCommand(
                    "INSERT INTO public.app_sitemap_area_node (id,parent_id,area_id,name,label,label_translations,weight,type,icon_class,url,entity_id,access_roles,entity_list_pages,entity_create_pages,entity_details_pages,entity_manage_pages)" +
                    "VALUES(@id,@parentId,@area_id,@name,@label,@label_translations,@weight,@type,@icon_class,@url,@entity_id,@access_roles,@entity_list_pages,@entity_create_pages,@entity_details_pages,@entity_manage_pages)");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@parentId", (object)parentId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@area_id", areaId));
			command.Parameters.Add(new NpgsqlParameter("@name", name));
			command.Parameters.Add(new NpgsqlParameter("@label", (object)label ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@label_translations", (object)labelTranslations ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@icon_class", (object)iconClass ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@url", (object)url ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@entity_id", (object)entityId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@weight", weight));
			command.Parameters.Add(new NpgsqlParameter("@type", type));

			if (accessRoles != null && accessRoles.Count > 0)
				command.Parameters.Add("@access_roles", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = accessRoles.ToArray();
			else
				command.Parameters.Add("@access_roles", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();

            if (entityListPages != null && entityListPages.Count > 0)
                command.Parameters.Add("@entity_list_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = entityListPages.ToArray();
            else
                command.Parameters.Add("@entity_list_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();

            if (entityCreatePages != null && entityCreatePages.Count > 0)
                command.Parameters.Add("@entity_create_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = entityCreatePages.ToArray();
            else
                command.Parameters.Add("@entity_create_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();

            if (entityDetailsPages != null && entityDetailsPages.Count > 0)
                command.Parameters.Add("@entity_details_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = entityDetailsPages.ToArray();
            else
                command.Parameters.Add("@entity_details_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();

            if (entityManagePages != null && entityManagePages.Count > 0)
                command.Parameters.Add("@entity_manage_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = entityManagePages.ToArray();
            else
                command.Parameters.Add("@entity_manage_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();


            if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Updates sitemap area node record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="areaId"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="iconClass"></param>
		/// <param name="url"></param>
		/// <param name="type"></param>
		/// <param name="entityId"></param>
		/// <param name="weight"></param>
		/// <param name="accessRoles"></param>
		/// <param name="transaction"></param>
		public void Update(Guid id, Guid areaId, string name, string label, string labelTranslations,
			string iconClass, string url, int type, Guid? entityId, int weight,
			List<Guid> accessRoles, List<Guid> entityListPages = null, List<Guid> entityCreatePages = null,
            List<Guid> entityDetailsPages = null, List<Guid> entityManagePages = null, NpgsqlTransaction transaction = null, Guid? parentId = null)
		{
			NpgsqlCommand command = new NpgsqlCommand(
					"UPDATE public.app_sitemap_area_node SET " +
					"parent_id = @parent_id, area_id = @area_id, name = @name, label = @label, label_translations = @label_translations, " +
					"weight = @weight, type = @type, icon_class = @icon_class, url = @url, entity_id = @entity_id, " +
                    "access_roles = @access_roles, entity_list_pages = @entity_list_pages, entity_create_pages = @entity_create_pages, " +
                    "entity_details_pages = @entity_details_pages, entity_manage_pages = @entity_manage_pages " +
					"WHERE id = @id");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@parent_id", (object)parentId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@area_id", areaId));
			command.Parameters.Add(new NpgsqlParameter("@name", name));
			command.Parameters.Add(new NpgsqlParameter("@label", (object)label ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@label_translations", (object)labelTranslations ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@icon_class", (object)iconClass ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@url", (object)url ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@entity_id", (object)entityId ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@weight", weight));
			command.Parameters.Add(new NpgsqlParameter("@type", type));

			if (accessRoles != null && accessRoles.Count > 0)
				command.Parameters.Add("@access_roles", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = accessRoles.ToArray();
			else
				command.Parameters.Add("@access_roles", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();

            if (entityListPages != null && entityListPages.Count > 0)
                command.Parameters.Add("@entity_list_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = entityListPages.ToArray();
            else
                command.Parameters.Add("@entity_list_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();

            if (entityCreatePages != null && entityCreatePages.Count > 0)
                command.Parameters.Add("@entity_create_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = entityCreatePages.ToArray();
            else
                command.Parameters.Add("@entity_create_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();

            if (entityDetailsPages != null && entityDetailsPages.Count > 0)
                command.Parameters.Add("@entity_details_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = entityDetailsPages.ToArray();
            else
                command.Parameters.Add("@entity_details_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();

            if (entityManagePages != null && entityManagePages.Count > 0)
                command.Parameters.Add("@entity_manage_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = entityManagePages.ToArray();
            else
                command.Parameters.Add("@entity_manage_pages", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();


            if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Deletes sitemap area node record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		public void Delete(Guid id, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.app_sitemap_area_node WHERE id = @id");
			command.Parameters.Add(new NpgsqlParameter("@id", id));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}
	}
}
