using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Hooks;
using WebVella.Erp.Utilities;

namespace WebVella.Erp.Api
{
	public class RecordManager
	{
		private const char RELATION_SEPARATOR = '.';
		private const char RELATION_NAME_RESULT_SEPARATOR = '$';

		private EntityManager entityManager;
		private EntityRelationManager entityRelationManager;
		private DbRelationRepository relationRepository;
		private List<EntityRelation> relations = null;
		private bool ignoreSecurity = false;
		private bool executeHooks = true;

		private DbContext suppliedContext = null;
		private DbContext CurrentContext
		{
			get
			{
				if (suppliedContext != null)
					return suppliedContext;
				else
					return DbContext.Current;
			}
		}


		public RecordManager(DbContext currentContext = null, bool ignoreSecurity = false, bool executeHooks = true)
		{
			if (currentContext != null)
				suppliedContext = currentContext;
			entityManager = new EntityManager(CurrentContext);
			entityRelationManager = new EntityRelationManager(CurrentContext);
			relationRepository = CurrentContext.RelationRepository;
			this.ignoreSecurity = ignoreSecurity;
			this.executeHooks = executeHooks;
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

				bool hooksExists = RecordHookManager.ContainsAnyHooksForRelation(relation.Name);
				if (hooksExists)
				{
					using (var connection = CurrentContext.CreateConnection())
					{
						try
						{
							connection.BeginTransaction();

							List<ErrorModel> errors = new List<ErrorModel>();
							RecordHookManager.ExecutePreCreateManyToManyRelationHook(relation.Name, originValue, targetValue, errors);
							if (errors.Count > 0)
							{
								connection.RollbackTransaction();
								response.Success = false;
								response.Object = null;
								response.Errors = errors;
								response.Timestamp = DateTime.UtcNow;
								return response;
							}

							relationRepository.CreateManyToManyRecord(relationId, originValue, targetValue);
							RecordHookManager.ExecutePostCreateManyToManyRelationHook(relation.Name, originValue, targetValue);

							connection.CommitTransaction();
							return response;
						}
						catch
						{
							connection.RollbackTransaction();
							throw;
						}
					}
				}
				else
				{
					relationRepository.CreateManyToManyRecord(relationId, originValue, targetValue);
					return response;
				}
			}
			catch (Exception e)
			{
				response.Success = false;
				response.Object = null;
				response.Timestamp = DateTime.UtcNow;

				if (ErpSettings.DevelopmentMode)
					response.Message = e.Message + e.StackTrace;
				else
					response.Message = "The entity relation record was not created. An internal error occurred!";

				return response;
			}
		}

		public QueryResponse RemoveRelationManyToManyRecord(Guid relationId, Guid? originValue, Guid? targetValue)
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


