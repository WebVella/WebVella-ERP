using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Utilities;
using Newtonsoft.Json.Linq;
using System.Net;
using WebVella.ERP.Database;
using WebVella.ERP.Utilities.Dynamic;
using System.Dynamic;

namespace WebVella.ERP.Api
{
	public class RecordManager
	{
		private const char RELATION_SEPARATOR = '.';
		private const char RELATION_NAME_RESULT_SEPARATOR = '$';

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

		public QueryResponse CreateRecord(string entityName, EntityRecord record, bool skipRecordReturn = false)
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

			using (DbConnection connection = DbContext.Current.CreateConnection())
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

					SetRecordServiceInformation(record, true, ignoreSecurity);

					if (record.Properties.Any(p => p.Key.StartsWith("$")))
					{
						connection.BeginTransaction();
						isTransactionActive = true;
					}

					Guid recordId = Guid.Empty;
					if (!record.Properties.ContainsKey("id"))
						recordId = Guid.NewGuid();
					else
					{
						//fixes issue with ID comming from webapi request 
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

								if (realtionSearchField.GetFieldType() == FieldType.MultiSelectField || realtionSearchField.GetFieldType() == FieldType.TreeSelectField)
									throw new Exception(string.Format("Invalid relation '{0}'. Fields from Multiselect and Treeselect types can't be used as relation fields.", pair.Key));

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

								if (field is AutoNumberField && pair.Value == null)
									continue;

								if (field.Required && pair.Value == null)
									storageRecordData.Add(new KeyValuePair<string, object>(field.Name, field.GetDefaultValue()));
								else
									storageRecordData.Add(new KeyValuePair<string, object>(field.Name, ExtractFieldValue(pair, field, true)));
							}
						}
						catch (Exception ex)
						{
							if (pair.Key != null)
								throw new Exception("Error during processing value for field: '" + pair.Key + "'. Invalid value: '" + pair.Value + "'", ex);
						}
					}

					SetRecordRequiredFieldsDefaultData(entity, storageRecordData);

					recRepo.Create(entity.Name, storageRecordData);

					foreach (var ooRelData in oneToOneRecordData)
					{
						recRepo.Update(ooRelData.EntityName, ooRelData.RecordData);
					}

					foreach (var omRelData in oneToManyRecordData)
					{
						recRepo.Update(omRelData.EntityName, omRelData.RecordData);
					}

					foreach (var mmRelData in manyToManyRecordData)
					{
						var mmResponse = CreateRelationManyToManyRecord(mmRelData.RelationId, mmRelData.OriginFieldValue, mmRelData.TargetFieldValue);

						if (!mmResponse.Success)
							throw new Exception(mmResponse.Message);
					}

					if (skipRecordReturn)
					{
						response.Object = null;
						response.Success = true;
						response.Message = "Record was created successfully";

						if (isTransactionActive)
							connection.CommitTransaction();
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

					if (isTransactionActive)
						connection.CommitTransaction();

					return response;
				}
				catch (Exception e)
				{
					if (isTransactionActive)
						connection.RollbackTransaction();

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

			return UpdateRecord(entity, record, skipRecordReturn);
		}

		public QueryResponse UpdateRecord(Entity entity, EntityRecord record, bool skipRecordReturn = false)
		{

			QueryResponse response = new QueryResponse();
			response.Object = null;
			response.Success = true;
			response.Timestamp = DateTime.UtcNow;

			using (DbConnection connection = DbContext.Current.CreateConnection())
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

					SetRecordServiceInformation(record, false, ignoreSecurity);

					//fixes issue with ID comming from webapi request 
					Guid recordId = Guid.Empty;
					if (record["id"] is string)
						recordId = new Guid(record["id"] as string);
					else if (record["id"] is Guid)
						recordId = (Guid)record["id"];
					else
						throw new Exception("Invalid record id");

					if (record.Properties.Any(p => p.Key.StartsWith("$")))
					{
						connection.BeginTransaction();
						isTransactionActive = true;
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

								if (realtionSearchField.GetFieldType() == FieldType.MultiSelectField || realtionSearchField.GetFieldType() == FieldType.TreeSelectField)
									throw new Exception(string.Format("Invalid relation '{0}'. Fields from Multiselect and Treeselect types can't be used as relation fields.", pair.Key));

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

								if (field is PasswordField && pair.Value == null)
									continue;

								if (field is AutoNumberField && pair.Value == null)
									continue;

								if (!storageRecordData.Any(r => r.Key == field.Name))
									storageRecordData.Add(new KeyValuePair<string, object>(field.Name, ExtractFieldValue(pair, field, true)));
							}
						}
						catch (Exception ex)
						{
							if (pair.Key != null)
								throw new Exception("Error during processing value for field: '" + pair.Key + "'. Invalid value: '" + pair.Value + "'", ex);
						}
					}

					var recRepo = DbContext.Current.RecordRepository;
					recRepo.Update(entity.Name, storageRecordData);

					foreach (var ooRelData in oneToOneRecordData)
					{
						recRepo.Update(ooRelData.EntityName, ooRelData.RecordData);
					}

					foreach (var omRelData in oneToManyRecordData)
					{
						recRepo.Update(omRelData.EntityName, omRelData.RecordData);
					}

					foreach (var mmRelData in manyToManyRecordData)
					{
						var mmResponse = CreateRelationManyToManyRecord(mmRelData.RelationId, mmRelData.OriginFieldValue, mmRelData.TargetFieldValue);

						if (!mmResponse.Success)
							throw new Exception(mmResponse.Message);
					}

					if (skipRecordReturn)
					{
						response.Object = null;
						response.Success = true;
						response.Message = "Record was updated successfully";

						if (isTransactionActive)
							connection.CommitTransaction();
						return response;
					}

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

					if (isTransactionActive)
						connection.CommitTransaction();
					return response;
				}
				catch (Exception e)
				{
					if (isTransactionActive)
						connection.RollbackTransaction();
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
					return pair.Value as bool?;
				else if (field is CurrencyField)
				{
					if (pair.Value == null)
						return null;

					decimal decimalValue;
					if (pair.Value is string)
						decimalValue = decimal.Parse(pair.Value as string);
					else
						decimalValue = Convert.ToDecimal(pair.Value);

					return decimal.Round(decimalValue, ((CurrencyField)field).Currency.DecimalDigits, MidpointRounding.AwayFromZero);
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
		/*
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
		}*/

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

		private void SetRecordRequiredFieldsDefaultData(Entity entity, List<KeyValuePair<string, object>> recordData)
		{
			if (recordData == null)
				return;

			if (entity == null)
				return;

			foreach (var field in entity.Fields)
			{
				if (field.Required && !recordData.Any(p => p.Key == field.Name) && field.GetFieldType() != FieldType.AutoNumberField)
				{
					var defaultValue = field.GetDefaultValue();

					recordData.Add(new KeyValuePair<string, object>(field.Name, defaultValue));
				}
			}
		}

		public List<EntityRecord> GetListRecords(List<Entity> entities, Entity entity, string listName, int? page = null, QueryObject queryObj = null,
					int? pageSize = null, bool export = false, EntityRelation auxRelation = null, Guid? auxRelatedRecordId = null,
					string auxRelationDirection = "origin-target", List<KeyValuePair<string, string>> overwriteArgs = null, bool returnAllRecords = false,
					int? recordsLmit = null)
		{
			if (entity == null)
				throw new Exception($"Entity '{entity.Name}' do not exist");

			RecordList list = null;
			if (entity != null && entity.RecordLists != null)
				list = entity.RecordLists.FirstOrDefault(l => l.Name == listName);

			if (list == null)
				throw new Exception($"Entity '{entity.Name}' do not have list named '{listName}'");

			//List<KeyValuePair<string, string>> queryStringOverwriteParameters = new List<KeyValuePair<string, string>>();
			//foreach (var key in Request.Query.Keys)
			//	queryStringOverwriteParameters.Add(new KeyValuePair<string, string>(key, Request.Query[key]));


			EntityQuery resultQuery = new EntityQuery(entity.Name, "*", queryObj, null, null, null, overwriteArgs);
			EntityRelationManager relManager = new EntityRelationManager();
			EntityRelationListResponse relListResponse = relManager.Read();
			List<EntityRelation> relationList = new List<EntityRelation>();
			if (relListResponse.Object != null)
				relationList = relListResponse.Object;

			if (list != null)
			{
				List<QuerySortObject> sortList = new List<QuerySortObject>();
				if (list.Sorts != null && list.Sorts.Count > 0)
				{
					foreach (var sort in list.Sorts)
					{
						QuerySortType sortType;
						if (Enum.TryParse<QuerySortType>(sort.SortType, true, out sortType))
							sortList.Add(new QuerySortObject(sort.FieldName, sortType));
					}
					resultQuery.Sort = sortList.ToArray();
				}

				if (list.Query != null)
				{
					var listQuery = RecordListQuery.ConvertQuery(list.Query);

					if (queryObj != null)
					{
						//if (queryObj.SubQueries != null && queryObj.SubQueries.Any())
						//	queryObj.SubQueries.Add(listQuery);
						//else
						queryObj = EntityQuery.QueryAND(listQuery, queryObj);
					}
					else
						queryObj = listQuery;

					resultQuery.Query = queryObj;
				}

				if (auxRelation != null && auxRelatedRecordId != null)
				{
					string relationField = $"${auxRelation.Name}.id";
					if (auxRelationDirection == "target-origin")
						relationField = "$" + relationField;

					var auxRelQuery = EntityQuery.QueryEQ(relationField, auxRelatedRecordId);
					if (resultQuery.Query != null)
						resultQuery.Query = EntityQuery.QueryAND(resultQuery.Query, auxRelQuery);
					else
						resultQuery.Query = auxRelQuery;

				}

				string queryFields = "id,";
				if (list.Columns != null)
				{
					foreach (var column in list.Columns)
					{
						if (column is RecordListFieldItem)
						{
							if (((RecordListFieldItem)column).Meta.Name != "id")
								queryFields += ((RecordListFieldItem)column).Meta.Name + ", ";
						}
						else if (column is RecordListRelationTreeItem)
						{
							if (export)
								continue;

							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationTreeItem)column).RelationId);

							string relName = relation != null ? string.Format("${0}.", relation.Name) : "";

							Guid relEntityId = relation.OriginEntityId;
							Guid relFieldId = relation.OriginFieldId;

							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							var treeId = (column as RecordListRelationTreeItem).TreeId;
							RecordTree tree = relEntity.RecordTrees.Single(x => x.Id == treeId);

							var relIdField = relEntity.Fields.Single(x => x.Name == "id");

							List<Guid> fieldIdsToInclude = new List<Guid>();
							fieldIdsToInclude.AddRange(tree.NodeObjectProperties);

							if (!fieldIdsToInclude.Contains(relIdField.Id))
								fieldIdsToInclude.Add(relIdField.Id);

							if (!fieldIdsToInclude.Contains(tree.NodeNameFieldId))
								fieldIdsToInclude.Add(tree.NodeNameFieldId);

							if (!fieldIdsToInclude.Contains(tree.NodeLabelFieldId))
								fieldIdsToInclude.Add(tree.NodeLabelFieldId);

							if (!fieldIdsToInclude.Contains(relField.Id))
								fieldIdsToInclude.Add(relField.Id);

							foreach (var fieldId in fieldIdsToInclude)
							{
								var f = relEntity.Fields.SingleOrDefault(x => x.Id == fieldId);
								if (f != null)
								{
									string qFieldName = string.Format("{0}{1},", relName, f.Name);
									if (!queryFields.Contains(qFieldName))
										queryFields += qFieldName;
								}
							}

							//always add target field in query, its value may be required for relative view and list
							Field field = entity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
							queryFields += field.Name + ", ";
						}
						else if (column is RecordListRelationFieldItem)
						{
							string targetOriginPrefix = "";
							if (list.RelationOptions != null)
							{
								var options = list.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordListRelationFieldItem)column).RelationId);
								if (options != null && options.Direction == "target-origin")
									targetOriginPrefix = "$";
							}

							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationFieldItem)column).RelationId);
							queryFields += string.Format(targetOriginPrefix + "${0}.{1}, ", relation.Name, ((RecordListRelationFieldItem)column).Meta.Name);

							//add ID field automatically if not added
							if (!queryFields.Contains(string.Format(targetOriginPrefix + "${0}.id", relation.Name)))
								queryFields += string.Format(targetOriginPrefix + "${0}.id,", relation.Name);

							//always add origin field in query, its value may be required for relative view and list
							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							queryFields += field.Name + ", ";
						}
						else if (column is RecordListListItem || column is RecordListViewItem)
						{
							if (export)
								continue;

							if (!queryFields.Contains(" id, ") && !queryFields.StartsWith("id,"))
								queryFields += "id, ";
						}
						else if (column is RecordListRelationListItem)
						{
							if (export)
								continue;

							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationListItem)column).RelationId);

							string targetOriginPrefix = "";
							if (list.RelationOptions != null)
							{
								var options = list.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordListRelationListItem)column).RelationId);
								if (options != null && options.Direction == "target-origin")
									targetOriginPrefix = "$";
							}

							string relName = relation != null ? string.Format(targetOriginPrefix + "${0}.", relation.Name) : "";

							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;

							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							string queryFieldName = string.Format("{0}{1}, ", relName, relField.Name);

							if (!queryFields.Contains(queryFieldName))
								queryFields += queryFieldName;

							//always add origin field in query, its value may be required for relative view and list
							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							queryFields += field.Name + ", ";
						}
						else if (column is RecordListRelationViewItem)
						{
							if (export)
								continue;

							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationViewItem)column).RelationId);

							string targetOriginPrefix = "";
							if (list.RelationOptions != null)
							{
								var options = list.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordListRelationViewItem)column).RelationId);
								if (options != null && options.Direction == "target-origin")
									targetOriginPrefix = "$";
							}

							string relName = relation != null ? string.Format(targetOriginPrefix + "${0}.", relation.Name) : "";

							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;

							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							string queryFieldName = string.Format("{0}{1}, ", relName, relField.Name);

							if (!queryFields.Contains(queryFieldName))
								queryFields += queryFieldName;

							//always add origin field in query, its value may be required for relative view and list
							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							queryFields += field.Name + ", ";
						}
					}

					if (queryFields.EndsWith(", "))
						queryFields = queryFields.Remove(queryFields.Length - 2);

					resultQuery.Fields = queryFields;

				}

				if (returnAllRecords)
				{
					resultQuery.Skip = null;
					resultQuery.Limit = null;
				}
				else
				{
					if (!pageSize.HasValue)
						pageSize = list.PageSize;

					if (pageSize.Value > 0)
					{
						resultQuery.Limit = pageSize.Value;
						if (page != null && page > 0)
							resultQuery.Skip = (page - 1) * resultQuery.Limit;
					}
				}

				if (recordsLmit.HasValue && recordsLmit.Value > 0)
				{
					resultQuery.Limit = recordsLmit;
				}
			}

			List<EntityRecord> resultDataList = new List<EntityRecord>();

			QueryResponse result = Find(resultQuery);
			if (!result.Success)
				if (result.Errors.Count > 0)
				{
					throw new Exception(result.Message + ". Reason: " + result.Errors[0].Message);
				}
				else
				{
					throw new Exception(result.Message);
				}

			if (list != null)
			{
				foreach (var record in result.Object.Data)
				{
					EntityRecord dataRecord = new EntityRecord();
					//always add id value
					dataRecord["id"] = record["id"];

					foreach (var column in list.Columns)
					{
						if (column is RecordListFieldItem)
						{
							dataRecord[column.DataName] = record[((RecordListFieldItem)column).FieldName];
						}
						else if (column is RecordListRelationFieldItem)
						{
							string propName = string.Format("${0}", ((RecordListRelationFieldItem)column).RelationName);
							List<EntityRecord> relFieldRecords = (List<EntityRecord>)record[propName];

							string idDataName = "$field" + propName + "$id";
							if (!dataRecord.Properties.ContainsKey(idDataName))
							{
								List<object> idFieldRecord = new List<object>();
								if (relFieldRecords != null)
								{
									foreach (var relFieldRecord in relFieldRecords)
										idFieldRecord.Add(relFieldRecord["id"]);
								}
								dataRecord[idDataName] = idFieldRecord;
							}

							List<object> resultFieldRecord = new List<object>();
							if (relFieldRecords != null)
							{
								foreach (var relFieldRecord in relFieldRecords)
								{
									resultFieldRecord.Add(relFieldRecord[((RecordListRelationFieldItem)column).FieldName]);
								}
							}
							dataRecord[column.DataName] = resultFieldRecord;


						}
						else if (column is RecordListListItem)
						{
							if (export)
								continue;

							dataRecord[column.DataName] = GetListRecords(entities, entity, ((RecordListListItem)column).ListName);
						}
						else if (column is RecordListRelationListItem)
						{
							if (export)
								continue;

							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationListItem)column).RelationId);
							string relName = string.Format("${0}", relation.Name);

							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							List<QueryObject> queries = new List<QueryObject>();
							foreach (var relatedRecord in relatedRecords)
								queries.Add(EntityQuery.QueryEQ(relField.Name, relatedRecord[relField.Name]));

							if (queries.Count > 0)
							{
								QueryObject subListQueryObj = EntityQuery.QueryOR(queries.ToArray());
								List<EntityRecord> subListResult = GetListRecords(entities, relEntity, ((RecordListRelationListItem)column).ListName, queryObj: subListQueryObj);
								dataRecord[((RecordListRelationListItem)column).DataName] = subListResult;
							}
							else
								dataRecord[((RecordListRelationListItem)column).DataName] = new List<object>();
						}
						else if (column is RecordListViewItem)
						{
							if (export)
								continue;

							dataRecord[column.DataName] = GetViewRecords(entities, entity, ((RecordListViewItem)column).ViewName, "id", record["id"]);
						}
						else if (column is RecordListRelationTreeItem)
						{
							if (export)
								continue;

							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationTreeItem)column).RelationId);
							string relName = string.Format("${0}", relation.Name);

							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							dataRecord[((RecordListRelationTreeItem)column).DataName] = relatedRecords;
						}
						else if (column is RecordListRelationViewItem)
						{
							if (export)
								continue;

							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordListRelationViewItem)column).RelationId);
							string relName = string.Format("${0}", relation.Name);

							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							List<EntityRecord> subViewResult = new List<EntityRecord>();
							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							foreach (var relatedRecord in relatedRecords)
							{
								subViewResult.AddRange(GetViewRecords(entities, relEntity, ((RecordListRelationViewItem)column).ViewName, relField.Name, relatedRecord[relField.Name]));
							}
							dataRecord[((RecordListRelationViewItem)column).DataName] = subViewResult;
						}
					}

					resultDataList.Add(dataRecord);
				}
			}
			else
			{
				foreach (var record in result.Object.Data)
				{
					EntityRecord dataRecord = new EntityRecord();
					foreach (var prop in record.Properties)
					{
						//string propName = "$field" + (prop.Key.StartsWith("$") ? prop.Key : "$" + prop.Key);
						string propName = prop.Key;
						dataRecord[propName] = record[prop.Key];
					}

					resultDataList.Add(dataRecord);
				}
			}

			return resultDataList;
		}

		public List<EntityRecord> GetViewRecords(List<Entity> entities, Entity entity, string viewName, string queryFieldName, object queryFieldValue)
		{
			EntityQuery resultQuery = new EntityQuery(entity.Name, "*", EntityQuery.QueryEQ(queryFieldName, queryFieldValue));

			EntityRelationManager relManager = new EntityRelationManager();
			EntityRelationListResponse relListResponse = relManager.Read();
			List<EntityRelation> relationList = new List<EntityRelation>();
			if (relListResponse.Object != null)
				relationList = relListResponse.Object;

			RecordView view = null;
			if (entity != null && entity.RecordViews != null)
				view = entity.RecordViews.FirstOrDefault(v => v.Name == viewName);

			List<EntityRecord> resultDataList = new List<EntityRecord>();

			string queryFields = "id,";

			//List<RecordViewItemBase> items = new List<RecordViewItemBase>();
			List<object> items = new List<object>();

			if (view != null)
			{

				if (view.Sidebar.Items.Any())
					items.AddRange(view.Sidebar.Items);

				foreach (var region in view.Regions)
				{
					if (region.Sections == null)
						continue;

					foreach (var section in region.Sections)
					{
						if (section.Rows == null)
							continue;

						foreach (var row in section.Rows)
						{
							if (row.Columns == null)
								continue;

							foreach (var column in row.Columns)
							{
								if (column.Items != null && column.Items.Count > 0)
									items.AddRange(column.Items);
							}
						}
					}
				}

				foreach (var item in items)
				{
					if (item is RecordViewFieldItem)
					{
						if (((RecordViewFieldItem)item).Meta.Name != "id")
							queryFields += ((RecordViewFieldItem)item).Meta.Name;
					}
					else if (item is RecordViewRelationFieldItem)
					{
						string targetOriginPrefix = "";
						if (view.RelationOptions != null)
						{
							var options = view.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewRelationFieldItem)item).RelationId);
							if (options != null && options.Direction == "target-origin")
								targetOriginPrefix = "$";

						}
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationFieldItem)item).RelationId);

						//add ID field automatically if not added
						if (!queryFields.Contains(string.Format(targetOriginPrefix + "${0}.id", relation.Name)))
							queryFields += string.Format(targetOriginPrefix + "${0}.id,", relation.Name);

						Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
						Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);

						queryFields += field.Name + ", ";
						queryFields += string.Format(targetOriginPrefix + "${0}.{1}, ", relation.Name, ((RecordViewRelationFieldItem)item).Meta.Name);

					}
					else if (item is RecordViewListItem || item is RecordViewViewItem)
					{
						if (!queryFields.Contains(" id, ") && !queryFields.StartsWith("id,"))
							queryFields += "id";
					}
					else if (item is RecordViewRelationTreeItem)
					{
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationTreeItem)item).RelationId);

						string relName = relation != null ? string.Format("${0}.", relation.Name) : "";

						Guid relEntityId = relation.OriginEntityId;
						Guid relFieldId = relation.OriginFieldId;

						Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
						Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

						var treeId = (item as RecordViewRelationTreeItem).TreeId;
						RecordTree tree = relEntity.RecordTrees.Single(x => x.Id == treeId);

						var relIdField = relEntity.Fields.Single(x => x.Name == "id");

						List<Guid> fieldIdsToInclude = new List<Guid>();
						fieldIdsToInclude.AddRange(tree.NodeObjectProperties);

						if (!fieldIdsToInclude.Contains(relIdField.Id))
							fieldIdsToInclude.Add(relIdField.Id);

						if (!fieldIdsToInclude.Contains(tree.NodeNameFieldId))
							fieldIdsToInclude.Add(tree.NodeNameFieldId);

						if (!fieldIdsToInclude.Contains(tree.NodeLabelFieldId))
							fieldIdsToInclude.Add(tree.NodeLabelFieldId);

						if (!fieldIdsToInclude.Contains(relField.Id))
							fieldIdsToInclude.Add(relField.Id);

						foreach (var fieldId in fieldIdsToInclude)
						{
							var f = relEntity.Fields.SingleOrDefault(x => x.Id == fieldId);
							if (f != null)
							{
								string qFieldName = string.Format("{0}{1},", relName, f.Name);
								if (!queryFields.Contains(qFieldName))
									queryFields += qFieldName;
							}
						}

						//always add target field in query, its value may be required for relative view and list
						Field field = entity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
						queryFields += field.Name + ", ";
					}
					else if (item is RecordViewRelationListItem)
					{
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationListItem)item).RelationId);

						string targetOriginPrefix = "";
						if (view.RelationOptions != null)
						{
							var options = view.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewRelationListItem)item).RelationId);
							if (options != null && options.Direction == "target-origin")
								targetOriginPrefix = "$";
						}

						string relName = relation != null ? string.Format(targetOriginPrefix + "${0}.", relation.Name) : "";

						Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
						Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;

						Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
						Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

						string qFieldName = string.Format("{0}{1},", relName, relField.Name);

						if (!queryFields.Contains(qFieldName))
							queryFields += qFieldName;

						//always add origin field in query, its value may be required for relative view and list
						Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
						Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
						queryFields += field.Name + ", ";

					}
					else if (item is RecordViewRelationViewItem)
					{
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationViewItem)item).RelationId);

						string targetOriginPrefix = "";
						if (view.RelationOptions != null)
						{
							var options = view.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewRelationViewItem)item).RelationId);
							if (options != null && options.Direction == "target-origin")
								targetOriginPrefix = "$";
						}

						string relName = relation != null ? string.Format(targetOriginPrefix + "${0}.", relation.Name) : "";

						Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
						Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;

						Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
						Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

						string qFieldName = string.Format("{0}{1},", relName, relField.Name);

						if (!queryFields.Contains(qFieldName))
							queryFields += qFieldName;

						//always add origin field in query, its value may be required for relative view and list
						Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
						Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
						queryFields += field.Name + ", ";
					}
					else if (item is RecordViewSidebarViewItem)
					{
						//nothing to add, just check for record id
						if (!queryFields.Contains(" id, ") && !queryFields.StartsWith("id,"))
							queryFields += "id";
					}
					else if (item is RecordViewSidebarListItem)
					{
						//nothing to add, just check for record id
						if (!queryFields.Contains(" id, ") && !queryFields.StartsWith("id,"))
							queryFields += "id";
					}
					else if (item is RecordViewSidebarRelationTreeItem)
					{
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationTreeItem)item).RelationId);

						string relName = relation != null ? string.Format("${0}.", relation.Name) : "";

						Guid relEntityId = relation.OriginEntityId;
						Guid relFieldId = relation.OriginFieldId;

						Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
						Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

						string qFieldName = string.Format("{0}{1},", relName, relField.Name);

						if (!queryFields.Contains(qFieldName))
							queryFields += qFieldName;

						//always add target field in query, its value may be required for relative view and list
						Field field = entity.Fields.FirstOrDefault(f => f.Id == relation.TargetFieldId);
						queryFields += field.Name + ", ";
					}
					else if (item is RecordViewSidebarRelationListItem)
					{
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationListItem)item).RelationId);

						string targetOriginPrefix = "";
						if (view.RelationOptions != null)
						{
							var options = view.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewSidebarRelationListItem)item).RelationId);
							if (options != null && options.Direction == "target-origin")
								targetOriginPrefix = "$";
						}

						string relName = relation != null ? string.Format(targetOriginPrefix + "${0}.", relation.Name) : "";

						Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
						Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;

						Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
						Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

						string qFieldName = string.Format("{0}{1},", relName, relField.Name);

						if (!queryFields.Contains(qFieldName))
							queryFields += qFieldName;

						//always add origin field in query, its value may be required for relative view and list
						Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
						Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
						queryFields += field.Name + ", ";
					}
					else if (item is RecordViewSidebarRelationViewItem)
					{
						EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationViewItem)item).RelationId);

						string targetOriginPrefix = "";
						if (view.RelationOptions != null)
						{
							var options = view.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewSidebarRelationViewItem)item).RelationId);
							if (options != null && options.Direction == "target-origin")
								targetOriginPrefix = "$";
						}

						string relName = relation != null ? string.Format(targetOriginPrefix + "${0}.", relation.Name) : "";

						Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
						Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;

						Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
						Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

						string qFieldName = string.Format("{0}{1},", relName, relField.Name);

						if (!queryFields.Contains(qFieldName))
							queryFields += qFieldName;

						//always add origin field in query, its value may be required for relative view and list
						Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
						Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
						queryFields += field.Name + ", ";
					}

					queryFields += ",";
				}

				queryFields = queryFields.Trim();
				if (queryFields.EndsWith(","))
					queryFields = queryFields.Remove(queryFields.Length - 1);

				resultQuery.Fields = queryFields;
			}

			QueryResponse result = Find(resultQuery);
			if (!result.Success)
				return resultDataList;

			if (view != null)
			{
				foreach (var record in result.Object.Data)
				{
					EntityRecord dataRecord = new EntityRecord();
					//always add id value
					dataRecord["id"] = record["id"];

					foreach (var item in items)
					{
						if (item is RecordViewFieldItem)
						{
							dataRecord[((RecordViewFieldItem)item).DataName] = record[((RecordViewFieldItem)item).FieldName];
						}
						else if (item is RecordViewListItem)
						{
							dataRecord[((RecordViewListItem)item).DataName] = GetListRecords(entities, entity, ((RecordViewListItem)item).ListName);
						}
						else if (item is RecordViewViewItem)
						{
							dataRecord[((RecordViewViewItem)item).DataName] = GetViewRecords(entities, entity, ((RecordViewViewItem)item).ViewName, "id", record["id"]);
						}
						else if (item is RecordViewRelationFieldItem)
						{
							string propName = string.Format("${0}", ((RecordViewRelationFieldItem)item).RelationName);
							List<EntityRecord> relFieldRecords = (List<EntityRecord>)record[propName];

							string idDataName = "$field" + propName + "$id";
							if (!dataRecord.Properties.ContainsKey(idDataName))
							{
								List<object> idFieldRecord = new List<object>();
								if (relFieldRecords != null)
								{
									foreach (var relFieldRecord in relFieldRecords)
										idFieldRecord.Add(relFieldRecord["id"]);
								}
								dataRecord[idDataName] = idFieldRecord;
							}

							List<object> resultFieldRecord = new List<object>();
							if (relFieldRecords != null)
							{
								foreach (var relFieldRecord in relFieldRecords)
								{
									resultFieldRecord.Add(relFieldRecord[((RecordViewRelationFieldItem)item).FieldName]);
								}
							}
							dataRecord[((RecordViewRelationFieldItem)item).DataName] = resultFieldRecord;
						}
						else if (item is RecordViewRelationTreeItem)
						{
							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationTreeItem)item).RelationId);
							string relName = string.Format("${0}", relation.Name);

							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							dataRecord[((RecordViewRelationTreeItem)item).DataName] = relatedRecords;
						}
						else if (item is RecordViewRelationListItem)
						{
							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationListItem)item).RelationId);
							string relName = string.Format("${0}", relation.Name);

							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							List<QueryObject> queries = new List<QueryObject>();
							foreach (var relatedRecord in relatedRecords)
								queries.Add(EntityQuery.QueryEQ(relField.Name, relatedRecord[relField.Name]));

							if (queries.Count > 0)
							{
								QueryObject subListQueryObj = EntityQuery.QueryOR(queries.ToArray());
								List<EntityRecord> subListResult = GetListRecords(entities, relEntity, ((RecordViewRelationListItem)item).ListName, queryObj: subListQueryObj);
								dataRecord[((RecordViewRelationListItem)item).DataName] = subListResult;
							}
							else
								dataRecord[((RecordViewRelationListItem)item).DataName] = new List<object>();
						}
						else if (item is RecordViewRelationViewItem)
						{
							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationViewItem)item).RelationId);
							string relName = string.Format("${0}", relation.Name);

							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							List<EntityRecord> subViewResult = new List<EntityRecord>();
							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							foreach (var relatedRecord in relatedRecords)
							{
								subViewResult.AddRange(GetViewRecords(entities, relEntity, ((RecordViewRelationViewItem)item).ViewName, "id", relatedRecord["id"]));
							}
							dataRecord[((RecordViewRelationViewItem)item).DataName] = subViewResult;

						}
						else if (item is RecordViewSidebarViewItem)
						{
							List<EntityRecord> subViewResult = GetViewRecords(entities, entity, ((RecordViewSidebarViewItem)item).ViewName, "id", record["id"]);
							dataRecord[((RecordViewSidebarViewItem)item).DataName] = subViewResult;
						}
						else if (item is RecordViewSidebarListItem)
						{
							var query = EntityQuery.QueryEQ("id", record["id"]);
							List<EntityRecord> subListResult = GetListRecords(entities, entity, ((RecordViewSidebarListItem)item).ListName, queryObj: query);
							dataRecord[((RecordViewSidebarListItem)item).DataName] = subListResult;
						}
						else if (item is RecordViewSidebarRelationTreeItem)
						{
							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationTreeItem)item).RelationId);
							string relName = string.Format("${0}", relation.Name);

							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							dataRecord[((RecordViewSidebarRelationTreeItem)item).DataName] = relatedRecords;
						}
						else if (item is RecordViewSidebarRelationListItem)
						{
							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationListItem)item).RelationId);
							string relName = string.Format("${0}", relation.Name);

							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							List<QueryObject> queries = new List<QueryObject>();
							foreach (var relatedRecord in relatedRecords)
								queries.Add(EntityQuery.QueryEQ(relField.Name, relatedRecord[relField.Name]));

							if (queries.Count > 0)
							{
								//QueryObject subListQueryObj = EntityQuery.QueryEQ(relField.Name, record[field.Name]);
								QueryObject subListQueryObj = EntityQuery.QueryOR(queries.ToArray());
								List<EntityRecord> subListResult = GetListRecords(entities, relEntity, ((RecordViewSidebarRelationListItem)item).ListName, queryObj: subListQueryObj);
								dataRecord[((RecordViewSidebarRelationListItem)item).DataName] = subListResult;
							}
							else
								dataRecord[((RecordViewSidebarRelationListItem)item).DataName] = new List<object>();
						}
						else if (item is RecordViewSidebarRelationViewItem)
						{
							EntityRelation relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationViewItem)item).RelationId);
							string relName = string.Format("${0}", relation.Name);

							Guid fieldId = entity.Id == relation.OriginEntityId ? relation.OriginFieldId : relation.TargetFieldId;
							Field field = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
							Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
							Guid relFieldId = entity.Id == relation.OriginEntityId ? relation.TargetFieldId : relation.OriginFieldId;
							Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);
							Field relField = relEntity.Fields.FirstOrDefault(f => f.Id == relFieldId);

							List<EntityRecord> subViewResult = new List<EntityRecord>();
							var relatedRecords = record["$" + relation.Name] as List<EntityRecord>;
							foreach (var relatedRecord in relatedRecords)
							{
								subViewResult.AddRange(GetViewRecords(entities, relEntity, ((RecordViewSidebarRelationViewItem)item).ViewName, "id", relatedRecord["id"]));
							}
							dataRecord[((RecordViewSidebarRelationViewItem)item).DataName] = subViewResult;
						}
					}

					resultDataList.Add(dataRecord);
				}
			}
			else
			{
				foreach (var record in result.Object.Data)
				{
					EntityRecord dataRecord = new EntityRecord();
					foreach (var prop in record.Properties)
					{
						//string propName = "$field" + (prop.Key.StartsWith("$") ? prop.Key : "$" + prop.Key);
						string propName = prop.Key;
						dataRecord[propName] = record[prop.Key];
					}

					resultDataList.Add(dataRecord);
				}
			}

			return resultDataList;
		}
	}
}