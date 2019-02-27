using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Database;
using WebVella.Erp.Fts;

namespace WebVella.Erp.Api
{
	public class SearchManager
	{
		private FtsAnalyzer ftsAnalyzer = new FtsAnalyzer();

		public SearchResultList Search(SearchQuery query)
		{
			if (query == null)
				throw new ArgumentNullException(nameof(query));

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			
			string sql = @"SELECT id,url,snippet,timestamp, COUNT(*) OVER() AS ___total_count___ FROM system_search ";
			if( query.ResultType == SearchResultType.Full )
				sql = @"SELECT *,  COUNT(*) OVER() AS ___total_count___ FROM system_search ";

			string textQuerySql = string.Empty;
			if (!string.IsNullOrWhiteSpace(query.Text))
			{
				if (query.SearchType == SearchType.Contains)
				{
					var words = query.Text.ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
					foreach (var word in words)
					{
						string parameterName = "@par_" + Guid.NewGuid().ToString().Replace("-", "");
						NpgsqlParameter parameter = new NpgsqlParameter(parameterName, $"%{word}%");
						parameters.Add(parameter);
						textQuerySql = textQuerySql + $"OR content ILIKE {parameterName} ";
					}
					if (textQuerySql.StartsWith("OR"))
					{
						textQuerySql = textQuerySql.Substring(2); //remove initial OR
						textQuerySql = $"({textQuerySql})"; //add brackets
					}
				}
				else if (query.SearchType == SearchType.Fts)
				{
					string parameterName = "@par_" + Guid.NewGuid().ToString().Replace("-", "");
					string analizedText = ftsAnalyzer.ProcessText(query.Text.ToLowerInvariant());
					bool singleWord = analizedText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Count() == 1;
					if (singleWord)
					{
						//search for all lexemes starting with this word 
						parameters.Add(new NpgsqlParameter(parameterName, analizedText + ":*" ));
						textQuerySql = textQuerySql + " to_tsvector( 'simple', stem_content ) @@ to_tsquery( 'simple', " + parameterName + ") ";
					}
					else
					{
						parameters.Add(new NpgsqlParameter(parameterName, analizedText));
						textQuerySql = textQuerySql + " to_tsvector( 'simple', stem_content) @@ plainto_tsquery( 'simple', " + parameterName + ") ";
					}
				}
			}

			string entityQuerySql = string.Empty;
			if (query.Entities.Any())
			{
				foreach (var id in query.Entities)
				{
					string parameterName = "@par_" + Guid.NewGuid().ToString().Replace("-", "");
					NpgsqlParameter parameter = new NpgsqlParameter(parameterName, $"%{id}%");
					parameters.Add(parameter);
					entityQuerySql = entityQuerySql + $"OR entities ILIKE {parameterName} ";
				}
				if (entityQuerySql.StartsWith("OR"))
				{
					entityQuerySql = entityQuerySql.Substring(2); //remove initial OR
					entityQuerySql = $"({entityQuerySql})"; //add brackets
				}
			}

			string appsQuerySql = string.Empty;
			if (query.Apps.Any())
			{
				foreach (var id in query.Apps)
				{
					string parameterName = "@par_" + Guid.NewGuid().ToString().Replace("-", "");
					NpgsqlParameter parameter = new NpgsqlParameter(parameterName, $"%{id}%");
					parameters.Add(parameter);
					appsQuerySql = appsQuerySql + $"OR entities ILIKE {parameterName} ";
				}
				if (appsQuerySql.StartsWith("OR"))
				{
					appsQuerySql = entityQuerySql.Substring(2); //remove initial OR
					appsQuerySql = $"({appsQuerySql})"; //add brackets
				}
			}

			string recordsQuerySql = string.Empty;
			if (query.Records.Any())
			{
				foreach (var id in query.Records)
				{
					string parameterName = "@par_" + Guid.NewGuid().ToString().Replace("-", "");
					NpgsqlParameter parameter = new NpgsqlParameter(parameterName, $"%{id}%");
					parameters.Add(parameter);
					recordsQuerySql = recordsQuerySql + $"OR entities ILIKE {parameterName} ";
				}
				if (recordsQuerySql.StartsWith("OR"))
				{
					recordsQuerySql = entityQuerySql.Substring(2); //remove initial OR
					recordsQuerySql = $"({recordsQuerySql})"; //add brackets
				}
			}

			string whereSql = string.Empty;
			if (!string.IsNullOrWhiteSpace(textQuerySql))
			{
				whereSql = $"WHERE {textQuerySql} ";
			}

			if (!string.IsNullOrWhiteSpace(entityQuerySql))
			{
				if( whereSql == string.Empty)
					whereSql = $"WHERE {entityQuerySql} ";
				else
					whereSql = $"AND {entityQuerySql} ";
			}
			if (!string.IsNullOrWhiteSpace(appsQuerySql))
			{
				if (whereSql == string.Empty)
					whereSql = $"WHERE {appsQuerySql} ";
				else
					whereSql = $"AND {appsQuerySql} ";
			}
			if (!string.IsNullOrWhiteSpace(recordsQuerySql))
			{
				if (whereSql == string.Empty)
					whereSql = $"WHERE {recordsQuerySql} ";
				else
					whereSql = $"AND {recordsQuerySql} ";
			}

			sql = sql + whereSql;

			if( query.SearchType != SearchType.Fts )
				sql = sql + " ORDER BY timestamp DESC ";

			string pagingSql = string.Empty;
			if (query.Limit != null || query.Skip != null)
			{
				pagingSql = "LIMIT ";
				if (query.Limit.HasValue && query.Limit != 0)
					pagingSql = pagingSql + query.Limit + " ";
				else
					pagingSql = pagingSql + "ALL ";

				if (query.Skip.HasValue)
					pagingSql = pagingSql + " OFFSET " + query.Skip;

				sql = sql + pagingSql;
			}

			DataTable dt = new DataTable();
			using (var connection = DbContext.Current.CreateConnection())
			{
				var command = connection.CreateCommand(sql, parameters: parameters);
				command.CommandTimeout = 60;
				new NpgsqlDataAdapter(command).Fill(dt);

				SearchResultList resultList = new SearchResultList();
				foreach (DataRow dr in dt.Rows)
				{
					resultList.Add(dr.MapTo<SearchResult>());
					if (resultList.TotalCount == 0)
						resultList.TotalCount = (int)((long)dr["___total_count___"]);
				}

				return resultList;
			}
		}