				bool hooksExists = RecordHookManager.ContainsAnyHooksForRelation(relation.Name);
				if (hooksExists)
				{
					using (var connection = CurrentContext.CreateConnection())
					{
						try
						{
							connection.BeginTransaction();

							List<ErrorModel> errors = new List<ErrorModel>();
							RecordHookManager.ExecutePreDeleteManyToManyRelationHook(relation.Name, originValue, targetValue, errors);
							if (errors.Count > 0)
							{
								connection.RollbackTransaction();
								response.Success = false;
								response.Object = null;
								response.Errors = errors;
								response.Timestamp = DateTime.UtcNow;
								return response;
							}

							relationRepository.DeleteManyToManyRecord(relationId, originValue, targetValue);
							RecordHookManager.ExecutePostDeleteManyToManyRelationHook(relation.Name, originValue, targetValue);

							connection.CommitTransaction();
							return response;
						}
						catch
						{
							connection.RollbackTransaction();
							throw;
						}
					}
				}
				else
				{
					relationRepository.DeleteManyToManyRecord(relationId, originValue, targetValue);
					return response;
				}
			}
			catch (Exception e)
			{
				response.Success = false;
				response.Object = null;
				response.Timestamp = DateTime.UtcNow;

				if (ErpSettings.DevelopmentMode)
					response.Message = e.Message + e.StackTrace;
				else
					response.Message = "The entity relation record was not created. An internal error occurred!";

				return response;
			}
		}

		public QueryResponse CreateRecord(string entityName, EntityRecord record)
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

			return CreateRecord(entity, record);
		}

		public QueryResponse CreateRecord(Guid entityId, EntityRecord record)
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

			return CreateRecord(entity, record);
		}

		public QueryResponse CreateRecord(Entity entity, EntityRecord record)
		{

			QueryResponse response = new QueryResponse();
			response.Object = null;
			response.Success = true;
			response.Timestamp = DateTime.UtcNow;
			var recRepo = CurrentContext.RecordRepository;

			using (DbConnection connection = CurrentContext.CreateConnection())
			{
				bool isTransactionActive = false;
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

					bool hooksExists = RecordHookManager.ContainsAnyHooksForEntity(entity.Name);

					if (record.Properties.Any(p => p.Key.StartsWith("$")) || hooksExists)
					{
						connection.BeginTransaction();
						isTransactionActive = true;
					}

					if (hooksExists && executeHooks)
					{
						List<ErrorModel> errors = new List<ErrorModel>();
						RecordHookManager.ExecutePreCreateRecordHooks(entity.Name, record, errors);
						if (errors.Count > 0)
						{
							if (isTransactionActive)
								connection.RollbackTransaction();

							response.Success = false;
							response.Object = null;
							response.Errors = errors;
							response.Timestamp = DateTime.UtcNow;
							return response;
						}
					}

					Guid recordId = Guid.Empty;
					if (!record.Properties.ContainsKey("id"))
						recordId = Guid.NewGuid();
					else
					{
						//fixes issue with ID coming from webapi request 
						if (record["id"] is string)
							recordId = new Guid(record["id"] as string);
						else if (record["id"] is Guid)
							recordId = (Guid)record["id"];
						else
							throw new Exception("Invalid record id");

						if (recordId == Guid.Empty)
							throw new Exception("Guid.Empty value cannot be used as valid value for record id.");
					}


					List<KeyValuePair<string, object>> storageRecordData = new List<KeyValuePair<string, object>>();
					List<dynamic> oneToOneRecordData = new List<dynamic>();
					List<dynamic> oneToManyRecordData = new List<dynamic>();
					List<dynamic> manyToManyRecordData = new List<dynamic>();

					Dictionary<string, EntityRecord> fieldsFromRelationList = new Dictionary<string, EntityRecord>();
					Dictionary<string, EntityRecord> relationFieldMetaList = new Dictionary<string, EntityRecord>();

					var relations = GetRelations();

					foreach (var pair in record.GetProperties())
					{
						try
						{
							if (pair.Key == null)
								continue;

							if (pair.Key.Contains(RELATION_SEPARATOR))
							{
								var relationData = pair.Key.Split(RELATION_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
								if (relationData.Count > 2)
									throw new Exception(string.Format("The specified field name '{0}' is incorrect. Only first level relation can be specified.", pair.Key));

								string relationName = relationData[0];
								string relationFieldName = relationData[1];
								string direction = "origin-target";

								if (string.IsNullOrWhiteSpace(relationName) || relationName == "$" || relationName == "$$")
									throw new Exception(string.Format("Invalid relation '{0}'. The relation name is not specified.", pair.Key));
								else if (!relationName.StartsWith("$"))
									throw new Exception(string.Format("Invalid relation '{0}'. The relation name is not correct.", pair.Key));
								else
									relationName = relationName.Substring(1);

								//check for target priority mark $$
								if (relationName.StartsWith("$"))
								{
									direction = "target-origin";
									relationName = relationName.Substring(1);
								}

								if (string.IsNullOrWhiteSpace(relationFieldName))
									throw new Exception(string.Format("Invalid relation '{0}'. The relation field name is not specified.", pair.Key));

								var relation = relations.SingleOrDefault(x => x.Name == relationName);
								if (relation == null)
									throw new Exception(string.Format("Invalid relation '{0}'. The relation does not exist.", pair.Key));

								if (relation.TargetEntityId != entity.Id && relation.OriginEntityId != entity.Id)
									throw new Exception(string.Format("Invalid relation '{0}'. The relation field belongs to entity that does not relate to current entity.", pair.Key));

								Entity relationEntity = null;
								Field relationField = null;
								Field realtionSearchField;
								Field field = null;

								if (relation.OriginEntityId == relation.TargetEntityId)
								{
									if (direction == "origin-target")
									{
										relationEntity = entity;
										relationField = relationEntity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
										realtionSearchField = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
										field = entity.Fields.FirstOrDefault(f => f.Id == relation.OriginFieldId);
									}
									else
									{
										relationEntity = entity;
										relationField = relationEntity.Fields.FirstOrDefault(f => f.Id == relation.OriginFieldId);
										realtionSearchField = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
										field = entity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
									}
								}
								else if (relation.OriginEntityId == entity.Id)
								{
									//direction doesn't matter
									relationEntity = GetEntity(relation.TargetEntityId);
									relationField = relationEntity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
									realtionSearchField = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
									field = entity.Fields.FirstOrDefault(f => f.Id == relation.OriginFieldId);
								}
								else
								{
									//direction doesn't matter
									relationEntity = GetEntity(relation.OriginEntityId);
									relationField = relationEntity.Fields.FirstOrDefault(f => f.Id == relation.OriginFieldId);
									realtionSearchField = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
									field = entity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
								}

								if (realtionSearchField == null)
									throw new Exception(string.Format("Invalid relation '{0}'. Field does not exist.", pair.Key));

								if (realtionSearchField.GetFieldType() == FieldType.MultiSelectField)
									throw new Exception(string.Format("Invalid relation '{0}'. Fields from Multiselect type can't be used as relation fields.", pair.Key));

								if (relation.RelationType == EntityRelationType.OneToOne &&
									((relation.TargetEntityId == entity.Id && field.Name == "id") || (relation.OriginEntityId == entity.Id && relationField.Name == "id")))
									throw new Exception(string.Format("Invalid relation '{0}'. Can't use relations when relation field is id field.", pair.Key));


								QueryObject filter = null;
								if ((relation.RelationType == EntityRelationType.OneToMany && relation.OriginEntityId == relation.TargetEntityId && direction == "origin-target") ||
									(relation.RelationType == EntityRelationType.OneToMany && relation.OriginEntityId != relation.TargetEntityId && relation.OriginEntityId == entity.Id) ||
									relation.RelationType == EntityRelationType.ManyToMany)
								{
									//expect array of values
									if (!record.Properties.ContainsKey(field.Name) || record[field.Name] == null)
										throw new Exception(string.Format("Invalid relation '{0}'. Relation field does not exist into input record data or its value is null.", pair.Key));

									List<string> values = new List<string>();
									if (pair.Value is JArray)
										values = ((JArray)pair.Value).Select(x => ((JToken)x).Value<string>()).ToList<string>();
									else if (pair.Value is List<Guid>)
										values = ((List<Guid>)pair.Value).Select(x => ((Guid)x).ToString()).ToList<string>();
									else if (pair.Value is List<object>)
										values = ((List<object>)pair.Value).Select(x => ((object)x).ToString()).ToList<string>();
									else if (pair.Value is List<string>)
										values = (List<string>)pair.Value;
									else if (pair.Value != null)
										values.Add(pair.Value.ToString());

									if (values.Count < 1)
										continue;

									List<QueryObject> queries = new List<QueryObject>();
									foreach (var val in values)
									{
										queries.Add(EntityQuery.QueryEQ(realtionSearchField.Name, val));
									}

									filter = EntityQuery.QueryOR(queries.ToArray());
								}
								else if ((relation.RelationType == EntityRelationType.OneToMany && relation.OriginEntityId == relation.TargetEntityId && direction == "target-origin") ||
									(relation.RelationType == EntityRelationType.OneToMany && relation.OriginEntityId != relation.TargetEntityId && relation.OriginEntityId != entity.Id))
								{
									List<string> values = new List<string>();
									if (pair.Value is JArray)
									{
										values = ((JArray)pair.Value).Select(x => ((JToken)x).Value<string>()).ToList<string>();
										if (values.Count > 0)
										{
											var newPair = new KeyValuePair<string, object>(pair.Key, values[0]);
											filter = EntityQuery.QueryEQ(realtionSearchField.Name, ExtractFieldValue(newPair, realtionSearchField, true));
										}
										else
										{
											throw new Exception("Array has not elements");
										}
									}
									else if (pair.Value is List<Guid>)
									{
										values = ((List<Guid>)pair.Value).Select(x => x.ToString()).ToList();
										if (values.Count > 0)
										{
											var newPair = new KeyValuePair<string, object>(pair.Key, values[0]);
											filter = EntityQuery.QueryEQ(realtionSearchField.Name, ExtractFieldValue(newPair, realtionSearchField, true));
										}
										else
										{
											throw new Exception("Array has not elements");
										}
									}
									else
									{
										filter = EntityQuery.QueryEQ(realtionSearchField.Name, ExtractFieldValue(pair, realtionSearchField, true));
									}
								}
								else
								{
									filter = EntityQuery.QueryEQ(realtionSearchField.Name, ExtractFieldValue(pair, realtionSearchField, true));
								}

								EntityRecord relationFieldMeta = new EntityRecord();
								relationFieldMeta["key"] = pair.Key;
								relationFieldMeta["direction"] = direction;
								relationFieldMeta["relationName"] = relationName;
								relationFieldMeta["relationEntity"] = relationEntity;
								relationFieldMeta["relationField"] = relationField;
								relationFieldMeta["realtionSearchField"] = realtionSearchField;
								relationFieldMeta["field"] = field;
								relationFieldMetaList[pair.Key] = relationFieldMeta;

								EntityRecord fieldsFromRelation = new EntityRecord();

								if (fieldsFromRelationList.Any(r => r.Key == relation.Name))
								{
									fieldsFromRelation = fieldsFromRelationList[relationName];
								}
								else
								{
									fieldsFromRelation["queries"] = new List<QueryObject>();
									fieldsFromRelation["direction"] = direction;
									fieldsFromRelation["relationEntityName"] = relationEntity.Name;
								}

								((List<QueryObject>)fieldsFromRelation["queries"]).Add(filter);
								fieldsFromRelationList[relationName] = fieldsFromRelation;
							}
						}
						catch (Exception ex)
						{
							if (pair.Key != null)
								throw new Exception("Error during processing value for field: '" + pair.Key + "'. Invalid value: '" + pair.Value + "'", ex);
						}
					}

					foreach (var fieldsFromRelation in fieldsFromRelationList)
					{
						EntityRecord fieldsFromRelationValue = (EntityRecord)fieldsFromRelation.Value;
						List<QueryObject> queries = (List<QueryObject>)fieldsFromRelationValue["queries"];
						string direction = (string)fieldsFromRelationValue["direction"];
						string relationEntityName = (string)fieldsFromRelationValue["relationEntityName"];
						QueryObject filter = EntityQuery.QueryAND(queries.ToArray());

						var relation = relations.SingleOrDefault(r => r.Name == fieldsFromRelation.Key);

						//get related records
						QueryResponse relatedRecordResponse = Find(new EntityQuery(relationEntityName, "*", filter, null, null, null));

						if (!relatedRecordResponse.Success || relatedRecordResponse.Object.Data.Count < 1)
						{
							throw new Exception(string.Format("Invalid relation '{0}'. The relation record does not exist.", relationEntityName));
						}
						else if (relatedRecordResponse.Object.Data.Count > 1 && ((relation.RelationType == EntityRelationType.OneToMany && relation.OriginEntityId == relation.TargetEntityId && direction == "target-origin") ||
							(relation.RelationType == EntityRelationType.OneToMany && relation.OriginEntityId != relation.TargetEntityId && relation.TargetEntityId == entity.Id) ||
							relation.RelationType == EntityRelationType.OneToOne))
						{
							//there can be no more than 1 records
							throw new Exception(string.Format("Invalid relation '{0}'. There are multiple relation records matching this value.", relationEntityName));
						}

						((EntityRecord)fieldsFromRelationList[fieldsFromRelation.Key])["relatedRecordResponse"] = relatedRecordResponse;
					}
					List<Tuple<Field, string>> fileFields = new List<Tuple<Field, string>>();
					foreach (var pair in record.GetProperties())
					{
						try
						{
							if (pair.Key == null)
								continue;

							if (pair.Key.Contains(RELATION_SEPARATOR))
							{
								EntityRecord relationFieldMeta = relationFieldMetaList.FirstOrDefault(f => f.Key == pair.Key).Value;

								if (relationFieldMeta == null)
									continue;

								string direction = (string)relationFieldMeta["direction"];
								string relationName = (string)relationFieldMeta["relationName"];
								Entity relationEntity = (Entity)relationFieldMeta["relationEntity"];
								Field relationField = (Field)relationFieldMeta["relationField"];
								Field realtionSearchField = (Field)relationFieldMeta["realtionSearchField"];
								Field field = (Field)relationFieldMeta["field"];

								var relation = relations.SingleOrDefault(r => r.Name == relationName);

								QueryResponse relatedRecordResponse = (QueryResponse)((EntityRecord)fieldsFromRelationList[relationName])["relatedRecordResponse"];

								var relatedRecords = relatedRecordResponse.Object.Data;
								List<Guid> relatedRecordValues = new List<Guid>();
								foreach (var relatedRecord in relatedRecords)
								{
									relatedRecordValues.Add((Guid)relatedRecord[relationField.Name]);
								}

								if (relation.RelationType == EntityRelationType.OneToOne &&
									((relation.OriginEntityId == relation.TargetEntityId && direction == "origin-target") || (relation.OriginEntityId != relation.TargetEntityId && relation.OriginEntityId == entity.Id)))
								{
									if (!record.Properties.ContainsKey(field.Name) || record[field.Name] == null)
										throw new Exception(string.Format("Invalid relation '{0}'. Relation field does not exist into input record data or its value is null.", pair.Key));

									var relatedRecord = relatedRecords[0];
									List<KeyValuePair<string, object>> relRecordData = new List<KeyValuePair<string, object>>();
									relRecordData.Add(new KeyValuePair<string, object>("id", relatedRecord["id"]));
									relRecordData.Add(new KeyValuePair<string, object>(relationField.Name, record[field.Name]));

									dynamic ooRelationData = new ExpandoObject();
									ooRelationData.RelationId = relation.Id;
									ooRelationData.RecordData = relRecordData;
									ooRelationData.EntityName = relationEntity.Name;

									oneToOneRecordData.Add(ooRelationData);
								}
								else if (relation.RelationType == EntityRelationType.OneToMany &&
									((relation.OriginEntityId == relation.TargetEntityId && direction == "origin-target") || (relation.OriginEntityId != relation.TargetEntityId && relation.OriginEntityId == entity.Id)))
								{
									if (!record.Properties.ContainsKey(field.Name) || record[field.Name] == null)
										throw new Exception(string.Format("Invalid relation '{0}'. Relation field does not exist into input record data or its value is null.", pair.Key));

									foreach (var data in relatedRecordResponse.Object.Data)
									{
										List<KeyValuePair<string, object>> relRecordData = new List<KeyValuePair<string, object>>();
										relRecordData.Add(new KeyValuePair<string, object>("id", data["id"]));
										relRecordData.Add(new KeyValuePair<string, object>(relationField.Name, record[field.Name]));

										dynamic omRelationData = new ExpandoObject();
										omRelationData.RelationId = relation.Id;
										omRelationData.RecordData = relRecordData;
										omRelationData.EntityName = relationEntity.Name;

										oneToManyRecordData.Add(omRelationData);
									}
								}
								else if (relation.RelationType == EntityRelationType.ManyToMany)
								{
									foreach (Guid relatedRecordIdValue in relatedRecordValues)
									{
										Guid relRecordId = Guid.Empty;
										if (record[field.Name] is string)
											relRecordId = new Guid(record[field.Name] as string);
										else if (record[field.Name] is Guid)
											relRecordId = (Guid)record[field.Name];
										else
											throw new Exception("Invalid record value for field: '" + pair.Key + "'. Invalid value: '" + pair.Value + "'");

										Guid originFieldValue = relRecordId;
										Guid targetFieldValue = relatedRecordIdValue;

										if (relation.TargetEntityId == entity.Id)
										{
											originFieldValue = relatedRecordIdValue;
											targetFieldValue = relRecordId;
										}

										dynamic mmRelationData = new ExpandoObject();
										mmRelationData.RelationId = relation.Id;
										mmRelationData.OriginFieldValue = originFieldValue;
										mmRelationData.TargetFieldValue = targetFieldValue;

										if (!manyToManyRecordData.Any(r => r.RelationId == mmRelationData.RelationId && r.OriginFieldValue == mmRelationData.OriginFieldValue && r.TargetFieldValue == mmRelationData.TargetFieldValue))
											manyToManyRecordData.Add(mmRelationData);
									}
								}
								else
								{
									if (!storageRecordData.Any(r => r.Key == field.Name))
										storageRecordData.Add(new KeyValuePair<string, object>(field.Name, relatedRecordValues[0]));
								}
							}
							else
							{
								//locate the field
								var field = entity.Fields.SingleOrDefault(x => x.Name == pair.Key);

								if (field is AutoNumberField) //Autonumber Value is always autogenerated, this ignored if provided
									continue;

								if (field is FileField || field is ImageField)
								{
									fileFields.Add(new Tuple<Field, string>(field, pair.Value as string));
								}
								else
								{
									if (field.Required && pair.Value == null)
										storageRecordData.Add(new KeyValuePair<string, object>(field.Name, field.GetFieldDefaultValue()));
									else
										storageRecordData.Add(new KeyValuePair<string, object>(field.Name, ExtractFieldValue(pair, field, true)));
								}
							}
						}
						catch (Exception ex)
						{
							if (pair.Key != null)
								throw new Exception("Error during processing value for field: '" + pair.Key + "'. Invalid value: '" + pair.Value + "'", ex);
						}
					}

					SetRecordRequiredFieldsDefaultData(entity, storageRecordData);



					foreach (var item in fileFields)
					{
						Field field = item.Item1;
						string path = item.Item2;
						if (!string.IsNullOrWhiteSpace(path) && path.StartsWith("/fs/"))
							path = path.Substring(3);

						if (!string.IsNullOrWhiteSpace(path) && path.StartsWith("fs/"))
							path = path.Substring(2);

						DbFileRepository fsRepository = new DbFileRepository();

						if (field.Required && string.IsNullOrWhiteSpace(path))
							storageRecordData.Add(new KeyValuePair<string, object>(field.Name, field.GetFieldDefaultValue()));
						else
						{
							if (!string.IsNullOrWhiteSpace(path) && path.StartsWith(DbFileRepository.FOLDER_SEPARATOR + DbFileRepository.TMP_FOLDER_NAME))
							{
								var fileName = path.Split(new[] { '/' }).Last();
								string source = path;
								string target = $"/{field.EntityName}/{record["id"]}/{fileName}";
								var movedFile = fsRepository.Move(source, target, false);

								storageRecordData.Add(new KeyValuePair<string, object>(field.Name, target));
							}
							else
							{
								storageRecordData.Add(new KeyValuePair<string, object>(field.Name, path));
							}
						}
					}

					recRepo.Create(entity.Name, storageRecordData);

					var query = EntityQuery.QueryEQ("id", recordId);
					var entityQuery = new EntityQuery(entity.Name, "*", query);

					// when user create record, it is get returned ignoring create permissions
					bool oldIgnoreSecurity = ignoreSecurity;
					response = Find(entityQuery);
					ignoreSecurity = oldIgnoreSecurity;

					//if not created exit immediately
					if (!(response.Object != null && response.Object.Data != null && response.Object.Data.Count > 0))
					{
						if (isTransactionActive)
							connection.RollbackTransaction();

						response.Success = false;
						response.Object = null;
						response.Timestamp = DateTime.UtcNow;
						response.Message = "The entity record was not created. An internal error occurred!";
						return response;
					}

					foreach (var ooRelData in oneToOneRecordData)
					{
						bool ooHooksExists = RecordHookManager.ContainsAnyHooksForEntity(ooRelData.EntityName);

						EntityRecord ooRecord = new EntityRecord();
						if (ooHooksExists && executeHooks)
						{
							//move values from ooRelData.RecordData to entity record
							var data = (IEnumerable<KeyValuePair<string, object>>)ooRelData.RecordData;
							foreach (var obj in data)
								ooRecord[obj.Key] = obj.Value;

							List<ErrorModel> errors = new List<ErrorModel>();
							RecordHookManager.ExecutePreUpdateRecordHooks(ooRelData.EntityName, ooRecord, errors);
							if (errors.Count > 0)
							{
								if (isTransactionActive)
									connection.RollbackTransaction();

								response.Success = false;
								response.Object = null;
								response.Errors = errors;
								response.Timestamp = DateTime.UtcNow;
								return response;
							}
							//move values from entity record to ooRelData.RecordData, because they may be changed in pre hooks
							List<KeyValuePair<string, object>> recordData = new List<KeyValuePair<string, object>>();
							foreach (var property in ooRecord.Properties)
								recordData.Add(new KeyValuePair<string, object>(property.Key, property.Value));

							ooRelData.RecordData = recordData;
						}

						recRepo.Update(ooRelData.EntityName, ooRelData.RecordData);

						if (ooHooksExists && executeHooks)
							RecordHookManager.ExecutePostUpdateRecordHooks(ooRelData.EntityName, ooRecord);
					}

					foreach (var omRelData in oneToManyRecordData)
					{
						bool ooHooksExists = RecordHookManager.ContainsAnyHooksForEntity(omRelData.EntityName);

						EntityRecord ooRecord = new EntityRecord();
						if (ooHooksExists && executeHooks)
						{
							var data = (IEnumerable<KeyValuePair<string, object>>)omRelData.RecordData;
							foreach (var obj in data)
								ooRecord[obj.Key] = obj.Value;

							List<ErrorModel> errors = new List<ErrorModel>();
							RecordHookManager.ExecutePreUpdateRecordHooks(omRelData.EntityName, ooRecord, errors);
							if (errors.Count > 0)
							{
								if (isTransactionActive)
									connection.RollbackTransaction();

								response.Success = false;
								response.Object = null;
								response.Errors = errors;
								response.Timestamp = DateTime.UtcNow;
								return response;
							}

							//move values from entity record to ooRelData.RecordData, because they may be changed in pre hooks
							List<KeyValuePair<string, object>> recordData = new List<KeyValuePair<string, object>>();
							foreach (var property in ooRecord.Properties)
								recordData.Add(new KeyValuePair<string, object>(property.Key, property.Value));

							omRelData.RecordData = recordData;
						}



						recRepo.Update(omRelData.EntityName, omRelData.RecordData);

						if (ooHooksExists && executeHooks)
							RecordHookManager.ExecutePostUpdateRecordHooks(omRelData.EntityName, ooRecord);
					}

					//TODO implement hooks
					foreach (var mmRelData in manyToManyRecordData)
					{
						var mmResponse = CreateRelationManyToManyRecord(mmRelData.RelationId, mmRelData.OriginFieldValue, mmRelData.TargetFieldValue);

						if (!mmResponse.Success)
							throw new Exception(mmResponse.Message);
					}

					//execute hooks after create related records
					if (response.Object != null && response.Object.Data != null && response.Object.Data.Count > 0)
					{
						response.Message = "Record was created successfully";

						if (hooksExists && executeHooks)
							RecordHookManager.ExecutePostCreateRecordHooks(entity.Name, response.Object.Data[0]);
					}

					if (isTransactionActive)
						connection.CommitTransaction();

					return response;
				}
				catch (ValidationException)
				{
					if (isTransactionActive)
						connection.RollbackTransaction();

					throw;
				}
				catch (Exception e)
				{
					if (isTransactionActive)
						connection.RollbackTransaction();

					response.Success = false;
					response.Object = null;
					response.Timestamp = DateTime.UtcNow;

					if (ErpSettings.DevelopmentMode)
						response.Message = e.Message + e.StackTrace;
					else
						response.Message = "The entity record was not created. An internal error occurred!";

					return response;
				}
			}
		}

		public QueryResponse UpdateRecord(string entityName, EntityRecord record)
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

			return UpdateRecord(entity, record);
		}

		public QueryResponse UpdateRecord(Guid entityId, EntityRecord record)
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

			return UpdateRecord(entity, record);
		}

		public QueryResponse UpdateRecord(Entity entity, EntityRecord record)
		{

			QueryResponse response = new QueryResponse();
			response.Object = null;
			response.Success = true;
			response.Timestamp = DateTime.UtcNow;

			using (DbConnection connection = CurrentContext.CreateConnection())
			{
				bool isTransactionActive = false;

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

					//fixes issue with ID coming from webapi request 
					Guid recordId = Guid.Empty;
					if (record["id"] is string)
						recordId = new Guid(record["id"] as string);
					else if (record["id"] is Guid)
						recordId = (Guid)record["id"];
					else
						throw new Exception("Invalid record id");

					bool hooksExists = RecordHookManager.ContainsAnyHooksForEntity(entity.Name);

					if (record.Properties.Any(p => p.Key.StartsWith("$")) || hooksExists)
					{
						connection.BeginTransaction();
						isTransactionActive = true;
					}

					if (hooksExists && executeHooks)
					{
						List<ErrorModel> errors = new List<ErrorModel>();
						RecordHookManager.ExecutePreUpdateRecordHooks(entity.Name, record, errors);
						if (errors.Count > 0)
						{
							if (isTransactionActive)
								connection.RollbackTransaction();

							response.Success = false;
							response.Object = null;
							response.Errors = errors;
							response.Timestamp = DateTime.UtcNow;
							return response;
						}
					}

					QueryObject filterObj = EntityQuery.QueryEQ("id", recordId);
					var oldRecordResponse = Find(new EntityQuery(entity.Name, "*", filterObj, null, null, null));
					if (!oldRecordResponse.Success)
						throw new Exception(oldRecordResponse.Message);
					else if (oldRecordResponse.Object.Data.Count == 0)
					{
						throw new Exception("Record with such Id is not found");
					}
					var oldRecord = oldRecordResponse.Object.Data[0];

					List<KeyValuePair<string, object>> storageRecordData = new List<KeyValuePair<string, object>>();
					List<dynamic> oneToOneRecordData = new List<dynamic>();
					List<dynamic> oneToManyRecordData = new List<dynamic>();
					List<dynamic> manyToManyRecordData = new List<dynamic>();

					Dictionary<string, EntityRecord> fieldsFromRelationList = new Dictionary<string, EntityRecord>();
					Dictionary<string, EntityRecord> relationFieldMetaList = new Dictionary<string, EntityRecord>();

					foreach (var pair in record.GetProperties())
					{
						try
						{
							if (pair.Key == null)
								continue;

							if (pair.Key.Contains(RELATION_SEPARATOR))
							{
								var relations = GetRelations();

								var relationData = pair.Key.Split(RELATION_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
								if (relationData.Count > 2)
									throw new Exception(string.Format("The specified field name '{0}' is incorrect. Only first level relation can be specified.", pair.Key));

								string relationName = relationData[0];
								string relationFieldName = relationData[1];
								string direction = "origin-target";

								if (string.IsNullOrWhiteSpace(relationName) || relationName == "$" || relationName == "$$")
									throw new Exception(string.Format("Invalid relation '{0}'. The relation name is not specified.", pair.Key));
								else if (!relationName.StartsWith("$"))
									throw new Exception(string.Format("Invalid relation '{0}'. The relation name is not correct.", pair.Key));
								else
									relationName = relationName.Substring(1);

								//check for target priority mark $$
								if (relationName.StartsWith("$"))
								{
									direction = "target-origin";
									relationName = relationName.Substring(1);
								}

								if (string.IsNullOrWhiteSpace(relationFieldName))
									throw new Exception(string.Format("Invalid relation '{0}'. The relation field name is not specified.", pair.Key));

								var relation = relations.SingleOrDefault(x => x.Name == relationName);
								if (relation == null)
									throw new Exception(string.Format("Invalid relation '{0}'. The relation does not exist.", pair.Key));

								if (relation.TargetEntityId != entity.Id && relation.OriginEntityId != entity.Id)
									throw new Exception(string.Format("Invalid relation '{0}'. The relation field belongs to entity that does not relate to current entity.", pair.Key));

								Entity relationEntity = null;
								Field relationField = null;
								Field realtionSearchField;
								Field field = null;

								if (relation.OriginEntityId == relation.TargetEntityId)
								{
									if (direction == "origin-target")
									{
										relationEntity = entity;
										relationField = relationEntity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
										realtionSearchField = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
										field = entity.Fields.FirstOrDefault(f => f.Id == relation.OriginFieldId);
									}
									else
									{
										relationEntity = entity;
										relationField = relationEntity.Fields.FirstOrDefault(f => f.Id == relation.OriginFieldId);
										realtionSearchField = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
										field = entity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
									}
								}
								else if (relation.OriginEntityId == entity.Id)
								{
									//direction doesn't matter
									relationEntity = GetEntity(relation.TargetEntityId);
									relationField = relationEntity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
									realtionSearchField = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
									field = entity.Fields.FirstOrDefault(f => f.Id == relation.OriginFieldId);
								}
								else
								{
									//direction doesn't matter
									relationEntity = GetEntity(relation.OriginEntityId);
									relationField = relationEntity.Fields.FirstOrDefault(f => f.Id == relation.OriginFieldId);
									realtionSearchField = relationEntity.Fields.FirstOrDefault(f => f.Name == relationFieldName);
									field = entity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
								}

								if (realtionSearchField == null)
									throw new Exception(string.Format("Invalid relation '{0}'. Field does not exist.", pair.Key));

								if (realtionSearchField.GetFieldType() == FieldType.MultiSelectField)
									throw new Exception(string.Format("Invalid relation '{0}'. Fields from Multiselect type can't be used as relation fields.", pair.Key));

								QueryObject filter = null;
								if ((relation.RelationType == EntityRelationType.OneToMany && relation.OriginEntityId == relation.TargetEntityId && direction == "origin-target") ||
									(relation.RelationType == EntityRelationType.OneToMany && relation.OriginEntityId != relation.TargetEntityId && relation.OriginEntityId == entity.Id) ||
									relation.RelationType == EntityRelationType.ManyToMany)
								{
									//expect array of values
									if (!record.Properties.ContainsKey(field.Name) || record[field.Name] == null)
										throw new Exception(string.Format("Invalid relation '{0}'. Relation field does not exist into input record data or its value is null.", pair.Key));

									List<string> values = new List<string>();
									if (pair.Value is JArray)
										values = ((JArray)pair.Value).Select(x => ((JToken)x).Value<string>()).ToList<string>();
									else if (pair.Value is List<object>)
										values = ((List<object>)pair.Value).Select(x => ((object)x).ToString()).ToList<string>();
									else if (pair.Value is List<Guid>)
										values = ((List<Guid>)pair.Value).Select(x => ((Guid)x).ToString()).ToList<string>();
									else if (pair.Value is List<string>)
										values = (List<string>)pair.Value;
									else if (pair.Value != null)
										values.Add(pair.Value.ToString());

									if (relation.RelationType == EntityRelationType.ManyToMany)
									{
										Guid? originFieldOldValue = (Guid)oldRecord[field.Name];
										Guid? targetFieldOldValue = null;
										if (relation.TargetEntityId == entity.Id)
										{
											originFieldOldValue = null;
											targetFieldOldValue = (Guid)oldRecord[field.Name];
										}

										var mmResponse = RemoveRelationManyToManyRecord(relation.Id, originFieldOldValue, targetFieldOldValue);

										if (!mmResponse.Success)
											throw new Exception(mmResponse.Message);
									}

									if (values.Count < 1)
										continue;

									List<QueryObject> queries = new List<QueryObject>();
									foreach (var val in values)
									{
										queries.Add(EntityQuery.QueryEQ(realtionSearchField.Name, val));
									}

									filter = EntityQuery.QueryOR(queries.ToArray());
								}
								else
								{
									filter = EntityQuery.QueryEQ(realtionSearchField.Name, ExtractFieldValue(pair, realtionSearchField, true));
								}

								EntityRecord relationFieldMeta = new EntityRecord();
								relationFieldMeta["key"] = pair.Key;
								relationFieldMeta["direction"] = direction;
								relationFieldMeta["relationName"] = relationName;
								relationFieldMeta["relationEntity"] = relationEntity;
								relationFieldMeta["relationField"] = relationField;
								relationFieldMeta["realtionSearchField"] = realtionSearchField;
								relationFieldMeta["field"] = field;
								relationFieldMetaList[pair.Key] = relationFieldMeta;

								EntityRecord fieldsFromRelation = new EntityRecord();

								if (fieldsFromRelationList.Any(r => r.Key == relation.Name))
								{
									fieldsFromRelation = fieldsFromRelationList[relationName];
								}
								else
								{
									fieldsFromRelation["queries"] = new List<QueryObject>();
									fieldsFromRelation["direction"] = direction;
									fieldsFromRelation["relationEntityName"] = relationEntity.Name;
								}

								((List<QueryObject>)fieldsFromRelation["queries"]).Add(filter);
								fieldsFromRelationList[relationName] = fieldsFromRelation;
							}
						}
						catch (Exception ex)
						{
							if (pair.Key != null)
								throw new Exception("Error during processing value for field: '" + pair.Key + "'. Invalid value: '" + pair.Value + "'", ex);
						}
					}

					foreach (var fieldsFromRelation in fieldsFromRelationList)
					{
						EntityRecord fieldsFromRelationValue = (EntityRecord)fieldsFromRelation.Value;
						List<QueryObject> queries = (List<QueryObject>)fieldsFromRelationValue["queries"];
						string direction = (string)fieldsFromRelationValue["direction"];
						string relationEntityName = (string)fieldsFromRelationValue["relationEntityName"];
						QueryObject filter = EntityQuery.QueryAND(queries.ToArray());

						var relation = relations.SingleOrDefault(r => r.Name == fieldsFromRelation.Key);

						//get related records
						QueryResponse relatedRecordResponse = Find(new EntityQuery(relationEntityName, "*", filter, null, null, null));

						if (!relatedRecordResponse.Success || relatedRecordResponse.Object.Data.Count < 1)
						{
							throw new Exception(string.Format("Invalid relation '{0}'. The relation record does not exist.", relationEntityName));
						}
						else if (relatedRecordResponse.Object.Data.Count > 1 && ((relation.RelationType == EntityRelationType.OneToMany && relation.OriginEntityId == relation.TargetEntityId && direction == "target-origin") ||
							(relation.RelationType == EntityRelationType.OneToMany && relation.OriginEntityId != relation.TargetEntityId && relation.TargetEntityId == entity.Id) ||
							relation.RelationType == EntityRelationType.OneToOne))
						{
							//there can be no more than 1 records
							throw new Exception(string.Format("Invalid relation '{0}'. There are multiple relation records matching this value.", relationEntityName));
						}

						((EntityRecord)fieldsFromRelationList[fieldsFromRelation.Key])["relatedRecordResponse"] = relatedRecordResponse;
					}
					List<Tuple<Field, string>> fileFields = new List<Tuple<Field, string>>();
					foreach (var pair in record.GetProperties())
					{
						try
						{
							if (pair.Key == null)
								continue;

							if (pair.Key.Contains(RELATION_SEPARATOR))
							{
								EntityRecord relationFieldMeta = relationFieldMetaList.FirstOrDefault(f => f.Key == pair.Key).Value;

								if (relationFieldMeta == null)
									continue;

								string direction = (string)relationFieldMeta["direction"];
								string relationName = (string)relationFieldMeta["relationName"];
								Entity relationEntity = (Entity)relationFieldMeta["relationEntity"];
								Field relationField = (Field)relationFieldMeta["relationField"];
								Field realtionSearchField = (Field)relationFieldMeta["realtionSearchField"];
								Field field = (Field)relationFieldMeta["field"];

								var relation = relations.SingleOrDefault(r => r.Name == relationName);

								QueryResponse relatedRecordResponse = (QueryResponse)((EntityRecord)fieldsFromRelationList[relationName])["relatedRecordResponse"];

								var relatedRecords = relatedRecordResponse.Object.Data;
								List<Guid> relatedRecordValues = new List<Guid>();
								foreach (var relatedRecord in relatedRecords)
								{
									relatedRecordValues.Add((Guid)relatedRecord[relationField.Name]);
								}

								if (relation.RelationType == EntityRelationType.OneToOne &&
									((relation.OriginEntityId == relation.TargetEntityId && direction == "origin-target") || relation.OriginEntityId == entity.Id))
								{
									if (!record.Properties.ContainsKey(field.Name) || record[field.Name] == null)
										throw new Exception(string.Format("Invalid relation '{0}'. Relation field does not exist into input record data or its value is null.", pair.Key));

									var relatedRecord = relatedRecords[0];
									List<KeyValuePair<string, object>> relRecordData = new List<KeyValuePair<string, object>>();
									relRecordData.Add(new KeyValuePair<string, object>("id", relatedRecord["id"]));
									relRecordData.Add(new KeyValuePair<string, object>(relationField.Name, record[field.Name]));

									dynamic ooRelationData = new ExpandoObject();
									ooRelationData.RelationId = relation.Id;
									ooRelationData.RecordData = relRecordData;
									ooRelationData.EntityName = relationEntity.Name;

									oneToOneRecordData.Add(ooRelationData);
								}
								else if (relation.RelationType == EntityRelationType.OneToMany &&
									((relation.OriginEntityId == relation.TargetEntityId && direction == "origin-target") || relation.OriginEntityId == entity.Id))
								{
									if (!record.Properties.ContainsKey(field.Name) || record[field.Name] == null)
										throw new Exception(string.Format("Invalid relation '{0}'. Relation field does not exist into input record data or its value is null.", pair.Key));

									foreach (var data in relatedRecordResponse.Object.Data)
									{
										List<KeyValuePair<string, object>> relRecordData = new List<KeyValuePair<string, object>>();
										relRecordData.Add(new KeyValuePair<string, object>("id", data["id"]));
										relRecordData.Add(new KeyValuePair<string, object>(relationField.Name, record[field.Name]));

										dynamic omRelationData = new ExpandoObject();
										omRelationData.RelationId = relation.Id;
										omRelationData.RecordData = relRecordData;
										omRelationData.EntityName = relationEntity.Name;

										oneToManyRecordData.Add(omRelationData);
									}
								}
								else if (relation.RelationType == EntityRelationType.ManyToMany)
								{
									foreach (Guid relatedRecordIdValue in relatedRecordValues)
									{
										Guid relRecordId = Guid.Empty;
										if (record[field.Name] is string)
											relRecordId = new Guid(record[field.Name] as string);
										else if (record[field.Name] is Guid)
											relRecordId = (Guid)record[field.Name];
										else
											throw new Exception("Invalid record value for field: '" + pair.Key + "'. Invalid value: '" + pair.Value + "'");

										Guid originFieldValue = relRecordId;
										Guid targetFieldValue = relatedRecordIdValue;

										if (relation.TargetEntityId == entity.Id)
										{
											originFieldValue = relatedRecordIdValue;
											targetFieldValue = relRecordId;
										}

										dynamic mmRelationData = new ExpandoObject();
										mmRelationData.RelationId = relation.Id;
										mmRelationData.OriginFieldValue = originFieldValue;
										mmRelationData.TargetFieldValue = targetFieldValue;

										if (!manyToManyRecordData.Any(r => r.RelationId == mmRelationData.RelationId && r.OriginFieldValue == mmRelationData.OriginFieldValue && r.TargetFieldValue == mmRelationData.TargetFieldValue))
											manyToManyRecordData.Add(mmRelationData);
									}
								}
								else
								{
									if (!storageRecordData.Any(r => r.Key == field.Name))
										storageRecordData.Add(new KeyValuePair<string, object>(field.Name, relatedRecordValues[0]));
								}
							}
							else
							{
								//locate the field
								var field = entity.Fields.SingleOrDefault(x => x.Name == pair.Key);

								if (field == null)
									continue;

								if (field is PasswordField && pair.Value == null)
									continue;

								if (field is AutoNumberField) //always ignored as it is autogenerated
									continue;

								if (field is FileField || field is ImageField)
								{
									fileFields.Add(new Tuple<Field, string>(field, pair.Value as string));
								}
								else
								{
									if (!storageRecordData.Any(r => r.Key == field.Name))
										storageRecordData.Add(new KeyValuePair<string, object>(field.Name, ExtractFieldValue(pair, field, true)));
								}

							}
						}
						catch (Exception ex)
						{
							if (pair.Key != null)
								throw new Exception("Error during processing value for field: '" + pair.Key + "'. Invalid value: '" + pair.Value + "'", ex);
						}
					}

					var recRepo = CurrentContext.RecordRepository;



					DbFileRepository fsRepository = new DbFileRepository();
					foreach (var item in fileFields)
					{
						Field field = item.Item1;
						string path = item.Item2;
						var originalRecordResponse = Find(new EntityQuery(field.EntityName, "*", EntityQuery.QueryEQ("id", record["id"])));
						EntityRecord originalRecord = originalRecordResponse.Object.Data[0];
						var originalFieldData = originalRecord.GetProperties().First(f => f.Key == field.Name);

						if (string.IsNullOrWhiteSpace(path))
						{
							//delete file                            
							string pathToDelete = (string)originalFieldData.Value;
							if (!string.IsNullOrWhiteSpace(path))
								fsRepository.Delete(pathToDelete);

							storageRecordData.Add(new KeyValuePair<string, object>(field.Name, field.GetFieldDefaultValue()));
						}
						else
						{   //update file
							if (path.StartsWith("/fs/"))
								path = path.Substring(3);

							if (path.StartsWith("fs/"))
								path = path.Substring(2);

							if (path.StartsWith(DbFileRepository.FOLDER_SEPARATOR + DbFileRepository.TMP_FOLDER_NAME))
							{
								var fileName = path.Split(new[] { '/' }).Last();
								string source = path;
								string target = $"/{field.EntityName}/{record["id"]}/{fileName}";
								var movedFile = fsRepository.Move(source, target, true);

								storageRecordData.Add(new KeyValuePair<string, object>(field.Name, target));
							}
							else
							{
								storageRecordData.Add(new KeyValuePair<string, object>(field.Name, path));
							}
						}
					}

					recRepo.Update(entity.Name, storageRecordData);

					var query = EntityQuery.QueryEQ("id", recordId);
					var entityQuery = new EntityQuery(entity.Name, "*", query);

					response = Find(entityQuery);
					if (!(response.Object != null && response.Object.Data.Count > 0))
					{
						if (isTransactionActive)
							connection.RollbackTransaction();
						response.Success = false;
						response.Object = null;
						response.Timestamp = DateTime.UtcNow;
						response.Message = "The entity record was not update. An internal error occurred!";
						return response;
					}

					foreach (var ooRelData in oneToOneRecordData)
					{
						bool ooHooksExists = RecordHookManager.ContainsAnyHooksForEntity(ooRelData.EntityName);

						EntityRecord ooRecord = new EntityRecord();
						if (ooHooksExists && executeHooks)
						{
							var data = (IEnumerable<KeyValuePair<string, object>>)ooRelData.RecordData;
							foreach (var obj in data)
								ooRecord[obj.Key] = obj.Value;

							List<ErrorModel> errors = new List<ErrorModel>();
							RecordHookManager.ExecutePreUpdateRecordHooks(ooRelData.EntityName, ooRecord, errors);
							if (errors.Count > 0)
							{
								if (isTransactionActive)
									connection.RollbackTransaction();

								response.Success = false;
								response.Object = null;
								response.Errors = errors;
								response.Timestamp = DateTime.UtcNow;
								return response;
							}

							//move values from entity record to ooRelData.RecordData, because they may be changed in pre hooks
							List<KeyValuePair<string, object>> recordData = new List<KeyValuePair<string, object>>();
							foreach (var property in ooRecord.Properties)
								recordData.Add(new KeyValuePair<string, object>(property.Key, property.Value));

							ooRelData.RecordData = recordData;
						}


						recRepo.Update(ooRelData.EntityName, ooRelData.RecordData);

						if (ooHooksExists && executeHooks)
							RecordHookManager.ExecutePostUpdateRecordHooks(ooRelData.EntityName, ooRecord);
					}

					foreach (var omRelData in oneToManyRecordData)
					{
						bool ooHooksExists = RecordHookManager.ContainsAnyHooksForEntity(omRelData.EntityName);

						EntityRecord ooRecord = new EntityRecord();
						if (ooHooksExists && executeHooks)
						{
							var data = (IEnumerable<KeyValuePair<string, object>>)omRelData.RecordData;
							foreach (var obj in data)
								ooRecord[obj.Key] = obj.Value;

							List<ErrorModel> errors = new List<ErrorModel>();
							RecordHookManager.ExecutePreUpdateRecordHooks(omRelData.EntityName, ooRecord, errors);
							if (errors.Count > 0)
							{
								if (isTransactionActive)
									connection.RollbackTransaction();

								response.Success = false;
								response.Object = null;
								response.Errors = errors;
								response.Timestamp = DateTime.UtcNow;
								return response;
							}

							//move values from entity record to ooRelData.RecordData, because they may be changed in pre hooks
							List<KeyValuePair<string, object>> recordData = new List<KeyValuePair<string, object>>();
							foreach (var property in ooRecord.Properties)
								recordData.Add(new KeyValuePair<string, object>(property.Key, property.Value));

							omRelData.RecordData = recordData;
						}

						recRepo.Update(omRelData.EntityName, omRelData.RecordData);

						if (ooHooksExists && executeHooks)
							RecordHookManager.ExecutePostUpdateRecordHooks(omRelData.EntityName, ooRecord);
					}

					//TODO implement hooks
					foreach (var mmRelData in manyToManyRecordData)
					{
						var mmResponse = CreateRelationManyToManyRecord(mmRelData.RelationId, mmRelData.OriginFieldValue, mmRelData.TargetFieldValue);

						if (!mmResponse.Success)
							throw new Exception(mmResponse.Message);
					}

					//execute hooks after update related records
					if (response.Object != null && response.Object.Data.Count > 0)
					{
						response.Message = "Record was updated successfully";

						if (hooksExists && executeHooks)
							RecordHookManager.ExecutePostUpdateRecordHooks(entity.Name, response.Object.Data[0]);
					}

					if (isTransactionActive)
						connection.CommitTransaction();

					return response;
				}
				catch (ValidationException)
				{
					if (isTransactionActive)
						connection.RollbackTransaction();

					throw;
				}
				catch (Exception e)
				{
					if (isTransactionActive)
						connection.RollbackTransaction();
					response.Success = false;
					response.Object = null;
					response.Timestamp = DateTime.UtcNow;

					if (ErpSettings.DevelopmentMode)
						response.Message = e.Message + e.StackTrace;
					else
						response.Message = "The entity record was not update. An internal error occurred!";

					return response;
				}
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
					bool hooksExists = RecordHookManager.ContainsAnyHooksForEntity(entity.Name);
					if (hooksExists && executeHooks)
					{
						List<ErrorModel> errors = new List<ErrorModel>();
						RecordHookManager.ExecutePreDeleteRecordHooks(entity.Name, response.Object.Data[0], errors);
						if (errors.Count > 0)
						{
							response.Message = errors[0].Message;
							response.Success = false;
							response.Object = null;
							response.Errors = errors;
							response.Timestamp = DateTime.UtcNow;
							return response;
						}
					}

					CurrentContext.RecordRepository.Delete(entity.Name, id);

					if (hooksExists && executeHooks)
						RecordHookManager.ExecutePostDeleteRecordHooks(entity.Name, response.Object.Data[0]);
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

				if (ErpSettings.DevelopmentMode)
					response.Message = e.Message + e.StackTrace;
				else
					response.Message = "The entity record was not update. An internal error occurred!";

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

				//try
				//{
				//	if (query.Query != null)
				//		ProcessQueryObject(entity, query.Query);
				//}
				//catch (Exception ex)
				//{
				//	response.Success = false;
				//	response.Message = "The query is incorrect and cannot be executed.";
				//	response.Object = null;
				//	response.Errors.Add(new ErrorModel { Message = ex.Message });
				//	response.Timestamp = DateTime.UtcNow;
				//	return response;
				//}

				var fields = CurrentContext.RecordRepository.ExtractQueryFieldsMeta(query);
				var data = CurrentContext.RecordRepository.Find(query);
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

				//try
				//{
				//	if (query.Query != null)
				//		ProcessQueryObject(entity, query.Query);
				//}
				//catch (Exception ex)
				//{
				//	response.Success = false;
				//	response.Message = "The query is incorrect and cannot be executed";
				//	response.Object = 0;
				//	response.Errors.Add(new ErrorModel { Message = ex.Message });
				//	response.Timestamp = DateTime.UtcNow;
				//	return response;
				//}

				List<Field> fields = CurrentContext.RecordRepository.ExtractQueryFieldsMeta(query);
				response.Object = CurrentContext.RecordRepository.Count(query);
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
				if (pair.Value == DBNull.Value)
				{
					pair = new KeyValuePair<string, object>(pair.Key, null);
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
				{
					if (pair.Value is string)
						return Convert.ToBoolean(pair.Value as string);
					return pair.Value as bool?;
				}
				else if (field is CurrencyField)
				{
					if (pair.Value == null)
						return null;

					decimal decimalValue;
					if (pair.Value is string)
						decimalValue = decimal.Parse(pair.Value as string);
					else
						decimalValue = Convert.ToDecimal(Convert.ToString(pair.Value));

					return decimal.Round(decimalValue, ((CurrencyField)field).Currency.DecimalDigits, MidpointRounding.AwayFromZero);
				}
				else if (field is DateField)
				{
					if (pair.Value == null)
						return null;

					DateTime? date = null;
					if (pair.Value is string)
					{
						if (string.IsNullOrWhiteSpace(pair.Value as string))
							return null;
						date = DateTime.Parse(pair.Value as string);
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
						date = pair.Value as DateTime?;
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
					if (pair.Value == null)
						return null;

					DateTime? date = null;
					if (pair.Value is string)
					{
						if (string.IsNullOrWhiteSpace(pair.Value as string))
							return null;
						date = DateTime.Parse(pair.Value as string);
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
						date = pair.Value as DateTime?;

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
					return date;
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
				else if (field is GeographyField)
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
			}
			else
			{
				return field.GetFieldDefaultValue();
			}

			throw new Exception("System Error. A field type is not supported in field value extraction process.");
		}

		private Entity GetEntity(string entityName)
		{
			return entityManager.ReadEntity(entityName).Object;
		}

		private Entity GetEntity(Guid entityId)
		{
			return entityManager.ReadEntity(entityId).Object;
		}

		private List<EntityRelation> GetRelations()
		{
			if (relations == null)
				relations = entityRelationManager.Read().Object;

			if (relations == null)
				return new List<EntityRelation>();

			return relations;
		}

		private void SetRecordRequiredFieldsDefaultData(Entity entity, List<KeyValuePair<string, object>> recordData)
		{
			if (recordData == null)
				return;

			if (entity == null)
				return;

			foreach (var field in entity.Fields)
			{
				if (field.Required && !recordData.Any(p => p.Key == field.Name)
					&& field.GetFieldType() != FieldType.AutoNumberField
					&& field.GetFieldType() != FieldType.FileField
					&& field.GetFieldType() != FieldType.ImageField)
				{
					var defaultValue = field.GetFieldDefaultValue();

					recordData.Add(new KeyValuePair<string, object>(field.Name, defaultValue));
				}
			}
		}
	}
}
