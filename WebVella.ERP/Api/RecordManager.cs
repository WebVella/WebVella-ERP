using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Utilities;
using Newtonsoft.Json.Linq;
using System.Net;
using WebVella.ERP.Database;

namespace WebVella.ERP.Api
{
	public class RecordManager
	{
		private List<Entity> entityCache;
		private EntityManager entityManager;
		private EntityRelationManager entityRelationManager;
		private DbRelationRepository relationRepository;
		private List<EntityRelation> relations = null;
		private bool ignoreSecurity = false;

		public RecordManager() : this(false)
		{
		}

		internal RecordManager(bool ignoreSecurity = false)
		{
			entityCache = new List<Entity>();
			entityManager = new EntityManager();
			entityRelationManager = new EntityRelationManager();
			relationRepository = DbContext.Current.RelationRepository;
			this.ignoreSecurity = ignoreSecurity;
		}

		public QueryResponse CreateRelationManyToManyRecord(Guid relationId, Guid originValue, Guid targetValue)
		{
			QueryResponse response = new QueryResponse();
			response.Object = null;
			response.Success = true;
			response.Timestamp = DateTime.UtcNow;

			try
			{
				var relation = relationRepository.Read(relationId);

				if (relation == null)
					response.Errors.Add(new ErrorModel { Message = "Relation does not exists." });

				if (response.Errors.Count > 0)
				{
					response.Object = null;
					response.Success = false;
					response.Timestamp = DateTime.UtcNow;
					return response;
				}

				relationRepository.CreateManyToManyRecord(relationId, originValue, targetValue);
				return response;
			}
			catch (Exception e)
			{
				response.Success = false;
				response.Object = null;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The entity relation record was not created. An internal error occurred!";
#endif
				return response;
			}
		}

		public QueryResponse RemoveRelationManyToManyRecord(Guid relationId, Guid originValue, Guid targetValue)
		{
			QueryResponse response = new QueryResponse();
			response.Object = null;
			response.Success = true;
			response.Timestamp = DateTime.UtcNow;

			try
			{
				var relation = relationRepository.Read(relationId);

				if (relation == null)
					response.Errors.Add(new ErrorModel { Message = "Relation does not exists." });

				if (response.Errors.Count > 0)
				{
					response.Object = null;
					response.Success = false;
					response.Timestamp = DateTime.UtcNow;
					return response;
				}

				relationRepository.DeleteManyToManyRecord(relationId, originValue, targetValue);
				return response;
			}
			catch (Exception e)
			{
				response.Success = false;
				response.Object = null;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The entity relation record was not created. An internal error occurred!";
#endif
				return response;
			}
		}

		public QueryResponse CreateRecord(string entityName, EntityRecord record, bool skipRecordReturn = false )
		{
			if (string.IsNullOrWhiteSpace(entityName))
			{
				QueryResponse response = new QueryResponse
				{
					Success = false,
					Object = null,
					Timestamp = DateTime.UtcNow
				};
				response.Errors.Add(new ErrorModel { Message = "Invalid entity name." });
				return response;
			}

			Entity entity = GetEntity(entityName);
			if (entity == null)
			{
				QueryResponse response = new QueryResponse
				{
					Success = false,
					Object = null,
					Timestamp = DateTime.UtcNow
				};
				response.Errors.Add(new ErrorModel { Message = "Entity cannot be found." });
				return response;
			}

			return CreateRecord(entity, record, skipRecordReturn);
		}

		public QueryResponse CreateRecord(Guid entityId, EntityRecord record, bool skipRecordReturn = false)
		{
			Entity entity = GetEntity(entityId);
			if (entity == null)
			{
				QueryResponse response = new QueryResponse
				{
					Success = false,
					Object = null,
					Timestamp = DateTime.UtcNow
				};
				response.Errors.Add(new ErrorModel { Message = "Entity cannot be found." });
				return response;
			}

			return CreateRecord(entity, record, skipRecordReturn);
		}

