using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;

namespace WebVella.Erp.Web.Repositories
{
	internal class SitemapAreaRepository : BaseDbRepository
	{
		public SitemapAreaRepository(string conString) : base(conString) { }

		/// <summary>
		/// Gets all appId by areaId
		/// </summary>
		/// <param name="appId"></param>
		/// <returns></returns>
		public Guid? GetAppIdByAreaId(Guid areaId, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT app_id FROM public.app_sitemap_area WHERE id = @id");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@id", areaId));

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
		/// Gets all application areas table
		/// </summary>
		/// <param name="appId"></param>
		/// <returns></returns>
		public DataTable GetApplicationAreas(Guid appId, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.app_sitemap_area WHERE app_id = @app_id");
			command.CommandType = CommandType.Text;
			command.Parameters.Add(new NpgsqlParameter("@app_id", appId));

			if (transaction != null)
				return ExecuteSqlQueryCommand(transaction, command);
			else
				return ExecuteSqlQueryCommand(command);
		}

		/// <summary>
		/// Inserts new sitemap area record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="appId"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="description"></param>
		/// <param name="descriptionTranslations"></param>
		/// <param name="iconClass"></param>
		/// <param name="color"></param>
		/// <param name="weight"></param>
		/// <param name="showGroupNames"></param>
		/// <param name="accessRoles"></param>
		/// <param name="transaction"></param>
		public void Insert(Guid id, Guid appId, string name, string label, string labelTranslations,
			string description, string descriptionTranslations, string iconClass, string color,
			int weight, bool showGroupNames, List<Guid> accessRoles, NpgsqlTransaction transaction = null)
		{

			NpgsqlCommand command = new NpgsqlCommand(
					"INSERT INTO public.app_sitemap_area (id,app_id,name,label,label_translations,description,description_translations," +
					"icon_class,color,weight,show_group_names,access_roles )" +
					"VALUES(@id,@app_id,@name,@label,@label_translations,@description,@description_translations," +
					"@icon_class,@color,@weight,@show_group_names,@access_roles )");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@app_id", appId));
			command.Parameters.Add(new NpgsqlParameter("@name", name));
			command.Parameters.Add(new NpgsqlParameter("@label", (object)label ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@label_translations", (object)labelTranslations ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@description", (object)description ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@description_translations", (object)descriptionTranslations ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@icon_class", (object)iconClass ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@color", (object)color ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@weight", weight));
			command.Parameters.Add(new NpgsqlParameter("@show_group_names", showGroupNames));

			if (accessRoles != null && accessRoles.Count > 0)
				command.Parameters.Add("@access_roles", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = accessRoles.ToArray();
			else
				command.Parameters.Add("@access_roles", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Updates existing sitemap area record
		/// </summary>
		/// <param name="id"></param>
		/// <param name="appId"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="description"></param>
		/// <param name="descriptionTranslations"></param>
		/// <param name="iconClass"></param>
		/// <param name="color"></param>
		/// <param name="weight"></param>
		/// <param name="showGroupNames"></param>
		/// <param name="accessRoles"></param>
		/// <param name="transaction"></param>
		public void Update(Guid id, Guid appId, string name, string label, string labelTranslations,
			string description, string descriptionTranslations, string iconClass, string color,
			int weight, bool showGroupNames, List<Guid> accessRoles, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand(
					"UPDATE public.app_sitemap_area " +
					"SET name = @name, label = @label, label_translations = @label_translations, " +
					"description = @description,description_translations = @description_translations," +
					"icon_class = @icon_class, color = @color, weight = @weight, show_group_names = @show_group_names, " +
					"access_roles = @access_roles " +
					" WHERE id = @id");

			command.Parameters.Add(new NpgsqlParameter("@id", id));
			command.Parameters.Add(new NpgsqlParameter("@app_id", appId));
			command.Parameters.Add(new NpgsqlParameter("@name", name));
			command.Parameters.Add(new NpgsqlParameter("@label", (object)label ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@label_translations", (object)labelTranslations ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@description", (object)description ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@description_translations", (object)descriptionTranslations ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@icon_class", (object)iconClass ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@color", (object)color ?? DBNull.Value));
			command.Parameters.Add(new NpgsqlParameter("@weight", weight));
			command.Parameters.Add(new NpgsqlParameter("@show_group_names", showGroupNames));


			if (accessRoles != null && accessRoles.Count > 0)
				command.Parameters.Add("@access_roles", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = accessRoles.ToArray();
			else
				command.Parameters.Add("@access_roles", NpgsqlDbType.Array | NpgsqlDbType.Uuid).Value = new List<Guid>().ToArray();

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}

		/// <summary>
		/// Deletes sitemap area
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		public void Delete(Guid id, NpgsqlTransaction transaction = null)
		{
			NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.app_sitemap_area WHERE id = @id");
			command.Parameters.Add(new NpgsqlParameter("@id", id));

			if (transaction != null)
				ExecuteSqlNonQueryCommands(transaction, command);
			else
				ExecuteSqlNonQueryCommands(command);
		}
	}
}
