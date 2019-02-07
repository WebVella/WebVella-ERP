using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;

namespace WebVella.Erp.Web.Repositories
{
	internal class SitemapAreaGroupRepository : BaseDbRepository
	{
		public SitemapAreaGroupRepository(string conString) : base(conString) { }

		/// <summary>
		/// Gets all areaId by groupId
		/// </summary>
		/// <param name="groupId"></param>
		/// <returns></returns>
		public Guid? GetAreaIdByGroupId(Guid groupId, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT area_id FROM public.app_sitemap_area_group WHERE id = @id");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@id", groupId));

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
		/// Gets all sitemap area groups table for specified area
		/// </summary>
		/// <param name="appId"></param>
		/// <returns></returns>
		public DataTable GetAreaGroups(Guid areaId, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.app_sitemap_area_group WHERE area_id = @area_id");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@area_id", areaId));

			if (transaction != null)
				return ExecuteSqlQueryCommand(transaction, command);
			else
				return ExecuteSqlQueryCommand(command);

		}


		/// <summary>
		/// Inserts sitemap area group record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="areaId"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="weight"></param>
		/// <param name="renderRoles"></param>
		/// <param name="transaction"></param>
		public void Insert(Guid id, Guid areaId, string name, string label, string labelTranslations,
			int weight, List<Guid> renderRoles, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand(
					"INSERT INTO public.app_sitemap_area_group (id,area_id,name,label,label_translations,weight,render_roles )" +
					"VALUES(@id,@area_id,@name,@label,@label_translations,@weight,@render_roles )");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@area_id", areaId));
			command.Parameters.Add(new NpgsqlParameter("@name", name));
			command.Parameters.Add(new NpgsqlParameter("@label", (object)label ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@label_translations", (object)labelTranslations ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@weight", weight));

			if (renderRoles != null && renderRoles.Count > 0)
				command.Parameters.Add("@render_roles", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = renderRoles.ToArray();
			else
				command.Parameters.Add("@render_roles", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Updates sitemap area group record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="areaId"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="weight"></param>
		/// <param name="renderRoles"></param>
		/// <param name="transaction"></param>
		public void Update(Guid id, Guid areaId, string name, string label, string labelTranslations,
			int weight, List<Guid> renderRoles, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand(
					"UPDATE public.app_sitemap_area_group " +
					"SET name = @name, area_id = @area_id, label = @label, label_translations = @label_translations, " +
					"weight = @weight, render_roles = @render_roles" +
					"WHERE id = @id");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@area_id", areaId));
			command.Parameters.Add(new NpgsqlParameter("@name", name));
			command.Parameters.Add(new NpgsqlParameter("@label", (object)label ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@label_translations", (object)labelTranslations ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@weight", weight));

			if (renderRoles != null && renderRoles.Count > 0)
				command.Parameters.Add("@render_roles", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = renderRoles.ToArray();
			else
				command.Parameters.Add("@render_roles", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Deletes sitemap area group record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		public void Delete(Guid id, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.app_sitemap_area_group WHERE id = @id");
			command.Parameters.Add(new NpgsqlParameter("@id", id));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}
	}
}