		public QueryResponse CreateRecord(Entity entity, EntityRecord record, bool skipRecordReturn = false)
		{

			QueryResponse response = new QueryResponse();
			response.Object = null;
			response.Success = true;
			response.Timestamp = DateTime.UtcNow;
			var recRepo = DbContext.Current.RecordRepository;

			try
			{
				if (entity == null)
					response.Errors.Add(new ErrorModel { Message = "Invalid entity name." });

				if (record == null)
					response.Errors.Add(new ErrorModel { Message = "Invalid record. Cannot be null." });

				if (response.Errors.Count > 0)
				{
					response.Object = null;
					response.Success = false;
					response.Timestamp = DateTime.UtcNow;
					return response;
				}

				if (!ignoreSecurity)
				{
					bool hasPermisstion = SecurityContext.HasEntityPermission(EntityPermission.Create, entity);
					if (!hasPermisstion)
					{
						response.StatusCode = HttpStatusCode.Forbidden;
						response.Success = false;
						response.Message = "Trying to create record in entity '" + entity.Name + "' with no create access.";
						response.Errors.Add(new ErrorModel { Message = "Access denied." });
						return response;
					}
				}

				SetRecordServiceInformation(record, true, ignoreSecurity);

				List<KeyValuePair<string, object>> storageRecordData = new List<KeyValuePair<string, object>>();

				var recordFields = record.GetProperties();
				foreach (var field in entity.Fields)
				{
					var pair = recordFields.SingleOrDefault(x => x.Key == field.Name);
					try
					{
						//autonumber field is handled at database level, so we don't process data here
						if (!(field is AutoNumberField))
							storageRecordData.Add(new KeyValuePair<string, object>(field.Name, ExtractFieldValue(pair, field, true)));
					}
					catch (Exception ex)
					{
						if (pair.Key == null)
							throw new Exception("Error during processing value for field: '" + field.Name + "'. No value is specified.");
						else
							throw new Exception("Error during processing value for field: '" + field.Name + "'. Invalid value: '" + pair.Value + "'", ex);
					}
				}



				Guid recordId = Guid.Empty;
				if (!record.Properties.ContainsKey("id"))
				{
					recordId = Guid.NewGuid();
					storageRecordData.Add(new KeyValuePair<string, object>("id", recordId));
				}

				//fixes issue with ID comming from webapi request 
				if (record["id"] is string)
					recordId = new Guid(record["id"] as string);
				else if (record["id"] is Guid)
					recordId = (Guid)record["id"];
				else
					throw new Exception("Invalid record id");

				if (recordId == Guid.Empty)
					throw new Exception("Guid.Empty value cannot be used as valid value for record id.");


				recRepo.Create(entity.Name, storageRecordData);

				if( skipRecordReturn )
				{
					response.Object = null;
					response.Success = true;
					response.Message = "Record was created successfully";
					return response;
				}

				var query = EntityQuery.QueryEQ("id", recordId);
				var entityQuery = new EntityQuery(entity.Name, "*", query);

				// when user create record, it is get returned ignoring create permissions
				bool oldIgnoreSecurity = ignoreSecurity;
				response = Find(entityQuery);
				ignoreSecurity = oldIgnoreSecurity;

				if (response.Object != null && response.Object.Data != null && response.Object.Data.Count > 0)
					response.Message = "Record was created successfully";
				else
				{
					response.Success = false;
					response.Message = "Record was not created successfully";
				}

				return response;
			}
			catch (Exception e)
			{
				response.Success = false;
				response.Object = null;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The entity record was not created. An internal error occurred!";
#endif
				return response;
			}

		}

