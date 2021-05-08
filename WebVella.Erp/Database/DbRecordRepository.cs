using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Database.Models;
using WebVella.Erp.Fts;
using WebVella.Erp.Utilities;

namespace WebVella.Erp.Database
{
    public class DbRecordRepository
    {
        #region <--- Constants --->

        private const string WILDCARD_SYMBOL = "*";
        private const char FIELDS_SEPARATOR = ',';
        private const char RELATION_SEPARATOR = '.';
        private const char RELATION_NAME_RESULT_SEPARATOR = '$';

        internal const string RECORD_COLLECTION_PREFIX = "rec_";
        const string BEGIN_OUTER_SELECT = @"SELECT row_to_json( X ) FROM (";
        const string BEGIN_SELECT = @"SELECT ";
        const string REGULAR_FIELD_SELECT = @" {1}.""{0}"" AS ""{0}"",";
        const string END_SELECT = @"";
        const string BEGIN_SELECT_DISTINCT = @"SELECT DISTINCT ";
        const string END_OUTER_SELECT = @") X";
        const string FROM = @"FROM {0}";

        //const string GROUPBY = @"GROUP BY {0}";

        const string OTM_RELATION_TEMPLATE = @"	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
					SELECT {1} 
					FROM {2} {3}
					WHERE {3}.{4} = {5}.{6} ) d )::jsonb AS ""{0}"",";

        const string MTM_RELATION_TEMPLATE = @"( SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
					SELECT {1}
					FROM {2} {3}
					LEFT JOIN  {4} {5} ON {6}.{7} = {8}.{9}
					WHERE {10}.{11} = {12}.{13} )d  )::jsonb AS ""{0}"",";

        const string FILTER_JOIN = @"LEFT OUTER JOIN  {0} {1} ON {2}.{3} = {4}.{5}";



        #endregion

        EntityManager entMan;
        EntityRelationManager relMan;
		FtsAnalyzer ftsAnalyzer = new FtsAnalyzer();
		private DbContext suppliedContext = null;
		public DbContext CurrentContext
		{
			get
			{
				if (suppliedContext != null)
					return suppliedContext;
				else
					return DbContext.Current;
			}
			set
			{
				suppliedContext = value;
			}
		}
		public DbRecordRepository(DbContext currentContext)
		{
			if (currentContext != null)
				suppliedContext = currentContext;

			entMan = new EntityManager(CurrentContext);
			relMan = new EntityRelationManager(CurrentContext);
		}


		public void Create(string entityName, IEnumerable<KeyValuePair<string, object>> recordData)
		{
			Entity entity = entMan.ReadEntity(entityName).Object;

			List<DbParameter> parameters = new List<DbParameter>();

			foreach (var record in recordData)
			{
				Field field = entity.Fields.FirstOrDefault(f => f.Name.ToLowerInvariant() == record.Key.ToLowerInvariant());

				DbParameter param = new DbParameter();
				param.Name = field.Name;
				param.Value = record.Value ?? DBNull.Value;
				if (field.GetFieldType() == FieldType.GeographyField)
				{
					// this is set as text because later
					// the generated SQL will be something like

					// INSERT INTO places 
					//  (id, 
					//  border) 
					// VALUES 
					//  (@id, 
					//  ST_Transform(ST_GeomFromGeoJSON(@border),4326)::geography)
					// 
					param.Type = NpgsqlDbType.Text;
					GeographyField geo = (field as GeographyField);

					if (param.Value == null || (string)param.Value == "")
					{
						if (geo.Format == GeographyFieldFormat.GeoJSON)
						{
							param.Value = "{\"type\":\"GeometryCollection\",\"geometries\":[]}";
						}
						else if (geo.Format == GeographyFieldFormat.Text)
						{
							param.Value = "GEOMETRYCOLLECTION EMPTY";
						}

					}

					param.ValueOverride = $"ST_Transform(ST_GeomFrom{geo.Format.Value.ToString()}(@{param.Name}{(geo.Format.Value == GeographyFieldFormat.Text ? ", " + geo.SRID : "")}),{geo.SRID})::geography";

				}
				else
				{
					param.Type = DbTypeConverter.ConvertToDatabaseType(field.GetFieldType());
				}
				parameters.Add(param);
			}

			string tableName = RECORD_COLLECTION_PREFIX + entityName;
			DbRepository.InsertRecord(tableName, parameters);
		}
		public void Update(string entityName, IEnumerable<KeyValuePair<string, object>> recordData)
		{
			Entity entity = entMan.ReadEntity(entityName).Object;

			List<DbParameter> parameters = new List<DbParameter>();
			Guid? id = null;

			foreach (var record in recordData)
			{
				Field field = entity.Fields.FirstOrDefault(f => f.Name.ToLowerInvariant() == record.Key.ToLowerInvariant());

				if (field.Name == "id")
					id = (Guid)record.Value;

				DbParameter param = new DbParameter();
				param.Name = field.Name;
				if (field.GetFieldType() == FieldType.GeographyField)
				{
					// this is set as text because later
					// the generated SQL will be something like

					// INSERT INTO places 
					//  (id, 
					//  border) 
					// VALUES 
					//  (@id, 
					//  ST_Transform(ST_GeomFromGeoJSON(@border),4326)::geography)
					// 
					param.Type = NpgsqlDbType.Text;
					GeographyField geo = (field as GeographyField);
					param.Value = record.Value;

					param.ValueOverride = $"ST_Transform(ST_GeomFrom{geo.Format.Value.ToString()}(@{param.Name}{(geo.Format.Value == GeographyFieldFormat.Text ? ", " + geo.SRID : "")}),{geo.SRID})::geography";

				}
				else
				{
					param.Value = record.Value ?? DBNull.Value;
					param.Type = DbTypeConverter.ConvertToDatabaseType(field.GetFieldType());
				}
				parameters.Add(param);

			}

			if (!id.HasValue)
				throw new StorageException("ID is missing. Cannot update records without ID specified.");

			string tableName = RECORD_COLLECTION_PREFIX + entityName;

			var updateSuccess = DbRepository.UpdateRecord(tableName, parameters);
			if (!updateSuccess)
				throw new StorageException("Failed to update record.");
		}

		public void Delete(string entityName, Guid id)
        {
            string tableName = RECORD_COLLECTION_PREFIX + entityName;

            EntityRecord outRecord = Find(entityName, id);
            if (outRecord == null)
                throw new StorageException("There is no record with such id to update.");

            DbRepository.DeleteRecord(tableName, id);
        }

        public EntityRecord FindTreeNodeRecord(string entityName, Guid id)
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
                            record[reader.GetName(index)] = reader[index] == DBNull.Value ? null : reader[index];

                    }
                    else
                    {
                        return null;
                    }

                    reader.Close();

                }
                return record;
            }
        }

        private static bool ContainsRelationalQuery(QueryObject query)
        {
            Queue<QueryObject> queue = new Queue<QueryObject>();

            if (query == null)
                return false;

            queue.Enqueue(query);
            while(queue.Count > 0 )
            {
                var q = queue.Dequeue();
                if( q.SubQueries != null && q.SubQueries.Count > 0 )
                {
                    foreach (var sq in q.SubQueries)
                        queue.Enqueue(sq);
                }
                if (q.FieldName != null && q.FieldName.Contains(RELATION_SEPARATOR))
                    return true;
            }

            return false;

        }

        public long Count(EntityQuery query)
        {
            string tableName = RECORD_COLLECTION_PREFIX + query.EntityName;
            using (DbConnection con = DbContext.Current.CreateConnection())
            {
                string sql = $"SELECT COUNT( {tableName}.id ) FROM {tableName} ";
                if(ContainsRelationalQuery(query.Query))
                    sql = $"SELECT COUNT( DISTINCT {tableName}.id ) FROM {tableName} ";

                string whereSql = string.Empty;
                string whereJoinSql = string.Empty;

                Entity entity = new EntityManager().ReadEntity(query.EntityName).Object;
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
				if( query != null )
					GenerateWhereClause(query.Query, entity, ref whereSql, ref whereJoinSql, ref parameters, query.OverwriteArgs);

                if (whereJoinSql.Length > 0)
                    sql = sql + "  " + whereJoinSql;

                if (whereSql.Length > 0)
                    sql = sql + " WHERE " + whereSql;

                NpgsqlCommand command = con.CreateCommand(sql);

                if (parameters.Count > 0)
                    command.Parameters.AddRange(parameters.ToArray());

                return (long)command.ExecuteScalar();
            }
        }


        public void CreateRecordField(string entityName, Field field)
        {
            string tableName = RECORD_COLLECTION_PREFIX + entityName;

            DbRepository.CreateColumn(tableName, field);
            if (field.Unique)
                DbRepository.CreateUniqueConstraint("idx_u_" + entityName + "_" + field.Name, tableName, new List<string> { field.Name });
            if (field.Searchable)
                DbRepository.CreateIndex("idx_s_" + entityName + "_" + field.Name, tableName, field.Name, field);
        }

        public void UpdateRecordField(string entityName, Field field)
        {
			//don't update default value for auto number field
			if (field.GetFieldType() == FieldType.AutoNumberField)
				return;

            string tableName = RECORD_COLLECTION_PREFIX + entityName;

			bool overrideNulls = field.Required && field.GetFieldDefaultValue() != null;
			DbRepository.SetColumnDefaultValue(RECORD_COLLECTION_PREFIX + entityName, field, overrideNulls);

			DbRepository.SetColumnNullable(RECORD_COLLECTION_PREFIX + entityName, field.Name, !field.Required);
			
           


            if (field.Searchable)
                DbRepository.CreateIndex("idx_s_" + entityName + "_" + field.Name, tableName, field.Name, field);
            else
                DbRepository.DropIndex("idx_s_" + entityName + "_" + field.Name);
        }

        public void RemoveRecordField(string entityName, Field field)
        {
            string tableName = RECORD_COLLECTION_PREFIX + entityName;

            //probably constraint will be removed automatically by postgresql, but to be sure
            if (field.Unique)
                DbRepository.DropUniqueConstraint("idx_u_" + entityName + "_" + field.Name, tableName);
            if (field.Searchable)
                DbRepository.CreateIndex("idx_s_" + entityName + "_" + field.Name, tableName, field.Name, field);

            DbRepository.DeleteColumn(tableName, field.Name);
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

        public static object ExtractFieldValue(object value, Field field, bool encryptPasswordFields = false)
        {
            if (value == null)
                return field.GetFieldDefaultValue();

			if (value is JToken)
			{
				//we convert JToken to string for specified types, because when date formated string 
				//is saved in JToken value, it get converted to DateTime. It may happen with other specific texts also.
				if( field is EmailField || field is FileField || field is ImageField ||
					field is HtmlField || field is MultiLineTextField || field is PasswordField ||
					field is PhoneField || field is SelectField || field is TextField || field is UrlField ||
					field is GeographyField)
					value = ((JToken)value).ToObject<string>();
				else
					value = ((JToken)value).ToObject<object>();
			}

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
				{
					if (string.IsNullOrWhiteSpace(value as string))
						return null;
					date = DateTime.Parse(value as string);
					switch (date.Value.Kind)
					{
						case DateTimeKind.Utc:
							return date.Value.ConvertToAppDate();
						case DateTimeKind.Local:
							return date.Value.ConvertToAppDate();
						case DateTimeKind.Unspecified:
							return date.Value;
					}
				}
				else
				{
					date = value as DateTime?;
					switch (date.Value.Kind)
					{
						case DateTimeKind.Utc:
							return date.Value.ConvertToAppDate();
						case DateTimeKind.Local:
							return date.Value.ConvertToAppDate();
						case DateTimeKind.Unspecified:
							return date.Value;
					}
				}
				return date;
			}
			else if (field is DateTimeField)
			{
				if (value == null)
					return null;

				DateTime? date = null;
				if (value is string)
				{
					if (string.IsNullOrWhiteSpace(value as string))
						return null;
					date = DateTime.Parse(value as string);
					////date can be local, utc and unspecified
					////if local convert to utc, unspecified is used as is
					//if (date.HasValue && date.Value.Kind == DateTimeKind.Local)
					//	date = date.Value.ToUniversalTime();

					switch (date.Value.Kind)
					{
						case DateTimeKind.Utc:
							return date;
						case DateTimeKind.Local:
							return date.Value.ToUniversalTime();
						case DateTimeKind.Unspecified:
							{
								var erpTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ErpSettings.TimeZoneName);
								return TimeZoneInfo.ConvertTimeToUtc(date.Value, erpTimeZone);
							}
					}
				}
				else
				{
					date = value as DateTime?;
					////date can be local, utc and unspecified
					////if local convert to utc, unspecified is used as is
					//if (date.HasValue && date.Value.Kind == DateTimeKind.Local)
					//	date = date.Value.ToUniversalTime();

					switch (date.Value.Kind)
					{
						case DateTimeKind.Utc:
							return date;
						case DateTimeKind.Local:
							return date.Value.ToUniversalTime();
						case DateTimeKind.Unspecified:
							{
								var erpTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ErpSettings.TimeZoneName);
								return TimeZoneInfo.ConvertTimeToUtc(date.Value, erpTimeZone);
							}
					}
				}

				//if (date != null)
				//	return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0, DateTimeKind.Utc);
				return date;
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
			else if (field is GeographyField)
				return value as string;
			else if (field is MultiSelectField)
			{
				if (value == null)
					return null;
				else if (value is JArray)
					return ((JArray)value).Select(x => ((JToken)x).Value<string>()).ToList<string>();
				else if (value is List<object>)
					return ((List<object>)value).Select(x => ((object)x).ToString()).ToList<string>();
				else if (value is string[])
					return new List<string>(value as string[]);
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

            throw new Exception("System Error. A field type is not supported in field value extraction process.");
        }

        public EntityRecord Find(string entityName, Guid id)
        {
            EntityQuery query = new EntityQuery(entityName, "*", EntityQuery.QueryEQ("id", id));
            var results = Find(query);
            return results.FirstOrDefault();
        }

        public List<EntityRecord> Find(EntityQuery query)
        {
            Entity entity = entMan.ReadEntity(query.EntityName).Object;
            var fields = ExtractQueryFieldsMeta(query);
            StringBuilder sql = new StringBuilder();
            StringBuilder sqlJoins = new StringBuilder();
            //StringBuilder sqlGroupBy = new StringBuilder();
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            bool containsRelationalQuery = ContainsRelationalQuery(query.Query);

            //there is a problem when select distinct rows and sort on field not included in select
            //so we add field we sort on and later remove it, only then distinct (containsRelationalQuery) is used
            List<Field> missingSortFields = new List<Field>();
            if (containsRelationalQuery && query.Sort != null && query.Sort.Length > 0)
            {
                foreach (var s in query.Sort)
                {
                    if (s.FieldName.Trim().StartsWith("{"))
                    {
                        //process json
                        dynamic parametrizedSort = ExtractSortFieldJsonValue(s.FieldName, query.OverwriteArgs);
                        if (parametrizedSort != null)
                        {
                            var sortField = entity.Fields.SingleOrDefault(x => x.Name == parametrizedSort.Field );
                            if (sortField == null) //we skip sorf fields not found in entity
                                continue;
                          
                            if(!fields.Any(f=>f.Id == sortField.Id))
                            {
                                fields.Add(sortField);
                                missingSortFields.Add(sortField);
                            }

                        }
                    }
                    else
                    {
                        var sortField = entity.Fields.SingleOrDefault(x => x.Name == s.FieldName);
                        if (sortField == null) //we skip sorf fields not found in entity
                            continue;

                        if (!fields.Any(f => f.Id == sortField.Id))
                        {
                            fields.Add(sortField);
                            missingSortFields.Add(sortField);
                        }
                    }
                }
            }

            bool noSelectRelations = !fields.Any(field => field is RelationFieldMeta);
            if (noSelectRelations)
            {
                #region no relations 

                var tableName = GetTableNameForEntity(entity);
				string columnNames = String.Join(",", fields.Select(x => x.GetFieldType() == FieldType.GeographyField ? "ST_As" + (x as GeographyField).Format + "(" + tableName + ".\"" + x.Name + "\") AS \"" + x.Name + "\"" : tableName + ".\"" + x.Name + "\""));
								
                if(!containsRelationalQuery)
                    sql.AppendLine("SELECT " + columnNames + " FROM " + tableName);
                else
                    sql.AppendLine("SELECT DISTINCT " + columnNames + " FROM " + tableName);

                if (query.Query != null)
                {
                    string whereSql = string.Empty;
                    string whereJoinSql = string.Empty;

                    GenerateWhereClause(query.Query, entity, ref whereSql, ref whereJoinSql, ref parameters, query.OverwriteArgs);

                    if (whereJoinSql.Length > 0)
                        sql.AppendLine(whereJoinSql);

                    if (whereSql.Length > 0)
                        sql.AppendLine("WHERE " + whereSql);
                }

                //sorting
                if (query.Sort != null && query.Sort.Length > 0)
                {
                    string sortSql = "ORDER BY ";

                    foreach (var s in query.Sort)
                    {
                        if (s.FieldName.Trim().StartsWith("{"))
                        {
                            //process json
                            dynamic parametrizedSort = ExtractSortFieldJsonValue(s.FieldName, query.OverwriteArgs);
                            if (parametrizedSort != null)
                            {
                                var sortField = parametrizedSort.Field;
                                var sortOrder = parametrizedSort.Order;

                                //field not found - skip
                                if (!entity.Fields.Any(x => x.Name == sortField))
                                    continue;

                                sortSql = sortSql + " " + GetTableNameForEntity(entity) + "." + sortField;
                                if (string.IsNullOrEmpty(sortOrder))
                                {
                                    if (s.SortType == QuerySortType.Ascending)
                                        sortSql = sortSql + " ASC,";
                                    else
                                        sortSql = sortSql + " DESC,";
                                }
                                else
                                {
                                    if (sortOrder == "asc")
                                        sortSql = sortSql + " ASC,";
                                    else
                                        sortSql = sortSql + " DESC,";
                                }

                            }
                        }
                        else
                        {
                            sortSql = sortSql + " " + tableName + ".\"" + s.FieldName + "\"";
                            if (s.SortType == QuerySortType.Ascending)
                                sortSql = sortSql + " ASC,";
                            else
                                sortSql = sortSql + " DESC,";
                        }
                    }

                    sortSql = sortSql.Remove(sortSql.Length - 1, 1);
                    if (sortSql.Trim() != "ORDER BY")
                        sql.AppendLine(sortSql);
                }

				//paging 
				if (query.Limit != null || query.Skip != null)
				{
					string pagingSql = "LIMIT ";
					if (query.Limit.HasValue && query.Limit != 0)
						pagingSql = pagingSql + query.Limit + " ";
					else
						pagingSql = pagingSql + "ALL ";

					if (query.Skip.HasValue)
						pagingSql = pagingSql + " OFFSET " + query.Skip;

					sql.AppendLine(pagingSql);
				}

				using (var conn = DbContext.Current.CreateConnection())
                {
                    List<EntityRecord> result = new List<EntityRecord>();
                    NpgsqlCommand command = conn.CreateCommand(sql.ToString());
                    command.Parameters.AddRange(parameters.ToArray());
                    using (var reader = command.ExecuteReader())
                    {
                        try
                        {
                            //remove not needed fields
                            if (missingSortFields.Any())
                            {
                                foreach (var sf in missingSortFields)
                                {
                                    var selectedField = fields.Single(f => f.Id == sf.Id);
                                    fields.Remove(selectedField);
                                }
                            }

                            int fieldcount = fields.Count;
                            while (reader.Read())
                            {
                                EntityRecord record = new EntityRecord();
                                for (int index = 0; index < fieldcount; index++)
                                {
                                    string fieldName = reader.GetName(index);
                                    Field field = fields.Single(x => x.Name == fieldName);
                                    record[fieldName] = reader[index] == DBNull.Value ? null : ExtractFieldValue(reader[index], field); ;
                                }

                                result.Add(record);
                            }
                        }
                        finally
                        {
                            reader.Close();
                        }

                        return result;
                    }
                }

                #endregion
            }
            else
            {
                #region relational 

                sql.AppendLine(BEGIN_OUTER_SELECT);

                if (!containsRelationalQuery)
                    sql.AppendLine(BEGIN_SELECT);
                else
                    sql.AppendLine(BEGIN_SELECT_DISTINCT);

                foreach (var field in fields)
                {
                    if (!(field is RelationFieldMeta))
                    {
                        sql.AppendLine(string.Format(REGULAR_FIELD_SELECT, field.Name, GetTableNameForEntity(entity.Name)));
                        //sqlGroupBy.Append(GetTableNameForEntity(entity) + "." + field.Name + ",");
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
                                    relationField.Name,
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
                                    relationField.Name,
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
                                        relationField.Name,
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
                                        relationField.Name,
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
                                        relationField.Name,
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
                                        relationField.Name,
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
                                        relationField.Name,
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
                                        relationField.Name,
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
                                        relationField.Name,
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
                                        relationField.Name,
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

                if (query.Query != null)
                {
                    string whereSql = string.Empty;
                    string whereJoinSql = string.Empty;

                    GenerateWhereClause(query.Query, entity, ref whereSql, ref whereJoinSql, ref parameters, query.OverwriteArgs);

                    if (whereJoinSql.Length > 0)
                        sql.AppendLine(whereJoinSql);

                    if (whereSql.Length > 0)
                        sql.AppendLine("WHERE " + whereSql);
                }

                //sorting
                if (query.Sort != null && query.Sort.Length > 0)
                {
                    string sortSql = "ORDER BY ";

                    foreach (var s in query.Sort)
                    {
                        if (s.FieldName.Trim().StartsWith("{"))
                        {
                            //process json
                            dynamic parametrizedSort = ExtractSortFieldJsonValue(s.FieldName, query.OverwriteArgs);
                            if (parametrizedSort != null)
                            {
                                var sortField = parametrizedSort.Field;
                                var sortOrder = parametrizedSort.Order;

                                //field not found - skip
                                if (!entity.Fields.Any(x => x.Name == sortField))
                                    continue;

                                sortSql = sortSql + " " + GetTableNameForEntity(entity) + "." + sortField;
                                if (sortOrder == null)
                                {
                                    if (s.SortType == QuerySortType.Ascending)
                                        sortSql = sortSql + " ASC,";
                                    else
                                        sortSql = sortSql + " DESC,";
                                }
                                else
                                {
                                    if (sortOrder == "asc")
                                        sortSql = sortSql + " ASC,";
                                    else
                                        sortSql = sortSql + " DESC,";
                                }

                            }
                        }
                        else
                        {
                            sortSql = sortSql + " " + GetTableNameForEntity(entity) + "." + s.FieldName;
                            if (s.SortType == QuerySortType.Ascending)
                                sortSql = sortSql + " ASC,";
                            else
                                sortSql = sortSql + " DESC,";
                        }
                    }

                    sortSql = sortSql.Remove(sortSql.Length - 1, 1);
                    if (sortSql.Trim() != "ORDER BY")
                        sql.AppendLine(sortSql);
                }

                //paging 
                if ((query.Limit != 0 && query.Limit != null) || query.Skip != null)
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
                    command.CommandTimeout = 600;
                    command.Parameters.AddRange(parameters.ToArray());
                    new NpgsqlDataAdapter(command).Fill(dt);
                }

                List<EntityRecord> result = new List<EntityRecord>();

                if( missingSortFields.Any() )
                {
                    foreach( var sf in missingSortFields )
                    {
                        var selectedField = fields.Single(f => f.Id == sf.Id);
                        fields.Remove(selectedField);
                    }
                }

                foreach (DataRow dr in dt.Rows)
                {
                    var jObj = JObject.Parse((string)dr[0]);
                    result.Add(ConvertJObjectToEntityRecord(jObj, fields));
                }

                return result;

                #endregion
            }
        }

        private void GenerateWhereClause(QueryObject query, Entity entity, ref string sql, ref string joinSql, ref List<NpgsqlParameter> parameters,
                                        List<KeyValuePair<string, string>> overwriteArgs = null)
        {

			if (query == null)
				return;

            Field field = null;
            FieldType fieldType = FieldType.GuidField;
            string paramName = null;
            string completeFieldName = null;
            if (!string.IsNullOrWhiteSpace(query.FieldName))
            {
                if (!query.FieldName.Contains(RELATION_NAME_RESULT_SEPARATOR))
                {
                    field = entity.Fields.SingleOrDefault(x => x.Name == query.FieldName);
					if(field == null) {
						throw new Exception("Queried field '" + query.FieldName + "' does not exist");
					}
                    fieldType = field.GetFieldType();
                    string entityTablePrefix = GetTableNameForEntity(entity) + ".";
                    completeFieldName = entityTablePrefix + query.FieldName;
                    paramName = "@" + query.FieldName + "_" + Guid.NewGuid().ToString().Replace("-", "");

                    bool skipClause;
                    var value = ExtractQueryFieldValue(query.FieldValue, field, overwriteArgs, out skipClause) ?? DBNull.Value;
                    if (skipClause)
                        return;
					query.FieldValue = value;
                    parameters.Add(new NpgsqlParameter(paramName, value));
                }
                else
                {
                    var relationData = query.FieldName.Split(RELATION_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                    if (relationData.Count > 2)
                        throw new Exception(string.Format("The specified query filter field '{0}' is incorrect. Only first level relation can be specified.", query.FieldName));

                    string relationName = relationData[0];
                    string relationFieldName = relationData[1];
                    string direction = "origin-target";

                    if (string.IsNullOrWhiteSpace(relationName) || relationName == "$" || relationName == "$$")
                        throw new Exception(string.Format("Invalid relation '{0}'. The relation name is not specified.", query.FieldName));
                    else if (!relationName.StartsWith("$"))
                        throw new Exception(string.Format("Invalid relation '{0}'. The relation name is not correct.", query.FieldName));
                    else
                        relationName = relationName.Substring(1);

                    //check for target priority mark $$
                    if (relationName.StartsWith("$"))
                    {
                        direction = "target-origin";
                        relationName = relationName.Substring(1);
                    }

                    if (string.IsNullOrWhiteSpace(relationFieldName))
                        throw new Exception(string.Format("Invalid query result field '{0}'. The relation field name is not specified.", query.FieldName));


                    RelationFieldMeta relationFieldMeta = new RelationFieldMeta();
                    relationFieldMeta.Name = "$" + relationName;
                    relationFieldMeta.Direction = direction;

                    relationFieldMeta.Relation = relMan.Read().Object.SingleOrDefault(x => x.Name == relationName);
                    if (relationFieldMeta.Relation == null)
                        throw new Exception(string.Format("Invalid relation '{0}'. The relation does not exist.", query.FieldName));

                    if (relationFieldMeta.Relation.TargetEntityId != entity.Id && relationFieldMeta.Relation.OriginEntityId != entity.Id)
                        throw new Exception(string.Format("Invalid relation '{0}'. The relation does relate to queries entity.", query.FieldName));

                    if (relationFieldMeta.Direction != direction)
                        throw new Exception(string.Format("You are trying to query relation '{0}' from origin->target and target->origin direction in single query. This is not allowed.", query.FieldName));

                    //Entity entity = entMan.ReadEntity(query.EntityName).Object;
                    relationFieldMeta.TargetEntity = entMan.ReadEntity(relationFieldMeta.Relation.TargetEntityId).Object;
                    relationFieldMeta.OriginEntity = entMan.ReadEntity(relationFieldMeta.Relation.OriginEntityId).Object;

                    //this should not happen in a perfect (no bugs) world
                    if (relationFieldMeta.OriginEntity == null)
                        throw new Exception(string.Format("Invalid query result field '{0}'. Related (origin)entity is missing.", query.FieldName));
                    if (relationFieldMeta.TargetEntity == null)
                        throw new Exception(string.Format("Invalid query result field '{0}'. Related (target)entity is missing.", query.FieldName));

                    relationFieldMeta.TargetField = relationFieldMeta.TargetEntity.Fields.Single(x => x.Id == relationFieldMeta.Relation.TargetFieldId);
                    relationFieldMeta.OriginField = relationFieldMeta.OriginEntity.Fields.Single(x => x.Id == relationFieldMeta.Relation.OriginFieldId);

                    //this should not happen in a perfect (no bugs) world
                    if (relationFieldMeta.OriginField == null)
                        throw new Exception(string.Format("Invalid query result field '{0}'. Related (origin)field is missing.", query.FieldName));
                    if (relationFieldMeta.TargetField == null)
                        throw new Exception(string.Format("Invalid query result field '{0}'. Related (target)field is missing.", query.FieldName));

                    Entity joinToEntity = null;
                    if (relationFieldMeta.TargetEntity.Id == entity.Id)
                        joinToEntity = relationFieldMeta.OriginEntity;
                    else
                        joinToEntity = relationFieldMeta.TargetEntity;

                    relationFieldMeta.Entity = joinToEntity;

                    var relatedField = joinToEntity.Fields.SingleOrDefault(x => x.Name == relationFieldName);
                    if (relatedField == null)
                        throw new Exception(string.Format("Invalid query result field '{0}'. The relation field does not exist.", query.FieldName));


                    string relationJoinSql = string.Empty;
                    completeFieldName = relationName + "." + relationFieldName;
                    fieldType = relatedField.GetFieldType();
                    paramName = "@" + relationFieldName + "_" + Guid.NewGuid().ToString().Replace("-", "");

                    bool skipClause;
                    var value = ExtractQueryFieldValue(query.FieldValue, relatedField, overwriteArgs, out skipClause) ?? DBNull.Value;
                    if (skipClause)
                        return;

                    parameters.Add(new NpgsqlParameter(paramName, value));

                    if (relationFieldMeta.Relation.RelationType == EntityRelationType.OneToOne)
                    {
                        //when the relation is origin -> target entity
                        if (relationFieldMeta.Relation.OriginEntityId == entity.Id)
                        {
                            relationJoinSql = string.Format(FILTER_JOIN,
                                GetTableNameForEntity(relationFieldMeta.TargetEntity), relationName,
                                relationName, relationFieldMeta.TargetField.Name,
                                GetTableNameForEntity(relationFieldMeta.OriginEntity), relationFieldMeta.OriginField.Name);
                        }
                        else //when the relation is target -> origin, we have to query origin entity
                        {
                            relationJoinSql = string.Format(FILTER_JOIN,
                                   GetTableNameForEntity(relationFieldMeta.OriginEntity), relationName,
                                   relationName, relationFieldMeta.OriginField.Name,
                                   GetTableNameForEntity(relationFieldMeta.TargetEntity), relationFieldMeta.TargetField.Name);
                        }
                    }
                    else if (relationFieldMeta.Relation.RelationType == EntityRelationType.OneToMany)
                    {
                        //when origin and target entity are different, then direction don't matter
                        if (relationFieldMeta.Relation.OriginEntityId != relationFieldMeta.Relation.TargetEntityId)
                        {
                            //when the relation is origin -> target entity
                            if (relationFieldMeta.Relation.OriginEntityId == entity.Id)
                            {
                                relationJoinSql = string.Format(FILTER_JOIN,
                                    GetTableNameForEntity(relationFieldMeta.TargetEntity), relationName,
                                    relationName, relationFieldMeta.TargetField.Name,
                                    GetTableNameForEntity(relationFieldMeta.OriginEntity), relationFieldMeta.OriginField.Name);
                            }
                            else //when the relation is target -> origin, we have to query origin entity
                            {
                                relationJoinSql = string.Format(FILTER_JOIN,
                                    GetTableNameForEntity(relationFieldMeta.OriginEntity), relationName,
                                    relationName, relationFieldMeta.OriginField.Name,
                                    GetTableNameForEntity(relationFieldMeta.TargetEntity), relationFieldMeta.TargetField.Name);
                            }
                        }
                        else //when the origin entity is same as target entity direction matters
                        {
                            if (relationFieldMeta.Direction == "target-origin")
                            {
                                relationJoinSql = string.Format(FILTER_JOIN,
                                   GetTableNameForEntity(relationFieldMeta.OriginEntity), relationName,
                                   relationName, relationFieldMeta.OriginField.Name,
                                   GetTableNameForEntity(relationFieldMeta.TargetEntity), relationFieldMeta.TargetField.Name);
                            }
                            else
                            {
                                relationJoinSql = string.Format(FILTER_JOIN,
                                    GetTableNameForEntity(relationFieldMeta.TargetEntity), relationName,
                                    relationName, relationFieldMeta.TargetField.Name,
                                    GetTableNameForEntity(relationFieldMeta.OriginEntity), relationFieldMeta.OriginField.Name);
                            }
                        }
                    }
                    else if (relationFieldMeta.Relation.RelationType == EntityRelationType.ManyToMany)
                    {
                        string relationTable = "rel_" + relationFieldMeta.Relation.Name;
                        string targetJoinAlias = relationName + "_target";
                        string originJoinAlias = relationName + "_origin";
                        string targetJoinTable = GetTableNameForEntity(relationFieldMeta.TargetEntity);
                        string originJoinTable = GetTableNameForEntity(relationFieldMeta.OriginEntity);

                        //if target is entity we query
                        if (entity.Id == relationFieldMeta.TargetEntity.Id)
                        {
                            relationJoinSql = string.Format(FILTER_JOIN,
                                     /*LEFT OUTER JOIN*/ relationTable, /* */ targetJoinAlias /*ON*/,
                                     targetJoinAlias, /*.*/ "target_id", /* =  */
                                     targetJoinTable, /*.*/ relationFieldMeta.TargetField.Name);

                            relationJoinSql = relationJoinSql + Environment.NewLine + string.Format(FILTER_JOIN,
                                    /*LEFT OUTER JOIN*/ originJoinTable, /* */ originJoinAlias /*ON*/,
                                    targetJoinAlias, /*.*/ "origin_id", /* =  */
									originJoinAlias, /*.*/ relationFieldMeta.OriginField.Name);

                            completeFieldName = originJoinAlias + "." + relationFieldName;
                        }
                        else // if origin is entity we query
                        {
                            relationJoinSql = string.Format(FILTER_JOIN,
                                    /*LEFT OUTER JOIN*/ relationTable, /* */ originJoinAlias /*ON*/,
                                    originJoinAlias, /*.*/ "origin_id", /* =  */
                                    originJoinTable, /*.*/ relationFieldMeta.OriginField.Name);

                            relationJoinSql = relationJoinSql + Environment.NewLine + string.Format(FILTER_JOIN,
                                      /*LEFT OUTER JOIN*/ targetJoinTable, /* */ targetJoinAlias /*ON*/,
                                    originJoinAlias, /*.*/ "target_id", /* =  */
                                    targetJoinAlias, /*.*/ relationFieldMeta.TargetField.Name);

                            completeFieldName = targetJoinAlias + "." + relationFieldName;
                        }
                    }


                    if (!joinSql.Contains(relationJoinSql))
                        joinSql = joinSql + Environment.NewLine + relationJoinSql;



                }

                if (fieldType == FieldType.MultiSelectField &&	
						!(query.QueryType == QueryType.EQ || query.QueryType == QueryType.NOT || query.QueryType == QueryType.CONTAINS ))
                    throw new Exception("The query operator is not supported on field '" + fieldType.ToString() + "'");
			}

            if (sql.Length > 0)
                sql = sql + " AND ";

            switch (query.QueryType)
            {
                case QueryType.EQ:
                    {
						if (query.FieldValue == null || DBNull.Value == query.FieldValue)
							sql = sql + " " + completeFieldName + " IS NULL";
						else
							sql = sql + " " + completeFieldName + "=" + paramName;

						return;
                    }
                case QueryType.NOT:
                    {
						if (query.FieldValue == null || DBNull.Value == query.FieldValue)
							sql = sql + " " + completeFieldName + " IS NOT NULL";
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

						if (fieldType == FieldType.MultiSelectField)
						{
							//parameter here is array of text
							sql = sql + " " + completeFieldName + " @> " + paramName;
						}
						else
						{
							//parameter value here is just text
							parameter.Value = "%" + parameter.Value + "%";
							sql = sql + " " + completeFieldName + " ILIKE " + paramName;
						}

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
				case QueryType.FTS:
					{
						//make text which we search lower case
						var parameter = parameters.Single(x => x.ParameterName == paramName);
						string text = (string)parameter.Value;

						bool singleWord = true;
						if (!string.IsNullOrWhiteSpace(text))
						{
							string analizedText = ftsAnalyzer.ProcessText(text);
							parameter.Value = analizedText;
							singleWord = analizedText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Count() == 1;
						}

						if (singleWord)
						{
							parameter.Value = parameter.Value + ":*"; //search for all lexemes starting with this word 
							if (string.IsNullOrWhiteSpace(query.FtsLanguage))
								sql = sql + " to_tsvector( 'simple', " + completeFieldName + ") @@ to_tsquery( 'simple', " + paramName + ") ";
							else
								sql = sql + " to_tsvector( '" + query.FtsLanguage + "' , " + completeFieldName + ") @@ to_tsquery( '" + query.FtsLanguage + "' ," + paramName + ") ";

						}
						else
						{
							if (string.IsNullOrWhiteSpace(query.FtsLanguage))
								sql = sql + " to_tsvector( 'simple', " + completeFieldName + ") @@ plainto_tsquery( 'simple', " + paramName + ") ";
							else
								sql = sql + " to_tsvector( '" + query.FtsLanguage + "' , " + completeFieldName + ") @@ plainto_tsquery( '" + query.FtsLanguage + "' ," + paramName + ") ";
						}
						return;
					}
				case QueryType.RELATED:
                    {
                        //TODO
                        throw new NotImplementedException();
                    }
                case QueryType.NOTRELATED:
                    {
                        //TODO
                        throw new NotImplementedException();
                    }
                case QueryType.AND:
                    {
                        if (query.SubQueries.Count == 1)
                            GenerateWhereClause(query.SubQueries[0], entity, ref sql, ref joinSql, ref parameters, overwriteArgs);
                        else
                        {
                            string andSql = string.Empty;
                            foreach (var q in query.SubQueries)
                            {
                                string subQuerySql = string.Empty;
                                GenerateWhereClause(q, entity, ref subQuerySql, ref joinSql, ref parameters, overwriteArgs);
                                if (andSql.Length == 0)
                                    andSql = subQuerySql;
                                else if (subQuerySql.Length > 0)
                                    andSql = andSql + " AND " + subQuerySql;
                            }

                            if (andSql.Length > 0)
                                sql = sql + " ( " + andSql + " )";
                        }
                        return;
                    }
                case QueryType.OR:
                    {
                        if (query.SubQueries.Count == 1)
                            GenerateWhereClause(query.SubQueries[0], entity, ref sql, ref joinSql, ref parameters, overwriteArgs);
                        else
                        {
                            string orSql = string.Empty;
                            foreach (var q in query.SubQueries)
                            {
                                string subQuerySql = string.Empty;
                                GenerateWhereClause(q, entity, ref subQuerySql, ref joinSql, ref parameters, overwriteArgs);
                                if (orSql.Length == 0)
                                    orSql = subQuerySql;
                                else if (subQuerySql.Length > 0)
                                    orSql = orSql + " OR " + subQuerySql;
                            }

                            if (orSql.Length > 0)
                                sql = sql + " ( " + orSql + " )";
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
            List<EntityRelation> relations = relMan.Read().Object;
            List<Field> result = new List<Field>();

            //split field string into tokens speparated by FIELDS_SEPARATOR
            List<string> tokens = query.Fields.Split(FIELDS_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            //check the query tokens for widcard symbol and validate it is only that symbol - //UPDATE: allow Wildcard and field names mix. WILL NOT BE DUPLICATED
            //if (tokens.Count > 1 && tokens.Any(x => x == WILDCARD_SYMBOL))
            //	throw new Exception("Invalid query syntax. Wildcard symbol can be used only with no other fields.");

            Entity entity = entMan.ReadEntity(query.EntityName).Object;
            if (entity == null)
                throw new Exception(string.Format("The entity '{0}' does not exists.", query.EntityName));

            //We check for wildcard symbol and if present include all fields of the queried entity 
            bool wildcardSelectionEnabled = tokens.Any(x => x == WILDCARD_SYMBOL);
            if (wildcardSelectionEnabled)
            {
                result.AddRange(entity.Fields);
                //return result; //UPDATE: allow Wildcard and field names mix. WILL NOT BE DUPLICATED
                tokens.Remove(WILDCARD_SYMBOL); //UPDATE: NULL Exception is triggered if not removed.
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

                    //Entity entity = entMan.ReadEntity(query.EntityName).Object;
                    relationFieldMeta.TargetEntity = entMan.ReadEntity(relationFieldMeta.Relation.TargetEntityId).Object;
                    relationFieldMeta.OriginEntity = entMan.ReadEntity(relationFieldMeta.Relation.OriginEntityId).Object;

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


        private object ExtractQueryFieldValue(object value, Field field, List<KeyValuePair<string, string>> overwriteArgs, out bool skipClause)
        {
            skipClause = false;

            if (value == null)
                return null;

            if (value is JToken)
                value = ((JToken)value).ToObject<object>();

            if (value is string && ((string)value).Trim().StartsWith("{"))
            {
                value = ExtractQueryFieldJsonValue((string)value, field, overwriteArgs, out skipClause);
                if (skipClause)
                    return null;
            }

            if (field is AutoNumberField)
            {
                if (value == null)
                    return null;
                if (value is string)
                    return decimal.Parse(value as string);

                return Convert.ToDecimal(value);
            }
            else if (field is CheckboxField)
            {
                if (value == null)
                    return null;
                if (value is string)
                    return bool.Parse(value as string);
                return value as bool?;
            }
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
                {
                    if (string.IsNullOrWhiteSpace(value as string))
                        return null;
                    return DateTime.Parse(value as string);
                }
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
                {
                    if (string.IsNullOrWhiteSpace(value as string))
                        return null;
                    return DateTime.Parse(value as string);
                }

                return value as DateTime?;
            }
            else if (field is EmailField)
                return value as string;
            else if (field is FileField)
                return value as string;
            else if (field is ImageField)
                return value as string;
            else if (field is HtmlField)
                return value as string;
            else if (field is MultiLineTextField)
                return value as string;
			else if (field is GeographyField)
				return value as string;
			else if (field is MultiSelectField)
            {
				if (value == null)
					return null;
				else if (value is JArray)
					return ((JArray)value).Select(x => ((JToken)x).Value<string>()).ToList<string>();
				else if (value is List<object>)
					return ((List<object>)value).Select(x => ((object)x).ToString()).ToList<string>();
				else if (value is string[])
					return new List<string>(value as string[]);
				else if (value is string)
					return new List<string>(((string)value).Split(',', StringSplitOptions.RemoveEmptyEntries));
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
                if (((PasswordField)field).Encrypted == true)
                {
                    if (string.IsNullOrWhiteSpace(value as string))
                        return null;

                    return PasswordUtil.GetMd5Hash(value as string);
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

				if( value is DBNull)
					return (Guid?)null;

				throw new Exception("Invalid Guid field value.");
            }
            else if (field is SelectField)
                return value as string;
            else if (field is TextField)
                return value as string;
            else if (field is UrlField)
                return value as string;


            throw new Exception("System Error. A field type is not supported in field value extraction process.");
        }


        private object ExtractQueryFieldJsonValue(string value, Field field, List<KeyValuePair<string, string>> overwriteArgs, out bool skipClause)
        {
            skipClause = false;
            JObject jObj = null;
            try
            {
                jObj = JObject.Parse(value);
            }
            catch
            {
                throw new Exception("Invalid query agrument json.");
            }


            JToken nameToken;
            if (!jObj.TryGetValue("name", out nameToken))
                throw new Exception("Invalid query agrument json. Missing name.");

            JToken optionToken;
            if (!jObj.TryGetValue("option", out optionToken))
                throw new Exception("Invalid query agrument json. Missing option.");

            JToken defaultToken;
            if (!jObj.TryGetValue("default", out defaultToken))
                throw new Exception("Invalid query agrument json. Missing default.");

            JToken settingsToken;
            if (!jObj.TryGetValue("settings", out settingsToken))
                throw new Exception("Invalid query agrument json. Missing settings.");

            if (nameToken.ToString().ToLowerInvariant() == "current_user")
            {
                //currently we have only id option so ignore options check for now
                ErpUser currentUser = SecurityContext.CurrentUser;
                if (currentUser != null)
                    return currentUser.Id;

                if (string.IsNullOrWhiteSpace(defaultToken.ToString()))
                    return null;

                return new Guid(defaultToken.ToString());
            }
            else if (nameToken.ToString().ToLowerInvariant() == "current_date")
            {
                DateTime currentDate = DateTime.UtcNow;

                if (optionToken.ToString().ToLowerInvariant() == "date")
                {
                    currentDate = currentDate.Date;
                }
                else if (optionToken.ToString().ToLowerInvariant() == "datetime")
                {
                    //already initialized, do nothing
                }
                else
                    throw new Exception("Not supported json query option:" + optionToken.ToString().ToLowerInvariant());

                int yearOffset = 0;
                int monthOffset = 0;
                int dayOffset = 0;
                int hourOffset = 0;
                int minuteOffset = 0;

                if (settingsToken.Type == JTokenType.Object)
                {
                    foreach (JProperty child in settingsToken.Children<JProperty>())
                    {
                        //skip null properties
                        if (child.Value.Type == JTokenType.Null)
                            continue;

                        switch (child.Name)
                        {
                            case "year":
                                Int32.TryParse(child.Value.ToString(), out yearOffset);
                                break;
                            case "month":
                                Int32.TryParse(child.Value.ToString(), out monthOffset);
                                break;
                            case "day":
                                Int32.TryParse(child.Value.ToString(), out dayOffset);
                                break;
                            case "hour":
                                Int32.TryParse(child.Value.ToString(), out hourOffset);
                                break;
                            case "minute":
                                Int32.TryParse(child.Value.ToString(), out minuteOffset);
                                break;
                        }
                    }
                }

                return currentDate.AddYears(yearOffset).AddMonths(monthOffset).AddDays(dayOffset).AddHours(hourOffset).AddMonths(minuteOffset);
            }
            else if (nameToken.ToString().ToLowerInvariant() == "url_query")
            {
                if (optionToken.Type == JTokenType.Null)
                    throw new Exception("Url query key not specified in json.");

                var queryParameterKey = optionToken.ToString().ToLowerInvariant();
                if (overwriteArgs != null && overwriteArgs.Any(x => x.Key.ToLowerInvariant() == queryParameterKey))
                {
                    KeyValuePair<string, string> pair = overwriteArgs.Single(x => x.Key.ToLowerInvariant() == queryParameterKey);
                    return pair.Value;
                }

                if (defaultToken.Type != JTokenType.Null)
                    return defaultToken.ToString();

                //if no query parameter and default value is null, then skip this query clause
                skipClause = true;
                return null;
            }
            else
                throw new Exception("Not supported name '" + nameToken.ToString() + "' in query json clause");

        }

        private dynamic ExtractSortFieldJsonValue(string value, List<KeyValuePair<string, string>> overwriteArgs)
        {
            JObject jObj = null;
            try
            {
                jObj = JObject.Parse(value);
            }
            catch
            {
                throw new Exception("Invalid query agrument json.");
            }


            JToken nameToken;
            if (!jObj.TryGetValue("name", out nameToken))
                throw new Exception("Invalid query agrument json. Missing name.");

            JToken optionToken;
            if (!jObj.TryGetValue("option", out optionToken))
                throw new Exception("Invalid query agrument json. Missing option.");

            JToken defaultToken;
            if (!jObj.TryGetValue("default", out defaultToken))
                throw new Exception("Invalid query agrument json. Missing default.");

            JToken settingsToken;
            if (!jObj.TryGetValue("settings", out settingsToken))
                throw new Exception("Invalid query agrument json. Missing settings.");

            if (nameToken.ToString().ToLowerInvariant() != "url_sort")
                throw new Exception("Not supported name '" + nameToken.ToString() + "' in sort json definition.");

            if (optionToken.Type == JTokenType.Null)
                throw new Exception("Url sort key not specified in json.");

            string sortField = string.Empty;
            string sortOrder = string.Empty;

            var sortParameterKey = optionToken.ToString().ToLowerInvariant();
            if (overwriteArgs != null && overwriteArgs.Any(x => x.Key.ToLowerInvariant() == sortParameterKey))
            {
                KeyValuePair<string, string> pair = overwriteArgs.Single(x => x.Key.ToLowerInvariant() == sortParameterKey);
                sortField = pair.Value;
            }
            else if (defaultToken.Type != JTokenType.Null)
                sortField = defaultToken.ToString();

            if (settingsToken.Type == JTokenType.Object)
            {
                var orderProperty = settingsToken.Children<JProperty>().SingleOrDefault(x => x.Name == "order");
                if (orderProperty != null && orderProperty.Value.Type != JTokenType.Null)
                {
                    var sortOrderParameterKey = orderProperty.Value.ToString().ToLowerInvariant();
                    if (overwriteArgs != null && overwriteArgs.Any(x => x.Key.ToLowerInvariant() == sortOrderParameterKey))
                    {
                        KeyValuePair<string, string> pair = overwriteArgs.Single(x => x.Key.ToLowerInvariant() == sortOrderParameterKey);
                        sortOrder = (pair.Value ?? "asc").Trim().ToLowerInvariant();
                        if (!(sortOrder == "asc" || sortOrder == "desc"))
                            sortOrder = null;
                    }
                }
            }

			if (string.IsNullOrWhiteSpace(sortField))
				return null;
			else
				sortField = sortField.Trim();

			if (string.IsNullOrWhiteSpace(sortOrder))
				sortOrder = sortOrder.Trim();

			return new { Field = sortField, Order = sortOrder };
        }
    }
}

