using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Api.Models.AutoMapper;
using WebVella.ERP.Database.Models;
using WebVella.ERP.Utilities;

namespace WebVella.ERP.Database
{
	public class DbRecordRepository
	{
		#region <--- Constants --->
		
		private const string WILDCARD_SYMBOL = "*";
		private const char FIELDS_SEPARATOR = ',';
		private const char RELATION_SEPARATOR = '.';
		private const char RELATION_NAME_RESULT_SEPARATOR = '$';

		internal const string RECORD_COLLECTION_PREFIX = "rec_";
		const string BEGIN_OUTER_SELECT = @"SELECT row_to_json( X )FROM (";
		const string BEGIN_SELECT = @"SELECT ";
		const string REGULAR_FIELD_SELECT = @" {1}.""{0}"" AS ""{0}"",";
		//const string JOIN_FIELD_SELECT = @"	'{0}', array_remove(array_agg(distinct(json_build_object({1})::jsonb)), NULL),";
		const string END_SELECT = @"";
		const string END_OUTER_SELECT = @") X";
		const string FROM = @"FROM {0}";
		//const string JOIN = @"LEFT JOIN  {0} {1} ON {2}.{3} = {4}.{5}";
		//const string GROUPBY = @"GROUP BY {0}";

		const string OTM_RELATION_TEMPLATE = @"	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
					SELECT {1} 
					FROM {2} {3}
					WHERE {3}.{4} = {5}.{6} ) d ) AS ""{0}"",";

		const string MTM_RELATION_TEMPLATE = @"( SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
					SELECT {1}
					FROM {2} {3}
					LEFT JOIN  {4} {5} ON {6}.{7} = {8}.{9}
					WHERE {10}.{11} = {12}.{13} )d  ) AS ""{0}"","; 

		#endregion

		public void Create(string entityName, IEnumerable<KeyValuePair<string, object>> recordData)
		{
			DbEntity entity = DbContext.Current.EntityRepository.Read(entityName);

			List<DbParameter> parameters = new List<DbParameter>();

			foreach (var record in recordData)
			{
				DbBaseField field = entity.Fields.FirstOrDefault(f => f.Name.ToLowerInvariant() == record.Key.ToLowerInvariant());

				DbParameter param = new DbParameter();
				param.Name = field.Name;
				param.Value = record.Value ?? DBNull.Value;
				param.Type = DbTypeConverter.ConvertToDatabaseType(field.GetFieldType());
				parameters.Add(param);
			}

			string tableName = RECORD_COLLECTION_PREFIX + entityName;
			DbRepository.InsertRecord(tableName, parameters);
		}

		public EntityRecord Update(string entityName, IEnumerable<KeyValuePair<string, object>> recordData)
		{
			DbEntity entity = DbContext.Current.EntityRepository.Read(entityName);

			List<DbParameter> parameters = new List<DbParameter>();
			Guid? id = null;

			foreach (var record in recordData)
			{
				DbBaseField field = entity.Fields.FirstOrDefault(f => f.Name.ToLowerInvariant() == record.Key.ToLowerInvariant());

				if (field.Name == "id")
					id = (Guid)record.Value;

				DbParameter param = new DbParameter();
				param.Name = field.Name;
				param.Value = record.Value ?? DBNull.Value;
				param.Type = DbTypeConverter.ConvertToDatabaseType(field.GetFieldType());
				parameters.Add(param);
			}

			if (!id.HasValue)
				throw new StorageException("ID is missing. Cannot update records without ID specified.");

			string tableName = RECORD_COLLECTION_PREFIX + entityName;

			var updateSuccess = DbRepository.UpdateRecord(tableName, parameters);
			if (!updateSuccess)
				throw new StorageException("Failed to update record.");

			return Find(entityName, id.Value);
		}

		public EntityRecord Delete(string entityName, Guid id)
		{
			string tableName = RECORD_COLLECTION_PREFIX + entityName;

			EntityRecord outRecord = Find(entityName, id);
			if (outRecord == null)
				throw new StorageException("There is no record with such id to update.");

			DbRepository.DeleteRecord(tableName, id);

			return outRecord;
		}

		public EntityRecord Find(string entityName, Guid id)
		{
			string tableName = RECORD_COLLECTION_PREFIX + entityName;

			EntityRecord record = new EntityRecord();

			using (DbConnection con = DbContext.Current.CreateConnection())
			{

				NpgsqlCommand command = con.CreateCommand($"SELECT * FROM {tableName} WHERE id=@id;");

				var parameter = command.CreateParameter() as NpgsqlParameter;
				parameter.ParameterName = "id";
				parameter.Value = id;
				parameter.NpgsqlDbType = NpgsqlDbType.Uuid;
				command.Parameters.Add(parameter);

				using (var reader = command.ExecuteReader())
				{

					int fieldcount = reader.FieldCount;

					if (reader.Read())
					{

						for (int index = 0; index < fieldcount; index++)
							record[reader.GetName(index)] = reader[index] == DBNull.Value ? null: reader[index];

					}
					else {
						return null;
					}

					reader.Close();

				}
				return record;
			}
		}

		/*
		public IEnumerable<IEnumerable<KeyValuePair<string, object>>> Find(string entityName, QueryObject query, QuerySortObject[] sort, int? skip, int? limit)
		{
			string tableName = RECORD_COLLECTION_PREFIX + entityName;

			List<List<KeyValuePair<string, object>>> records = new List<List<KeyValuePair<string, object>>>();

			using (DbConnection con = DbContext.Current.CreateConnection())
			{
				NpgsqlCommand command = con.CreateCommand($"SELECT * FROM {tableName};");

				//var parameter = command.CreateParameter() as NpgsqlParameter;
				//parameter.ParameterName = "id";
				//parameter.Value = id;
				//parameter.NpgsqlDbType = NpgsqlDbType.Uuid;
				//command.Parameters.Add(parameter);

				var reader = command.ExecuteReader();

				int fieldcount = reader.FieldCount;

				while (reader.Read())
				{
					List<KeyValuePair<string, object>> record = new List<KeyValuePair<string, object>>();
					for (int index = 0; index < fieldcount; index++)
					{
						KeyValuePair<string, object> column = new KeyValuePair<string, object>(reader.GetName(index), reader[index]);
						record.Add(column);
					}

					records.Add(record);
				}

				return records;
			}
		}
		*/

		public long Count(string entityName, QueryObject query)
		{
			string tableName = RECORD_COLLECTION_PREFIX + entityName;
			using (DbConnection con = DbContext.Current.CreateConnection())
			{
				NpgsqlCommand command = con.CreateCommand($"SELECT COUNT(*) FROM {tableName};");
				return (long)command.ExecuteScalar();
			}
		}

		public void CreateRecordField(string entityName, string fieldName, object value)
		{
			string tableName = RECORD_COLLECTION_PREFIX + entityName;

			DbEntity dbEntity = DbContext.Current.EntityRepository.Read(entityName);
			Entity entity = dbEntity.MapTo<Entity>();
			Field field = entity.Fields.FirstOrDefault(f => f.Name.ToLowerInvariant() == fieldName.ToLowerInvariant());
			DbRepository.CreateColumn(tableName, field.Name, field.GetFieldType(), false, value, !field.Required);
		}

		public void RemoveRecordField(string entityName, string fieldName)
		{
			string tableName = RECORD_COLLECTION_PREFIX + entityName;

			DbRepository.DeleteColumn(tableName, fieldName);
		}

		private EntityRecord ConvertJObjectToEntityRecord(JObject jObj, List<Field> fields)
		{
			EntityRecord record = new EntityRecord();
			foreach (Field field in fields)
			{
				if (!(field is RelationFieldMeta))
				{
					record[field.Name] = ExtractFieldValue(jObj[field.Name], field);
				}
				else
				{
					List<EntityRecord> relRecords = new List<EntityRecord>();
					var relFields = ((RelationFieldMeta)field).Fields;
					JArray relatedJsonRecords = jObj[field.Name].Value<JArray>();
					foreach (JObject relatedObj in relatedJsonRecords)
						relRecords.Add(ConvertJObjectToEntityRecord(relatedObj, relFields));

					record[field.Name] = relRecords;
				}
			}
			return record;
		}

		private object ExtractFieldValue(object value, Field field, bool encryptPasswordFields = false)
		{
			if (value == null)
				return field.GetDefaultValue();

			if (value is JToken)
				value = ((JToken)value).ToObject<object>();

			if (field is AutoNumberField)
			{
				if (value == null)
					return null;
				if (value is string)
					return decimal.Parse(value as string);

				return Convert.ToDecimal(value);
			}
			else if (field is CheckboxField)
				return value as bool?;
			else if (field is CurrencyField)
			{
				if (value == null)
					return null;
				if (value is string)
				{
					if (string.IsNullOrWhiteSpace(value as string))
						return null;
					if ((value as string).StartsWith("$"))
						value = (value as string).Substring(1);
					return decimal.Parse(value as string);
				}

				return Convert.ToDecimal(value);
			}
			else if (field is DateField)
			{
				if (value == null)
					return null;

				DateTime? date = null;
				if (value is string)
					date = DateTime.Parse(value as string);
				else
					date = value as DateTime?;

				if (date != null)
					return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0, DateTimeKind.Utc);
			}
			else if (field is DateTimeField)
			{

				if (value == null)
					return null;

				if (value is string)
					return DateTime.Parse(value as string);

				return value as DateTime?;
			}
			else if (field is EmailField)
				return value as string;
			else if (field is FileField)
				//TODO convert file path to url path
				return value as string;
			else if (field is ImageField)
				//TODO convert image path to url path
				return value as string;
			else if (field is HtmlField)
				return value as string;
			else if (field is MultiLineTextField)
				return value as string;
			else if (field is MultiSelectField)
			{
				if (value == null)
					return null;
				else if (value is JArray)
					return ((JArray)value).Select(x => ((JToken)x).Value<string>()).ToList<string>();
				else if (value is List<object>)
					return ((List<object>)value).Select(x => ((object)x).ToString()).ToList<string>();
				else
					return value as IEnumerable<string>;
			}
			else if (field is NumberField)
			{
				if (value == null)
					return null;
				if (value is string)
					return decimal.Parse(value as string);

				return Convert.ToDecimal(value);
			}
			else if (field is PasswordField)
			{
				if (encryptPasswordFields)
				{
					if (((PasswordField)field).Encrypted == true)
					{
						if (string.IsNullOrWhiteSpace(value as string))
							return null;

						return PasswordUtil.GetMd5Hash(value as string);
					}
				}
				return value;
			}
			else if (field is PercentField)
			{
				if (value == null)
					return null;
				if (value is string)
					return decimal.Parse(value as string);

				return Convert.ToDecimal(value);
			}
			else if (field is PhoneField)
				return value as string;
			else if (field is GuidField)
			{
				if (value is string)
				{
					if (string.IsNullOrWhiteSpace(value as string))
						return null;

					return new Guid(value as string);
				}

				if (value is Guid)
					return (Guid?)value;

				if (value == null)
					return (Guid?)null;

				throw new Exception("Invalid Guid field value.");
			}
			else if (field is SelectField)
				return value as string;
			else if (field is TextField)
				return value as string;
			else if (field is UrlField)
				return value as string;
			else if (field is TreeSelectField)
			{
				if (value == null)
					return null;
				else if (value is JArray)
					return ((JArray)value).Select(x => new Guid(((JToken)x).Value<string>())).ToList<Guid>();
				else if (value is List<object>)
					return ((List<object>)value).Select(x => ((Guid)x)).ToList<Guid>();
				else
					return value as IEnumerable<Guid>;
			}


			throw new Exception("System Error. A field type is not supported in field value extraction process.");
		}

		public List<EntityRecord> Find( EntityQuery query )
		{
			var entity = DbContext.Current.EntityRepository.Read(query.EntityName).MapTo<Entity>();
			var fields = ExtractQueryFieldsMeta(query);
            StringBuilder sql = new StringBuilder();
			StringBuilder sqlJoins = new StringBuilder();
			StringBuilder sqlGroupBy = new StringBuilder();

			sql.AppendLine(BEGIN_OUTER_SELECT);
			sql.AppendLine(BEGIN_SELECT);
			foreach (var field in fields)
			{
				if (!(field is RelationFieldMeta))
				{
					sql.AppendLine(string.Format(REGULAR_FIELD_SELECT, field.Name, GetTableNameForEntity(entity.Name)));
					sqlGroupBy.Append(GetTableNameForEntity(entity) + "." + field.Name + ",");
				}
				else
				{
					RelationFieldMeta relationField = (RelationFieldMeta)field;
					var relationName = relationField.Relation.Name;

					//here we don't have any direction, because it is set by join clause bellow
					StringBuilder sbRelatedFields = new StringBuilder();
					foreach (var f in relationField.Fields)
						sbRelatedFields.Append(string.Format("{0}.{1},", relationName, f.Name));

					sbRelatedFields.Remove(sbRelatedFields.Length - 1, 1);

					//sql.AppendLine(string.Format(JOIN_FIELD_SELECT, field.Name, sbRelatedFields));

					if (relationField.Relation.RelationType == EntityRelationType.OneToOne)
					{
						//when the relation is origin -> target entity
						if (relationField.Relation.OriginEntityId == entity.Id)
						{
							//join target entity
							//sqlJoins.AppendLine(string.Format(JOIN, GetTableNameForEntity(relationField.TargetEntity), relationName, relationName,
							//	relationField.TargetField.Name, GetTableNameForEntity(relationField.OriginEntity), relationField.OriginField.Name));

							sql.AppendLine(string.Format(OTM_RELATION_TEMPLATE,
								field.Name,
								sbRelatedFields.ToString(),
								GetTableNameForEntity(relationField.TargetEntity),
								relationName,
								relationField.TargetField.Name,
								GetTableNameForEntity(relationField.OriginEntity),
								relationField.OriginField.Name));
						}
						else //when the relation is target -> origin, we have to query origin entity
						{
							//join origin entity
							//sqlJoins.AppendLine(string.Format(JOIN, GetTableNameForEntity(relationField.OriginEntity), relationName, relationName,
							//	relationField.OriginField.Name, GetTableNameForEntity(relationField.TargetEntity), relationField.TargetField.Name));

							sql.AppendLine(string.Format(OTM_RELATION_TEMPLATE,
								field.Name,
								sbRelatedFields.ToString(),
								GetTableNameForEntity(relationField.OriginEntity),
								relationName,
								relationField.OriginField.Name,
								GetTableNameForEntity(relationField.TargetEntity),
								relationField.TargetField.Name));
						}
					}
					else if (relationField.Relation.RelationType == EntityRelationType.OneToMany)
					{
						//when origin and target entity are different, then direction don't matter
						if (relationField.Relation.OriginEntityId != relationField.Relation.TargetEntityId)
						{
							//when the relation is origin -> target entity
							if (relationField.Relation.OriginEntityId == entity.Id)
							{
								//join target entity
								//sqlJoins.AppendLine(string.Format(JOIN, GetTableNameForEntity(relationField.TargetEntity), relationName, relationName,
								//	relationField.TargetField.Name, GetTableNameForEntity(relationField.OriginEntity), relationField.OriginField.Name));

								sql.AppendLine(string.Format(OTM_RELATION_TEMPLATE,
									field.Name,
									sbRelatedFields.ToString(),
									GetTableNameForEntity(relationField.TargetEntity),
									relationName,
									relationField.TargetField.Name,
									GetTableNameForEntity(relationField.OriginEntity),
									relationField.OriginField.Name));
							}
							else //when the relation is target -> origin, we have to query origin entity
							{
								//join origin entity
								//sqlJoins.AppendLine(string.Format(JOIN, GetTableNameForEntity(relationField.OriginEntity), relationName, relationName,
								//	relationField.OriginField.Name, GetTableNameForEntity(relationField.TargetEntity), relationField.TargetField.Name));
								sql.AppendLine(string.Format(OTM_RELATION_TEMPLATE,
									field.Name,
									sbRelatedFields.ToString(),
									GetTableNameForEntity(relationField.OriginEntity),
									relationName,
									relationField.OriginField.Name,
									GetTableNameForEntity(relationField.TargetEntity),
									relationField.TargetField.Name));
							}
						}
						else //when the origin entity is same as target entity direction matters
						{
							if (relationField.Direction == "target-origin")
							{
								//join origin entity
								//sqlJoins.AppendLine(string.Format(JOIN, GetTableNameForEntity(relationField.OriginEntity), relationName, relationName,
								//	relationField.OriginField.Name, GetTableNameForEntity(relationField.TargetEntity), relationField.TargetField.Name));

								sql.AppendLine(string.Format(OTM_RELATION_TEMPLATE,
									field.Name,
									sbRelatedFields.ToString(),
									GetTableNameForEntity(relationField.OriginEntity),
									relationName,
									relationField.OriginField.Name,
									GetTableNameForEntity(relationField.TargetEntity),
									relationField.TargetField.Name));
							}
							else
							{
								//join target entity
								//sqlJoins.AppendLine(string.Format(JOIN, GetTableNameForEntity(relationField.TargetEntity), relationName, relationName,
								//	relationField.TargetField.Name, GetTableNameForEntity(relationField.OriginEntity), relationField.OriginField.Name));
								sql.AppendLine(string.Format(OTM_RELATION_TEMPLATE,
									field.Name,
									sbRelatedFields.ToString(),
									GetTableNameForEntity(relationField.TargetEntity),
									relationName,
									relationField.TargetField.Name,
									GetTableNameForEntity(relationField.OriginEntity),
									relationField.OriginField.Name));
							}
						}
					}
					else if (relationField.Relation.RelationType == EntityRelationType.ManyToMany)
					{
						string relationTable = "rel_" + relationField.Relation.Name;
						string targetJoinAlias = relationName + "_target";
						string originJoinAlias = relationName + "_origin";


						if (relationField.Relation.OriginEntityId == relationField.Relation.TargetEntityId)
						{
							if (relationField.Direction == "target-origin")
							{
								//sqlJoins.AppendLine(string.Format(JOIN, relationTable, targetJoinAlias,
								//	 targetJoinAlias, "target_id", GetTableNameForEntity(entity), relationField.TargetField.Name));

								//sqlJoins.AppendLine(string.Format(JOIN, GetTableNameForEntity(relationField.OriginEntity), relationName,
								//			 relationName, relationField.OriginField.Name, targetJoinAlias, "origin_id"));

								sql.AppendLine(string.Format(MTM_RELATION_TEMPLATE,
									field.Name,
									sbRelatedFields.ToString(),
									GetTableNameForEntity(relationField.OriginEntity),
									relationName,
									relationTable,
									targetJoinAlias,
									targetJoinAlias,
									"target_id",
									GetTableNameForEntity(entity),
									relationField.TargetField.Name,
									relationName,
									relationField.OriginField.Name,
									targetJoinAlias,
									"origin_id"));
							}
							else
							{
								//sqlJoins.AppendLine(string.Format(JOIN, relationTable, originJoinAlias,
								//	originJoinAlias, "origin_id", GetTableNameForEntity(entity), relationField.OriginField.Name));

								//sqlJoins.AppendLine(string.Format(JOIN, GetTableNameForEntity(relationField.TargetEntity), relationName,
								//	 originJoinAlias, "target_id", relationName, relationField.TargetField.Name));

								sql.AppendLine(string.Format(MTM_RELATION_TEMPLATE,
									field.Name,
									sbRelatedFields.ToString(),
									GetTableNameForEntity(relationField.TargetEntity),
									relationName,
									relationTable,
									originJoinAlias,
									originJoinAlias,
									"origin_id",
									GetTableNameForEntity(entity),
									relationField.OriginField.Name,
									relationName,
									relationField.OriginField.Name,
									originJoinAlias,
									"target_id"));
							}
						}
						else if (relationField.Relation.OriginEntityId == entity.Id)
						{
							//sqlJoins.AppendLine(string.Format(JOIN, relationTable, originJoinAlias,
							//	 originJoinAlias, "origin_id", GetTableNameForEntity(entity), relationField.OriginField.Name));

							//sqlJoins.AppendLine(string.Format(JOIN, GetTableNameForEntity(relationField.TargetEntity), relationName,
							//				 originJoinAlias, "target_id", relationName, relationField.TargetField.Name));

							//		const string MTM_RELATION_TEMPLATE = @"'{0}', ( SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
							//			SELECT {1}
							//			FROM {2} {3}
							//			LEFT JOIN  {4} {5} ON {6}.{7} = {8}.{9}
							//			WHERE {10}.{11} = {12}.{13} )d  ),";


							sql.AppendLine(string.Format(MTM_RELATION_TEMPLATE,
									field.Name,
									sbRelatedFields.ToString(),
									GetTableNameForEntity(relationField.TargetEntity),
									relationName,
									relationTable,
									originJoinAlias,
									originJoinAlias,
									"origin_id",
									GetTableNameForEntity(entity),
									relationField.OriginField.Name,
									relationName,
									relationField.OriginField.Name,
									originJoinAlias,
									"target_id"));
						}
						else //when the relation is target -> origin, we have to query origin entity
						{
							//sqlJoins.AppendLine(string.Format(JOIN, relationTable, targetJoinAlias,
							//		 targetJoinAlias, "target_id", GetTableNameForEntity(entity), relationField.TargetField.Name));

							//sqlJoins.AppendLine(string.Format(JOIN, GetTableNameForEntity(relationField.OriginEntity), relationName,
							//			targetJoinAlias, "origin_id", relationName, relationField.OriginField.Name));

							sql.AppendLine(string.Format(MTM_RELATION_TEMPLATE,
									field.Name,
									sbRelatedFields.ToString(),
									GetTableNameForEntity(relationField.OriginEntity),
									relationName,
									relationTable,
									targetJoinAlias,
									targetJoinAlias,
									"target_id",
									GetTableNameForEntity(entity),
									relationField.TargetField.Name,
									relationName,
									relationField.OriginField.Name,
									targetJoinAlias,
									"origin_id"));
						}
					}
				}
			}
			sql.Remove(sql.Length - 3, 3); //remove newline and comma
			sql.AppendLine(END_SELECT);
			sql.AppendLine(string.Format(FROM, GetTableNameForEntity(entity)));

			//where clause
			string whereSql = string.Empty;
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			if (query.Query != null)
			{
				GenerateWhereClause(query.Query, entity, ref whereSql, ref parameters);
				sql.AppendLine("WHERE " + whereSql);
			}

			//sorting
			if (query.Sort != null && query.Sort.Length > 0)
			{
				string sortSql = "ORDER BY ";

				foreach (var s in query.Sort)
				{
					sortSql = sortSql + " " + GetTableNameForEntity(entity) + "." + s.FieldName;
					if (s.SortType == QuerySortType.Ascending)
						sortSql = sortSql + " ASC,";
					else
						sortSql = sortSql + " DESC,";
				}

				sortSql = sortSql.Remove(sortSql.Length - 1, 1);
				sql.AppendLine(sortSql);
			}

			//paging 
			if (query.Limit != null || query.Skip != null)
			{
				string pagingSql = "LIMIT ";
				if (query.Limit.HasValue)
					pagingSql = pagingSql + query.Limit + " ";
				else
					pagingSql = pagingSql + "ALL ";

				if (query.Skip.HasValue)
					pagingSql = pagingSql + " OFFSET " + query.Skip;

				sql.AppendLine(pagingSql);
			}

			sql.AppendLine(END_OUTER_SELECT);

			DataTable dt = new DataTable();
			using (var conn = DbContext.Current.CreateConnection())
			{
				NpgsqlCommand command = conn.CreateCommand(sql.ToString());
				command.Parameters.AddRange(parameters.ToArray());
				new NpgsqlDataAdapter(command).Fill(dt);
			}

			List<EntityRecord> result = new List<EntityRecord>();

			foreach (DataRow dr in dt.Rows)
			{
				var jObj = JObject.Parse((string)dr[0]);
				result.Add(ConvertJObjectToEntityRecord(jObj, fields));
			}

			return result;

		}

		private void GenerateWhereClause(QueryObject query, Entity entity, ref string sql, ref List<NpgsqlParameter> parameters)
		{
			if (sql.Length > 0)
				sql = sql + " AND ";

			Field field = null;
			FieldType fieldType = FieldType.GuidField;
			string paramName = null;
			string completeFieldName = null;
			if (!string.IsNullOrWhiteSpace(query.FieldName))
			{
				field = entity.Fields.SingleOrDefault(x => x.Name == query.FieldName);
				fieldType = field.GetFieldType();
				string entityTablePrefix = GetTableNameForEntity(entity) + ".";
				completeFieldName = entityTablePrefix + query.FieldName;
				paramName = "@" + query.FieldName + "_" + Guid.NewGuid().ToString().Replace("-", "");
				parameters.Add(new NpgsqlParameter(paramName, query.FieldValue));


				if ((fieldType == FieldType.MultiSelectField || fieldType == FieldType.TreeSelectField) &&
					  !(query.QueryType == QueryType.EQ || query.QueryType == QueryType.NOT))
					throw new Exception("The query operator is not supported on field '" + fieldType.ToString() + "'");
			}

			switch (query.QueryType)
			{
				case QueryType.EQ:
					{
						if (fieldType == FieldType.MultiSelectField)
						{
							var parameter = parameters.Single(x => x.ParameterName == paramName);
							parameter.Value = new List<string>() { (string)query.FieldValue };
							sql = sql + " " + paramName + " IN ( " + completeFieldName + " )";
						}
						else if (fieldType == FieldType.TreeSelectField)
						{
							var parameter = parameters.Single(x => x.ParameterName == paramName);
							parameter.Value = new List<Guid>() { (Guid)query.FieldValue };
							sql = sql + " " + paramName + " IN ( " + completeFieldName + " )";
						}
						else
							sql = sql + " " + completeFieldName + "=" + paramName;

						return;
					}
				case QueryType.NOT:
					{
						if (fieldType == FieldType.MultiSelectField)
						{
							var parameter = parameters.Single(x => x.ParameterName == paramName);
							parameter.Value = new List<string>() { (string)query.FieldValue };
							sql = sql + " " + paramName + " NOT IN ( " + completeFieldName + " )";
						}
						else if (fieldType == FieldType.TreeSelectField)
						{
							var parameter = parameters.Single(x => x.ParameterName == paramName);
							parameter.Value = new List<Guid>() { (Guid)query.FieldValue };
							sql = sql + " " + paramName + " NOT IN ( " + completeFieldName + " )";
						}
						else
							sql = sql + " " + completeFieldName + "<>" + paramName;

						return;
					}
				case QueryType.LT:
					{
						sql = sql + " " + completeFieldName + "<" + paramName;
						return;
					}
				case QueryType.LTE:
					{
						sql = sql + " " + completeFieldName + "<=" + paramName;
						return;
					}
				case QueryType.GT:
					{
						sql = sql + " " + completeFieldName + ">" + paramName;
						return;
					}
				case QueryType.GTE:
					{
						sql = sql + " " + completeFieldName + ">=" + paramName;
						return;
					}
				case QueryType.CONTAINS:
					{
						var parameter = parameters.Single(x => x.ParameterName == paramName);
						parameter.Value = "%" + parameter.Value + "%";
						sql = sql + " " + completeFieldName + " ILIKE " + paramName;
						return;
					}
				case QueryType.STARTSWITH:
					{
						var parameter = parameters.Single(x => x.ParameterName == paramName);
						parameter.Value = parameter.Value + "%";
						sql = sql + " " + completeFieldName + " ILIKE " + paramName;
						return;
					}
				case QueryType.REGEX:
					{
						var regexOperator = "~";
						switch (query.RegexOperator)
						{
							case QueryObjectRegexOperator.MatchCaseSensitive:
								regexOperator = "~";
								break;
							case QueryObjectRegexOperator.MatchCaseInsensitive:
								regexOperator = "~*";
								break;
							case QueryObjectRegexOperator.DontMatchCaseSensitive:
								regexOperator = "!~";
								break;
							case QueryObjectRegexOperator.DontMatchCaseInsensitive:
								regexOperator = "!~*";
								break;
						}

						sql = sql + " " + completeFieldName + " " + regexOperator + " " + paramName;
						return;
					}
				case QueryType.RELATED:
					{
						//TODO
						throw new NotImplementedException();
						return;
					}
				case QueryType.NOTRELATED:
					{
						//TODO
						throw new NotImplementedException();
						return;
					}
				case QueryType.AND:
					{
						if (query.SubQueries.Count == 1)
							GenerateWhereClause(query.SubQueries[0], entity, ref sql, ref parameters);
						else
						{
							string andSql = string.Empty;
							foreach (var q in query.SubQueries)
							{
								string subQuerySql = string.Empty;
								GenerateWhereClause(q, entity, ref subQuerySql, ref parameters);
								if (andSql.Length == 0)
									andSql = subQuerySql;
								else
									andSql = andSql + " AND " + subQuerySql;
							}
							sql = sql + " ( " + andSql + " )";
						}
						return;
					}
				case QueryType.OR:
					{
						if (query.SubQueries.Count == 1)
							GenerateWhereClause(query.SubQueries[0], entity, ref sql, ref parameters);
						else
						{
							string andSql = string.Empty;
							foreach (var q in query.SubQueries)
							{
								string subQuerySql = string.Empty;
								GenerateWhereClause(q, entity, ref subQuerySql, ref parameters);
								if (andSql.Length == 0)
									andSql = subQuerySql;
								else
									andSql = andSql + " OR " + subQuerySql;
							}
							sql = sql + " ( " + andSql + " )";
						}
						return;
					}
				default:
					throw new Exception("Not supported query type");
			}
		}

		private string GetTableNameForEntity(Entity entity)
		{
			return GetTableNameForEntity(entity.Name);
		}

		private string GetTableNameForEntity(string entityName)
		{
			return RECORD_COLLECTION_PREFIX + entityName;
		}

		internal List<Field> ExtractQueryFieldsMeta(EntityQuery query)
		{
			List<EntityRelation> relations = new EntityRelationManager().Read().Object;
            List<Field> result = new List<Field>();

			//split field string into tokens speparated by FIELDS_SEPARATOR
			List<string> tokens = query.Fields.Split(FIELDS_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

			//check the query tokens for widcard symbol and validate it is only that symbol
			if (tokens.Count > 1 && tokens.Any(x => x == WILDCARD_SYMBOL))
				throw new Exception("Invalid query syntax. Wildcard symbol can be used only with no other fields.");

			Entity entity = DbContext.Current.EntityRepository.Read(query.EntityName).MapTo<Entity>();
			if (entity == null)
				throw new Exception(string.Format("The entity '{0}' does not exists.", query.EntityName));

			//We check for wildcard symbol and if present include all fields of the queried entity 
			bool wildcardSelectionEnabled = tokens.Any(x => x == WILDCARD_SYMBOL);
			if (wildcardSelectionEnabled)
			{
				result.AddRange(entity.Fields);
				return result;
			}

			//process only tokens do not contain RELATION_SEPARATOR 
			foreach (var token in tokens)
			{
				if (!token.Contains(RELATION_SEPARATOR))
				{
					//locate the field
					var field = entity.Fields.SingleOrDefault(x => x.Name == token);

					//found no field for specified token
					if (field == null)
						throw new Exception(string.Format("Invalid query result field '{0}'. The field name is incorrect.", token));

					//check for duplicated field tokens and ignore them
					if (!result.Any(x => x.Id == field.Id))
						result.Add(field);
				}
				else
				{
					var relationData = token.Split(RELATION_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
					if (relationData.Count > 2)
						throw new Exception(string.Format("The specified query result  field '{0}' is incorrect. Only first level relation can be specified.", token));

					string relationName = relationData[0];
					string relationFieldName = relationData[1];
					string direction = "origin-target";

					if (string.IsNullOrWhiteSpace(relationName) || relationName == "$" || relationName == "$$")
						throw new Exception(string.Format("Invalid relation '{0}'. The relation name is not specified.", token));
					else if (!relationName.StartsWith("$"))
						throw new Exception(string.Format("Invalid relation '{0}'. The relation name is not correct.", token));
					else
						relationName = relationName.Substring(1);

					//check for target priority mark $$
					if (relationName.StartsWith("$"))
					{
						direction = "target-origin";
						relationName = relationName.Substring(1);
					}

					if (string.IsNullOrWhiteSpace(relationFieldName))
						throw new Exception(string.Format("Invalid query result field '{0}'. The relation field name is not specified.", token));



					Field field = result.SingleOrDefault(x => x.Name == "$" + relationName);
					RelationFieldMeta relationFieldMeta = null;
					if (field == null)
					{
						relationFieldMeta = new RelationFieldMeta();
						relationFieldMeta.Name = "$" + relationName;
						relationFieldMeta.Direction = direction;
						result.Add(relationFieldMeta);
					}
					else
						relationFieldMeta = (RelationFieldMeta)field;
					
					
					relationFieldMeta.Relation = relations.SingleOrDefault(x => x.Name == relationName);
					if (relationFieldMeta.Relation == null)
						throw new Exception(string.Format("Invalid relation '{0}'. The relation does not exist.", token));

					if (relationFieldMeta.Relation.TargetEntityId != entity.Id && relationFieldMeta.Relation.OriginEntityId != entity.Id)
						throw new Exception(string.Format("Invalid relation '{0}'. The relation does relate to queries entity.", token));

					if (relationFieldMeta.Direction != direction)
						throw new Exception(string.Format("You are trying to query relation '{0}' from origin->target and target->origin direction in single query. This is not allowed.", token));

					relationFieldMeta.TargetEntity = DbContext.Current.EntityRepository.Read(relationFieldMeta.Relation.TargetEntityId).MapTo<Entity>();
					relationFieldMeta.OriginEntity = DbContext.Current.EntityRepository.Read(relationFieldMeta.Relation.OriginEntityId).MapTo<Entity>();

					//this should not happen in a perfect (no bugs) world
					if (relationFieldMeta.OriginEntity == null)
						throw new Exception(string.Format("Invalid query result field '{0}'. Related (origin)entity is missing.", token));
					if (relationFieldMeta.TargetEntity == null)
						throw new Exception(string.Format("Invalid query result field '{0}'. Related (target)entity is missing.", token));

					relationFieldMeta.TargetField = relationFieldMeta.TargetEntity.Fields.Single(x => x.Id == relationFieldMeta.Relation.TargetFieldId);
					relationFieldMeta.OriginField = relationFieldMeta.OriginEntity.Fields.Single(x => x.Id == relationFieldMeta.Relation.OriginFieldId);

					//this should not happen in a perfect (no bugs) world
					if (relationFieldMeta.OriginField == null)
						throw new Exception(string.Format("Invalid query result field '{0}'. Related (origin)field is missing.", token));
					if (relationFieldMeta.TargetField == null)
						throw new Exception(string.Format("Invalid query result field '{0}'. Related (target)field is missing.", token));

					Entity joinToEntity = null;
					if (relationFieldMeta.TargetEntity.Id == entity.Id)
						joinToEntity = relationFieldMeta.OriginEntity;
					else
						joinToEntity = relationFieldMeta.TargetEntity;

					relationFieldMeta.Entity = joinToEntity;

					var relatedField = joinToEntity.Fields.SingleOrDefault(x => x.Name == relationFieldName);
					if (relatedField == null)
						throw new Exception(string.Format("Invalid query result field '{0}'. The relation field does not exist.", token));

					//add id field of related entity
					if (relatedField.Name != "id")
					{
						var relatedIdField = joinToEntity.Fields.SingleOrDefault(x => x.Name == "id");

						//if field already added
						if (!relationFieldMeta.Fields.Any(x => x.Id == relatedIdField.Id))
							relationFieldMeta.Fields.Add(relatedIdField);
					}

					//if field already added
					if (relationFieldMeta.Fields.Any(x => x.Id == relatedField.Id))
						continue;


					relationFieldMeta.Fields.Add(relatedField);
				}
			}

			return result;
		}
	}
}