		public QueryResponse UpdateRecord(string entityName, EntityRecord record, bool skipRecordReturn = false)
		{
			if (string.IsNullOrWhiteSpace(entityName))
			{
				QueryResponse response = new QueryResponse
				{
					Success = false,
					Object = null,
					Timestamp = DateTime.UtcNow
				};
				response.Errors.Add(new ErrorModel { Message = "Invalid entity name." });
				return response;
			}

			Entity entity = GetEntity(entityName);
			if (entity == null)
			{
				QueryResponse response = new QueryResponse
				{
					Success = false,
					Object = null,
					Timestamp = DateTime.UtcNow
				};
				response.Errors.Add(new ErrorModel { Message = "Entity cannot be found." });
				return response;
			}

			return UpdateRecord(entity, record, skipRecordReturn);
		}

		public QueryResponse UpdateRecord(Guid entityId, EntityRecord record, bool skipRecordReturn = false)
		{
			Entity entity = GetEntity(entityId);
			if (entity == null)
			{
				QueryResponse response = new QueryResponse
				{
					Success = false,
					Object = null,
					Timestamp = DateTime.UtcNow
				};
				response.Errors.Add(new ErrorModel { Message = "Entity cannot be found." });
				return response;
			}

			return UpdateRecord(entity, record, skipRecordReturn );
		}

		public QueryResponse UpdateRecord(Entity entity, EntityRecord record, bool skipRecordReturn = false)
		{

			QueryResponse response = new QueryResponse();
			response.Object = null;
			response.Success = true;
			response.Timestamp = DateTime.UtcNow;

			try
			{
				if (entity == null)
					response.Errors.Add(new ErrorModel { Message = "Invalid entity name." });

				if (record == null)
					response.Errors.Add(new ErrorModel { Message = "Invalid record. Cannot be null." });
				else if (!record.Properties.ContainsKey("id"))
					response.Errors.Add(new ErrorModel { Message = "Invalid record. Missing ID field." });

				if (response.Errors.Count > 0)
				{
					response.Object = null;
					response.Success = false;
					response.Timestamp = DateTime.UtcNow;
					return response;
				}


				if (!ignoreSecurity)
				{
					bool hasPermisstion = SecurityContext.HasEntityPermission(EntityPermission.Update, entity);
					if (!hasPermisstion)
					{
						response.StatusCode = HttpStatusCode.Forbidden;
						response.Success = false;
						response.Message = "Trying to update record in entity '" + entity.Name + "'  with no update access.";
						response.Errors.Add(new ErrorModel { Message = "Access denied." });
						return response;
					}
				}

				SetRecordServiceInformation(record, false, ignoreSecurity);

				List<KeyValuePair<string, object>> storageRecordData = new List<KeyValuePair<string, object>>();

				var recordFields = record.GetProperties();
				foreach (var field in entity.Fields)
				{
					var pair = recordFields.SingleOrDefault(x => x.Key == field.Name);
					try
					{
						if (pair.Key == null)
							continue;

						if (field is PasswordField && pair.Value == null)
							continue;



						storageRecordData.Add(new KeyValuePair<string, object>(field.Name, ExtractFieldValue(pair, field, true)));
					}
					catch (Exception ex)
					{
						if (pair.Key == null)
							throw new Exception("Error during processing value for field: '" + field.Name + "'. No value is specified.");
						else
							throw new Exception("Error during processing value for field: '" + field.Name + "'. Invalid value: '" + pair.Value + "'", ex);
					}
				}

				var recRepo = DbContext.Current.RecordRepository;
				recRepo.Update(entity.Name, storageRecordData);

				if( skipRecordReturn )
				{
					response.Object = null;
					response.Success = true;
					response.Message = "Record was updated successfully";
					return response;
				}

				//fixes issue with ID comming from webapi request 
				Guid recordId = Guid.Empty;
				if (record["id"] is string)
					recordId = new Guid(record["id"] as string);
				else if (record["id"] is Guid)
					recordId = (Guid)record["id"];
				else
					throw new Exception("Invalid record id");

				var query = EntityQuery.QueryEQ("id", recordId);
				var entityQuery = new EntityQuery(entity.Name, "*", query);

				response = Find(entityQuery);
				if (response.Object != null && response.Object.Data.Count > 0)
					response.Message = "Record was updated successfully";
				else
				{
					response.Success = false;
					response.Message = "Record was not updated successfully";
				}

				return response;
			}
			catch (Exception e)
			{
				response.Success = false;
				response.Object = null;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The entity record was not update. An internal error occurred!";
#endif
				return response;
			}

		}