		public SearchResult AddToIndex(string url, string snippet, string content, List<Guid> entities = null,
				List<Guid> apps = null, List<Guid> records = null, string auxData = null, DateTime? timestamp = null)
		{
			SearchResult record = new SearchResult();
			record.Id = new Guid();
			record.Url = url ?? string.Empty;
			record.Snippet = snippet ?? string.Empty;
			record.Content = (content ?? string.Empty).ToLowerInvariant(); ;
			
			record.StemContent = ftsAnalyzer.ProcessText((content ?? string.Empty).ToLowerInvariant());

			record.AuxData = auxData ?? string.Empty;
			if (entities != null)
				record.Entities.AddRange(entities);
			if (apps != null)
				record.Entities.AddRange(apps);
			if (records != null)
				record.Entities.AddRange(records);

			record.Timestamp = DateTime.UtcNow;
			if (timestamp.HasValue)
				record.Timestamp = timestamp.Value;

			using (var connection = DbContext.Current.CreateConnection())
			{
				var command = connection.CreateCommand(@"INSERT INTO system_search (id,entities,apps,records,content,stem_content,snippet,url,aux_data,""timestamp"")
						VALUES( @id,@entities,@apps,@records,@content,@stem_content,@snippet,@url,@aux_data,@timestamp) ");

				command.Parameters.Add(new NpgsqlParameter("@id", Guid.NewGuid()));
				command.Parameters.Add(new NpgsqlParameter("@entities", JsonConvert.SerializeObject(record.Entities ?? new List<Guid>())));
				command.Parameters.Add(new NpgsqlParameter("@apps", JsonConvert.SerializeObject(record.Apps ?? new List<Guid>())));
				command.Parameters.Add(new NpgsqlParameter("@records", JsonConvert.SerializeObject(record.Records ?? new List<Guid>())));
				command.Parameters.Add(new NpgsqlParameter("@content", record.Content ?? string.Empty));
				command.Parameters.Add(new NpgsqlParameter("@stem_content", record.StemContent ?? string.Empty));
				command.Parameters.Add(new NpgsqlParameter("@snippet", record.Snippet ?? string.Empty));
				command.Parameters.Add(new NpgsqlParameter("@url", record.Url ?? string.Empty));
				command.Parameters.Add(new NpgsqlParameter("@aux_data", record.AuxData ?? string.Empty));
				command.Parameters.Add(new NpgsqlParameter("@timestamp", record.Timestamp));
				command.ExecuteNonQuery();

			}

			return record;
		}

		public void RemoveFromIndex(Guid id)
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				var command = connection.CreateCommand(@"DELETE FROM system_search WHERE id = @id");
				command.Parameters.Add(new NpgsqlParameter("@id", Guid.NewGuid()));
				command.ExecuteNonQuery();

			}
		}

	}
}
