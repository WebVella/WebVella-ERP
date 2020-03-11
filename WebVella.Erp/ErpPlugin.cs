using AutoMapper.Configuration;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using WebVella.Erp.Database;

namespace WebVella.Erp
{
	public abstract class ErpPlugin
	{
		[JsonProperty(PropertyName = "name")]
		public virtual string Name { get; protected set; }

		[JsonProperty(PropertyName = "prefix")]
		public virtual string Prefix { get; protected set; }

		[JsonProperty(PropertyName = "url")]
		public virtual string Url { get; protected set; }

		[JsonProperty(PropertyName = "description")]
		public virtual string Description { get; protected set; }

		[JsonProperty(PropertyName = "version")]
		public virtual int Version { get; protected set; }

		[JsonProperty(PropertyName = "company")]
		public virtual string Company { get; protected set; }

		[JsonProperty(PropertyName = "company_url")]
		public virtual string CompanyUrl { get; protected set; }

		[JsonProperty(PropertyName = "author")]
		public virtual string Author { get; protected set; }

		[JsonProperty(PropertyName = "repository")]
		public virtual string Repository { get; protected set; }

		[JsonProperty(PropertyName = "license")]
		public virtual string License { get; protected set; }

		[JsonProperty(PropertyName = "settings_url")]
		public virtual string SettingsUrl { get; protected set; }

		[JsonProperty(PropertyName = "plugin_page_url")]
		public virtual string PluginPageUrl { get; protected set; }

		[JsonProperty(PropertyName = "icon_url")]
		public virtual string IconUrl { get; protected set; }

		public virtual void SetAutoMapperConfiguration(MapperConfigurationExpression cfg)
		{
		}

		public virtual void Initialize(IServiceProvider ServiceProvider)
		{
		}

		[Obsolete("GetJobTypes is obsolete. No need to define list of job types, system finds it automatically. This method will be removed soon.")]
		public virtual IEnumerable<Type> GetJobTypes()
		{
			return new List<Type>();
		}

		public string GetPluginData()
		{
			if (string.IsNullOrWhiteSpace(Name))
				throw new Exception("Plugin name is not specified while trying to load plugin data");

			using (var connection = DbContext.Current.CreateConnection())
			{
				var cmd = connection.CreateCommand("SELECT * FROM plugin_data WHERE name = @name");
				cmd.Parameters.Add(new NpgsqlParameter("@name", Name));

				DataTable dt = new DataTable();
				new NpgsqlDataAdapter(cmd).Fill(dt);

				if (dt.Rows.Count == 0)
					return null;

				return (string)dt.Rows[0]["data"];
			}
		}

		public void SavePluginData(string data)
		{
			if (string.IsNullOrWhiteSpace(Name))
				throw new Exception("Plugin name is not specified while trying to load plugin data");

			bool pluginDataExists = GetPluginData() != null;

			if (!pluginDataExists)
			{
				using (var connection = DbContext.Current.CreateConnection())
				{
					var cmd = connection.CreateCommand("INSERT INTO plugin_data (id,name,data) VALUES( @id,@name,@data )");
					cmd.Parameters.Add(new NpgsqlParameter("@id", Guid.NewGuid()));
					cmd.Parameters.Add(new NpgsqlParameter("@name", Name));
					cmd.Parameters.Add(new NpgsqlParameter("@data", data));
					cmd.ExecuteNonQuery();
				}
			}
			else
			{
				using (var connection = DbContext.Current.CreateConnection())
				{
					var cmd = connection.CreateCommand("UPDATE plugin_data SET data = @data WHERE name = @name");
					cmd.Parameters.Add(new NpgsqlParameter("@name", Name));
					cmd.Parameters.Add(new NpgsqlParameter("@data", data));
					cmd.ExecuteNonQuery();
				}
			}
		}
	}
}