		public QueryResponse DeleteRecord(string entityName, Guid id)
		{
			if (string.IsNullOrWhiteSpace(entityName))
			{
				QueryResponse response = new QueryResponse
				{
					Success = false,
					Object = null,
					Timestamp = DateTime.UtcNow
				};
				response.Errors.Add(new ErrorModel { Message = "Invalid entity name." });
				return response;
			}

			Entity entity = GetEntity(entityName);
			if (entity == null)
			{
				QueryResponse response = new QueryResponse
				{
					Success = false,
					Object = null,
					Timestamp = DateTime.UtcNow
				};
				response.Errors.Add(new ErrorModel { Message = "Entity cannot be found." });
				return response;
			}

			return DeleteRecord(entity, id);
		}

		public QueryResponse DeleteRecord(Guid entityId, Guid id)
		{
			Entity entity = GetEntity(entityId);
			if (entity == null)
			{
				QueryResponse response = new QueryResponse
				{
					Success = false,
					Object = null,
					Timestamp = DateTime.UtcNow
				};
				response.Errors.Add(new ErrorModel { Message = "Entity cannot be found." });
				return response;
			}

			return DeleteRecord(entity, id);
		}

		public QueryResponse DeleteRecord(Entity entity, Guid id)
		{

			QueryResponse response = new QueryResponse();
			response.Object = null;
			response.Success = true;
			response.Timestamp = DateTime.UtcNow;

			try
			{
				if (entity == null)
				{
					response.Errors.Add(new ErrorModel { Message = "Invalid entity name." });
					response.Success = false;
					return response;
				}


				if (!ignoreSecurity)
				{
					bool hasPermisstion = SecurityContext.HasEntityPermission(EntityPermission.Delete, entity);
					if (!hasPermisstion)
					{
						response.StatusCode = HttpStatusCode.Forbidden;
						response.Success = false;
						response.Message = "Trying to delete record in entity '" + entity.Name + "' with no delete access.";
						response.Errors.Add(new ErrorModel { Message = "Access denied." });
						return response;
					}
				}

				List<KeyValuePair<string, object>> storageRecordData = new List<KeyValuePair<string, object>>();

				var query = EntityQuery.QueryEQ("id", id);
				var entityQuery = new EntityQuery(entity.Name, "*", query);

				response = Find(entityQuery);
				if (response.Object != null && response.Object.Data.Count == 1)
				{
					DbContext.Current.RecordRepository.Delete(entity.Name, id);
				}
				else
				{
					response.Success = false;
					response.Message = "Record was not found.";
					return response;
				}


				return response;
			}
			catch (Exception e)
			{
				response.Success = false;
				response.Object = null;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The entity record was not update. An internal error occurred!";
#endif
				return response;
			}

		}

		public QueryResponse Find(EntityQuery query)
		{
			QueryResponse response = new QueryResponse
			{
				Success = true,
				Message = "The query was successfully executed.",
				Timestamp = DateTime.UtcNow
			};

			try
			{
				var entity = GetEntity(query.EntityName);
				if (entity == null)
				{
					response.Success = false;
					response.Message = string.Format("The query is incorrect. Specified entity '{0}' does not exist.", query.EntityName);
					response.Object = null;
					response.Errors.Add(new ErrorModel { Message = response.Message });
					response.Timestamp = DateTime.UtcNow;
					return response;
				}


				if (!ignoreSecurity)
				{
					bool hasPermisstion = SecurityContext.HasEntityPermission(EntityPermission.Read, entity);
					if (!hasPermisstion)
					{
						response.StatusCode = HttpStatusCode.Forbidden;
						response.Success = false;
						response.Message = "Trying to read records from entity '" + entity.Name + "'  with no read access.";
						response.Errors.Add(new ErrorModel { Message = "Access denied." });
						return response;
					}
				}

				try
				{
					if (query.Query != null)
						ProcessQueryObject(entity, query.Query);
				}
				catch (Exception ex)
				{
					response.Success = false;
					response.Message = "The query is incorrect and cannot be executed.";
					response.Object = null;
					response.Errors.Add(new ErrorModel { Message = ex.Message });
					response.Timestamp = DateTime.UtcNow;
					return response;
				}

				var fields = DbContext.Current.RecordRepository.ExtractQueryFieldsMeta(query);
				var data = DbContext.Current.RecordRepository.Find(query);
				response.Object = new QueryResult { FieldsMeta = fields, Data = data };
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = "The query is incorrect and cannot be executed";
				response.Object = null;
				response.Errors.Add(new ErrorModel { Message = ex.Message });
				response.Timestamp = DateTime.UtcNow;
				return response;
			}

			return response;
		}

		public QueryCountResponse Count(EntityQuery query)
		{
			QueryCountResponse response = new QueryCountResponse
			{
				Success = true,
				Message = "The query was successfully executed.",
				Timestamp = DateTime.UtcNow
			};

			try
			{
				var entity = GetEntity(query.EntityName);
				if (entity == null)
				{
					response.Success = false;
					response.Message = string.Format("The query is incorrect. Specified entity '{0}' does not exist.", query.EntityName);
					response.Object = 0;
					response.Errors.Add(new ErrorModel { Message = response.Message });
					response.Timestamp = DateTime.UtcNow;
					return response;
				}

				try
				{
					if (query.Query != null)
						ProcessQueryObject(entity, query.Query);
				}
				catch (Exception ex)
				{
					response.Success = false;
					response.Message = "The query is incorrect and cannot be executed";
					response.Object = 0;
					response.Errors.Add(new ErrorModel { Message = ex.Message });
					response.Timestamp = DateTime.UtcNow;
					return response;
				}

				List<Field> fields = DbContext.Current.RecordRepository.ExtractQueryFieldsMeta(query);
				response.Object = DbContext.Current.RecordRepository.Count(query.EntityName, query.Query);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = "The query is incorrect and cannot be executed";
				response.Object = 0;
				response.Errors.Add(new ErrorModel { Message = ex.Message });
				response.Timestamp = DateTime.UtcNow;
				return response;
			}

			return response;
		}

		private object ExtractFieldValue(KeyValuePair<string, object>? fieldValue, Field field, bool encryptPasswordFields = false)
		{
			if (fieldValue != null && fieldValue.Value.Key != null)
			{
				var pair = fieldValue.Value;
				if(pair.Value == DBNull.Value) {
					pair = new KeyValuePair<string,object>(pair.Key,null);
				}

				if (field is AutoNumberField)
				{
					if (pair.Value == null)
						return null;
					if (pair.Value is string)
						return (int)decimal.Parse(pair.Value as string);

					return Convert.ToDecimal(pair.Value);
				}
				else if (field is CheckboxField)
					return pair.Value as bool?;
				else if (field is CurrencyField)
				{
					if (pair.Value == null)
						return null;
					if (pair.Value is string)
						return decimal.Parse(pair.Value as string);

					return Convert.ToDecimal(pair.Value);
				}
				else if (field is DateField)
				{
					if (pair.Value == null)
						return null;

					DateTime? date = null;
					if (pair.Value is string)
						date = DateTime.Parse(pair.Value as string);
					else
						date = pair.Value as DateTime?;

					if (date != null)
						return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0, DateTimeKind.Utc);
				}
				else if (field is DateTimeField)
				{

					if (pair.Value == null)
						return null;

					if (pair.Value is string)
						return DateTime.Parse(pair.Value as string);

					return pair.Value as DateTime?;
				}
				else if (field is EmailField)
					return pair.Value as string;
				else if (field is FileField)
					//TODO convert file path to url path
					return pair.Value as string;
				else if (field is ImageField)
					//TODO convert image path to url path
					return pair.Value as string;
				else if (field is HtmlField)
					return pair.Value as string;
				else if (field is MultiLineTextField)
					return pair.Value as string;
				else if (field is MultiSelectField)
				{
					if (pair.Value == null)
						return null;
					else if (pair.Value is JArray)
						return ((JArray)pair.Value).Select(x => ((JToken)x).Value<string>()).ToList<string>();
					else if (pair.Value is List<object>)
						return ((List<object>)pair.Value).Select(x => ((object)x).ToString()).ToList<string>();
					else
						return pair.Value as IEnumerable<string>;
				}
				else if (field is NumberField)
				{
					if (pair.Value == null)
						return null;
					if (pair.Value is string)
						return decimal.Parse(pair.Value as string);

					return Convert.ToDecimal(pair.Value);
				}
				else if (field is PasswordField)
				{
					if (encryptPasswordFields)
					{
						if (((PasswordField)field).Encrypted == true)
						{
							if (string.IsNullOrWhiteSpace(pair.Value as string))
								return null;

							return PasswordUtil.GetMd5Hash(pair.Value as string);
						}
					}
					return pair.Value;
				}
				else if (field is PercentField)
				{
					if (pair.Value == null)
						return null;
					if (pair.Value is string)
						return decimal.Parse(pair.Value as string);

					return Convert.ToDecimal(pair.Value);
				}
				else if (field is PhoneField)
					return pair.Value as string;
				else if (field is GuidField)
				{
					if (pair.Value is string)
					{
						if (string.IsNullOrWhiteSpace(pair.Value as string))
							return null;

						return new Guid(pair.Value as string);
					}

					if (pair.Value is Guid)
						return (Guid?)pair.Value;

					if (pair.Value == null)
						return (Guid?)null;

					throw new Exception("Invalid Guid field value.");
				}
				else if (field is SelectField)
					return pair.Value as string;
				else if (field is TextField)
					return pair.Value as string;
				else if (field is UrlField)
					return pair.Value as string;
				else if (field is TreeSelectField)
				{
					if (pair.Value == null)
						return null;
					else if (pair.Value is JArray)
						return ((JArray)pair.Value).Select(x => new Guid(((JToken)x).Value<string>())).ToList<Guid>();
					else if (pair.Value is List<object>)
						return ((List<object>)pair.Value).Select(x => ((Guid)x)).ToList<Guid>();
					else
						return pair.Value as IEnumerable<Guid>;
				}
			}
			else
			{
				return field.GetDefaultValue();
			}

			throw new Exception("System Error. A field type is not supported in field value extraction process.");
		}

		private Entity GetEntity(string entityName)
		{
			var entity = entityCache.SingleOrDefault(x => x.Name == entityName);
			if (entity == null)
			{
				entity = entityManager.ReadEntity(entityName).Object;

				if (entity != null)
					entityCache.Add(entity);
			}

			return entity;
		}

		private Entity GetEntity(Guid entityId)
		{
			var entity = entityCache.SingleOrDefault(x => x.Id == entityId);
			if (entity == null)
			{
				entity = entityManager.ReadEntity(entityId).Object;

				if (entity != null)
					entityCache.Add(entity);
			}

			return entity;
		}

		private List<EntityRelation> GetRelations()
		{
			if (relations == null)
				relations = entityRelationManager.Read().Object;

			if (relations == null)
				return new List<EntityRelation>();

			return relations;
		}

		private void ProcessQueryObject(Entity entity, QueryObject obj)
		{
			if (obj == null)
				return;

			if (obj.QueryType != QueryType.AND && obj.QueryType != QueryType.OR &&
				obj.QueryType != QueryType.RELATED && obj.QueryType != QueryType.NOTRELATED)
			{
				var field = entity.Fields.SingleOrDefault(x => x.Name == obj.FieldName);
				if (!(obj.QueryType == QueryType.RELATED || obj.QueryType == QueryType.NOTRELATED))
				{
					if (field == null)
						throw new Exception(string.Format("There is not entity field '{0}' you try to query by.", obj.FieldName));
				}

				if (field is NumberField || field is AutoNumberField)
				{
					if (obj.FieldValue != null)
						obj.FieldValue = Convert.ToDecimal(obj.FieldValue);
				}
				else if (field is GuidField)
				{
					if (obj.FieldValue != null && obj.FieldValue is string)
					{
						var stringGuid = obj.FieldValue as string;
						if (!string.IsNullOrWhiteSpace(stringGuid))
							obj.FieldValue = new Guid(stringGuid);
						else
							obj.FieldValue = null;
					}
				}
				else if (field is CheckboxField)
				{
					if (obj.FieldValue != null && obj.FieldValue is string)
						obj.FieldValue = bool.Parse(obj.FieldValue as string);
				}
				else if (field is PasswordField && obj.FieldValue != null)
					obj.FieldValue = PasswordUtil.GetMd5Hash(obj.FieldValue as string);
			}

			if (obj.QueryType == QueryType.RELATED || obj.QueryType == QueryType.NOTRELATED)
			{
				var relation = relationRepository.Read(obj.FieldName);
				if (relation == null)
					throw new Exception(string.Format("There is not relation with name '{0}' used in your query.", obj.FieldName));

				if (relation.RelationType != EntityRelationType.ManyToMany)
					throw new Exception(string.Format("Only many to many relations can used in Related and NotRelated query operators.", obj.FieldName));

				var direction = obj.FieldValue as string ?? "origin-target";
				if (relation.OriginEntityId == relation.TargetEntityId)
				{
					if (direction == "target-origin")
						obj.FieldName = $"#{obj.FieldName}_origins";
					else
						obj.FieldName = $"#{obj.FieldName}_targets";

				}
				else
				{
					if (entity.Id == relation.OriginEntityId)
						obj.FieldName = $"#{obj.FieldName}_targets";
					else
						obj.FieldName = $"#{obj.FieldName}_origins";
				}
			}

			if (obj.QueryType == QueryType.AND || obj.QueryType == QueryType.OR)
			{
				if (obj.SubQueries != null && obj.SubQueries.Count > 0)
					foreach (var subObj in obj.SubQueries)
					{
						ProcessQueryObject(entity, subObj);
					}
			}
		}

		private void SetRecordServiceInformation(EntityRecord record, bool newRecord = true, bool ignoreSecurity = false)
		{
			if (record == null)
				return;

			if (newRecord)
			{

				record["created_on"] = DateTime.UtcNow;
				record["last_modified_on"] = DateTime.UtcNow;
				if (SecurityContext.CurrentUser != null)
				{
					record["created_by"] = SecurityContext.CurrentUser.Id;
					record["last_modified_by"] = SecurityContext.CurrentUser.Id;
				}
				else
				{
					//if ignore security is set then do not overwrite already set values
					//needed to set first user
					if (!ignoreSecurity)
					{
						record["created_by"] = null;
						record["last_modified_by"] = null;
					}
				}
			}
			else
			{
				record["last_modified_on"] = DateTime.UtcNow;

				if (SecurityContext.CurrentUser != null)
					record["last_modified_by"] = SecurityContext.CurrentUser.Id;
				else
					record["last_modified_by"] = null;

			}
		}
	}
}