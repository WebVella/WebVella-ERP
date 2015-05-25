using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Storage;
using System.Security.Cryptography;
using WebVella.ERP.Utilities;

namespace WebVella.ERP.Api
{
    public class RecordManager
    {
        private IErpService erpService;

        private const string ID_FIELD_NAME = "Id";
        private const string WILDCARD_SYMBOL = "*";
        private const char FIELDS_SEPARATOR = ',';
        private const char RELATION_SEPARATOR = '.';
        private const char RELATION_NAME_RESULT_SEPARATOR = '$';
        private List<Entity> entityCache;
        private EntityManager entityManager;
        private EntityRelationManager entityRelationManager;
        private List<EntityRelation> relations = null;

        /// <summary>
        /// The contructor
        /// </summary>
        /// <param name="service"></param>
        public RecordManager(IErpService service)
        {
            erpService = service;
            entityCache = new List<Entity>();
            entityManager = new EntityManager(erpService.StorageService);
            entityRelationManager = new EntityRelationManager(erpService.StorageService);
        }

        public QueryResponse CreateRelationManyToManyRecord(Guid relationId, Guid originValue, Guid targetValue)
        {
            QueryResponse response = new QueryResponse();
            response.Object = null;
            response.Success = true;
            response.Timestamp = DateTime.UtcNow;

            try
            {
                var relRep = erpService.StorageService.GetEntityRelationRepository();
                var relation = relRep.Read(relationId);

                if (relation == null)
                    response.Errors.Add(new ErrorModel { Message = "Relation does not exists." });

                var targetValues = relRep.ReadManyToManyRecordByOrigin(relationId, originValue);
                if (targetValues.Contains(targetValue))
                    response.Errors.Add(new ErrorModel { Message = "The relation record already exists." });

                if (response.Errors.Count > 0)
                {
                    response.Object = null;
                    response.Success = false;
                    response.Timestamp = DateTime.UtcNow;
                    return response;
                }

                relRep.CreateManyToManyRecord(relationId, originValue, targetValue);
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
                var relRep = erpService.StorageService.GetEntityRelationRepository();
                var relation = relRep.Read(relationId);

                if (relation == null)
                    response.Errors.Add(new ErrorModel { Message = "Relation does not exists." });

                var targetValues = relRep.ReadManyToManyRecordByOrigin(relationId, originValue);
                if (!targetValues.Contains(targetValue))
                    response.Errors.Add(new ErrorModel { Message = "The relation record do not exists." });


                if (response.Errors.Count > 0)
                {
                    response.Object = null;
                    response.Success = false;
                    response.Timestamp = DateTime.UtcNow;
                    return response;
                }

                relRep.DeleteManyToManyRecord(relationId, originValue, targetValue);
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

                List<KeyValuePair<string, object>> storageRecordData = new List<KeyValuePair<string, object>>();

                var recordFields = record.GetProperties();
                foreach (var field in entity.Fields)
                {
                    var pair = recordFields.SingleOrDefault(x => x.Key == field.Name);
                    try {
                        storageRecordData.Add(new KeyValuePair<string, object>(field.Name, ExractFieldValue(pair, field, true)));
                    }
                    catch( Exception ex )
                    {
                        if( pair.Key == null )
                            throw new Exception("Error during processing value for field: '" + field.Name + "'. No value is specified." ); 
                        else
                            throw new Exception("Error during processing value for field: '" + field.Name + "'. Invalid value: '" + pair.Value +"'", ex);
                    }
                }

                var recRepo = erpService.StorageService.GetRecordRepository();
                recRepo.Create(entity.Name, storageRecordData);

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

                        

                        storageRecordData.Add(new KeyValuePair<string, object>(field.Name, ExractFieldValue(pair, field, true)));
                    }
                    catch (Exception ex)
                    {
                        if (pair.Key == null)
                            throw new Exception("Error during processing value for field: '" + field.Name + "'. No value is specified.");
                        else
                            throw new Exception("Error during processing value for field: '" + field.Name + "'. Invalid value: '" + pair.Value + "'", ex);
                    }
                }

                var recRepo = erpService.StorageService.GetRecordRepository();
                recRepo.Update(entity.Name, storageRecordData);

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

        public QueryResponse DeleteRecord(string entityName, Guid id )
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

        public QueryResponse DeleteRecord(Guid entityId, Guid id )
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

        public QueryResponse DeleteRecord(Entity entity, Guid id )
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


                List<KeyValuePair<string, object>> storageRecordData = new List<KeyValuePair<string, object>>();

                var query = EntityQuery.QueryEQ("id", id);
                var entityQuery = new EntityQuery(entity.Name, "*", query);

                response = Find(entityQuery);
                if (response.Object != null && response.Object.Data.Count ==1 )
                {
                    var recRepo = erpService.StorageService.GetRecordRepository();
                    recRepo.Delete(entity.Name, id);
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

                try
                {
                    if (query.Query != null)
                        ProcessQueryObject(entity, query.Query);
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

                IStorageEntityRelationRepository entityRelationRepository = erpService.StorageService.GetEntityRelationRepository();
                List<Field> fields = ExtractQueryFieldsMeta(query);
                var recRepo = erpService.StorageService.GetRecordRepository();
                var storageRecords = recRepo.Find(query.EntityName, query.Query, query.Sort, query.Skip, query.Limit);


                List<EntityRecord> data = new List<EntityRecord>();
                foreach (var record in storageRecords)
                {
                    var dataRecord = new EntityRecord();
                    foreach (var field in fields)
                    {
                        var recValue = record.SingleOrDefault(x => x.Key == field.Name);
                        if (!(field is RelationFieldMeta))
                        {
                            dataRecord[field.Name] = ExractFieldValue(recValue, field);
                        }
                        else //relation field
                        {
                            RelationFieldMeta relationField = (RelationFieldMeta)field;

                            if (relationField.Relation.RelationType == EntityRelationType.OneToOne)
                            {
                                IEnumerable<KeyValuePair<string, object>> relatedStorageRecord = null;
                                //when the relation is origin -> target entity
                                if (relationField.Relation.OriginEntityId == entity.Id)
                                {
                                    recValue = record.SingleOrDefault(x => x.Key == relationField.OriginField.Name);
                                    if (recValue.Value != null)
                                    {
                                        var relQuery = EntityQuery.QueryEQ(relationField.TargetField.Name, recValue.Value);
                                        relatedStorageRecord = recRepo.Find(relationField.TargetEntity.Name, relQuery, null, 0, 1).SingleOrDefault();
                                    }
                                }
                                else //when the relation is target -> origin, we have to query origin entity
                                {
                                    recValue = record.SingleOrDefault(x => x.Key == relationField.TargetField.Name);
                                    if (recValue.Value != null)
                                    {
                                        var relQuery = EntityQuery.QueryEQ(relationField.OriginField.Name, recValue.Value);
                                        relatedStorageRecord = recRepo.Find(relationField.OriginEntity.Name, relQuery, null, 0, 1).SingleOrDefault();
                                    }
                                }

                                var dataArrayRecord = new List<EntityRecord>();
                                if (relatedStorageRecord != null)
                                {
                                    var relatedObject = new EntityRecord();
                                    foreach (var relField in relationField.Fields)
                                    {
                                        var relValue = relatedStorageRecord.SingleOrDefault(x => x.Key == relField.Name);
                                        relatedObject[relField.Name] = ExractFieldValue(relValue, relField);
                                    }
                                    dataArrayRecord.Add(relatedObject);
                                }
                                dataRecord[field.Name] = dataArrayRecord;
                            }
                            else if (relationField.Relation.RelationType == EntityRelationType.OneToMany)
                            {
                                IEnumerable<IEnumerable<KeyValuePair<string, object>>> relatedStorageRecords = null;
                                //when the relation is origin -> target entity
                                if (relationField.Relation.OriginEntityId == entity.Id)
                                {
                                    recValue = record.SingleOrDefault(x => x.Key == relationField.OriginField.Name);
                                    if (recValue.Value != null)
                                    {
                                        var relQuery = EntityQuery.QueryEQ(relationField.TargetField.Name, recValue.Value);
                                        relatedStorageRecords = recRepo.Find(relationField.TargetEntity.Name, relQuery, null, null, null);
                                    }
                                }
                                else //when the relation is target -> origin, we have to query origin entity
                                {
                                    recValue = record.SingleOrDefault(x => x.Key == relationField.TargetField.Name);
                                    if (recValue.Value != null)
                                    {
                                        var relQuery = EntityQuery.QueryEQ(relationField.OriginField.Name, recValue.Value);
                                        relatedStorageRecords = recRepo.Find(relationField.OriginEntity.Name, relQuery, null, null, null);
                                    }
                                }

                                var dataArrayRecord = new List<EntityRecord>();
                                if (relatedStorageRecords != null)
                                {
                                    foreach (var relatedStorageRecord in relatedStorageRecords)
                                    {
                                        var relatedObject = new EntityRecord();
                                        foreach (var relField in relationField.Fields)
                                        {
                                            var relValue = relatedStorageRecord.SingleOrDefault(x => x.Key == relField.Name);
                                            relatedObject[relField.Name] = ExractFieldValue(relValue, relField);
                                        }
                                        dataArrayRecord.Add(relatedObject);
                                    }
                                }
                                dataRecord[field.Name] = dataArrayRecord;
                            }
                            else if (relationField.Relation.RelationType == EntityRelationType.ManyToMany)
                            {
                                List<IEnumerable<KeyValuePair<string, object>>> relatedStorageRecords = null;
                                //when the relation is origin -> target entity
                                if (relationField.Relation.OriginEntityId == entity.Id)
                                {
                                    recValue = record.SingleOrDefault(x => x.Key == relationField.OriginField.Name);
                                    if (recValue.Value != null)
                                    {
                                        List<Guid> relatedRecordIds = entityRelationRepository.ReadManyToManyRecordByTarget(relationField.Relation.Id, (Guid)recValue.Value);
                                        relatedStorageRecords = new List<IEnumerable<KeyValuePair<string, object>>>();
                                        foreach (Guid id in relatedRecordIds)
                                        {
                                            var relQuery = EntityQuery.QueryEQ(relationField.TargetField.Name, id);
                                            var relatedStorageRecord = recRepo.Find(relationField.TargetEntity.Name, relQuery, null, null, null).FirstOrDefault();
                                            if (relatedStorageRecord != null)
                                                relatedStorageRecords.Add(relatedStorageRecord);
                                        }
                                    }
                                }
                                else //when the relation is target -> origin, we have to query origin entity
                                {
                                    recValue = record.SingleOrDefault(x => x.Key == relationField.TargetField.Name);
                                    if (recValue.Value != null)
                                    {

                                        List<Guid> relatedRecordIds = entityRelationRepository.ReadManyToManyRecordByOrigin(relationField.Relation.Id, (Guid)recValue.Value);
                                        relatedStorageRecords = new List<IEnumerable<KeyValuePair<string, object>>>();
                                        foreach (Guid id in relatedRecordIds)
                                        {
                                            var relQuery = EntityQuery.QueryEQ(relationField.OriginField.Name, id);
                                            var relatedStorageRecord = recRepo.Find(relationField.OriginEntity.Name, relQuery, null, null, null).FirstOrDefault();
                                            if (relatedStorageRecord != null)
                                                relatedStorageRecords.Add(relatedStorageRecord);
                                        }
                                    }
                                }

                                var dataArrayRecord = new List<EntityRecord>();
                                if (relatedStorageRecords != null)
                                {
                                    foreach (var relatedStorageRecord in relatedStorageRecords)
                                    {
                                        var relatedObject = new EntityRecord();
                                        foreach (var relField in relationField.Fields)
                                        {
                                            var relValue = relatedStorageRecord.SingleOrDefault(x => x.Key == relField.Name);
                                            relatedObject[relField.Name] = ExractFieldValue(relValue, relField);
                                        }
                                        dataArrayRecord.Add(relatedObject);
                                    }
                                }
                                dataRecord[field.Name] = dataArrayRecord;
                            }
                        }
                    }
                    data.Add(dataRecord);
                }

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

        private object ExractFieldValue(KeyValuePair<string, object>? fieldValue, Field field, bool encryptPasswordFields = false)
        {
            if (fieldValue != null && fieldValue.Value.Key != null)
            {
                var pair = fieldValue.Value;

                if (field is AutoNumberField)
                    return pair.Value as decimal?;
                else if (field is CheckboxField)
                    return pair.Value as bool?;
                else if (field is CurrencyField)
                    return pair.Value as decimal?;
                else if (field is DateField)
                    return pair.Value as DateTime?;
                else if (field is DateTimeField)
                    return pair.Value as DateTime?;
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
                    return pair.Value as IEnumerable<string>;
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
                    return pair.Value as decimal?;
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
                #region <--- the field value doesn't exist. Set defaults from meta

                if (field is AutoNumberField)
                    return ((AutoNumberField)field).DefaultValue;
                else if (field is CheckboxField)
                    return ((CheckboxField)field).DefaultValue;
                else if (field is CurrencyField)
                    return ((CurrencyField)field).DefaultValue;
                else if (field is DateField)
                {
                    if (((DateField)field).UseCurrentTimeAsDefaultValue.Value)
                        return DateTime.UtcNow.Date;
                    else
                        return ((DateField)field).DefaultValue;
                }
                else if (field is DateTimeField)
                {
                    if (((DateTimeField)field).UseCurrentTimeAsDefaultValue.Value)
                        return DateTime.UtcNow;
                    else
                        return ((DateTimeField)field).DefaultValue;
                }
                else if (field is EmailField)
                    return ((EmailField)field).DefaultValue;
                else if (field is FileField)
                    //TODO convert file path to url path
                    return ((FileField)field).DefaultValue;
                else if (field is ImageField)
                    //TODO convert file path to url path
                    return ((ImageField)field).DefaultValue;
                else if (field is HtmlField)
                    return ((HtmlField)field).DefaultValue;
                else if (field is MultiLineTextField)
                    return ((MultiLineTextField)field).DefaultValue;
                else if (field is MultiSelectField)
                    return ((MultiSelectField)field).DefaultValue;
                else if (field is NumberField)
                    return ((NumberField)field).DefaultValue;
                else if (field is PasswordField)
                    return null;
                else if (field is PercentField)
                    return ((PercentField)field).DefaultValue;
                else if (field is PhoneField)
                    return ((PhoneField)field).DefaultValue;
                else if (field is GuidField)
                    throw new Exception("Guid value is missing for Guid type field.");
                else if (field is SelectField)
                    return ((SelectField)field).DefaultValue;
                else if (field is TextField)
                    return ((TextField)field).DefaultValue;
                else if (field is UrlField)
                    return ((UrlField)field).DefaultValue;
                #endregion
            }

            throw new Exception("System Error. A field type is not supported in field value extraction process.");
        }

        private List<Field> ExtractQueryFieldsMeta(EntityQuery query)
        {
            List<Field> result = new List<Field>();

            //split field string into tokens speparated by FIELDS_SEPARATOR
            List<string> tokens = query.Fields.Split(FIELDS_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            //check the query tokens for widcard symbol and validate it is only that symbol
            if (tokens.Count > 1 && tokens.Any(x => x == WILDCARD_SYMBOL))
                throw new Exception("Invalid query syntax. Wildcard symbol can be used only with no other fields.");

            //get entity by name
            Entity entity = GetEntity(query.EntityName);

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

                    if (string.IsNullOrWhiteSpace(relationName) || relationName == "$")
                        throw new Exception(string.Format("Invalid relation '{0}'. The relation name is not specified.", token));
                    else if (!relationName.StartsWith("$"))
                        throw new Exception(string.Format("Invalid relation '{0}'. The relation name is not correct.", token));
                    else
                        relationName = relationName.Substring(1);

                    if (string.IsNullOrWhiteSpace(relationFieldName))
                        throw new Exception(string.Format("Invalid query result field '{0}'. The relation field name is not specified.", token));



                    Field field = result.SingleOrDefault(x => x.Name == "$" + relationName);
                    RelationFieldMeta relationFieldMeta = null;
                    if (field == null)
                    {
                        relationFieldMeta = new RelationFieldMeta();
                        relationFieldMeta.Name = "$" + relationName;
                        result.Add(relationFieldMeta);
                    }
                    else
                        relationFieldMeta = (RelationFieldMeta)field;

                    relationFieldMeta.Relation = GetRelations().SingleOrDefault(x => x.Name == relationName);
                    if (relationFieldMeta.Relation == null)
                        throw new Exception(string.Format("Invalid relation '{0}'. The relation does not exist.", token));

                    if (relationFieldMeta.Relation.TargetEntityId != entity.Id && relationFieldMeta.Relation.OriginEntityId != entity.Id)
                        throw new Exception(string.Format("Invalid relation '{0}'. The relation does relate to queries entity.", token));

                    relationFieldMeta.TargetEntity = GetEntity(relationFieldMeta.Relation.TargetEntityId);
                    relationFieldMeta.OriginEntity = GetEntity(relationFieldMeta.Relation.OriginEntityId);

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

                    var relatedField = joinToEntity.Fields.SingleOrDefault(x => x.Name == relationFieldName);
                    if (relatedField == null)
                        throw new Exception(string.Format("Invalid query result field '{0}'. The relation field does not exist.", token));

                    //if field already added
                    if (relationFieldMeta.Fields.Any(x => x.Id == relatedField.Id))
                        continue;

                    relationFieldMeta.Fields.Add(relatedField);
                }
            }

            return result;
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

            if (obj.QueryType != QueryType.AND && obj.QueryType != QueryType.OR)
            {
                var field = entity.Fields.SingleOrDefault(x => x.Name == obj.FieldName);
                if (field == null)
                    throw new Exception(string.Format("There is not entity field '{0}' you try to query by.", obj.FieldName));
                if (field is NumberField)
                {
                    if (obj.FieldValue != null)
                        obj.FieldValue = Convert.ToDecimal(obj.FieldValue);
                }
                else if (field is AutoNumberField)
                {
                    if (obj.FieldValue != null)
                        obj.FieldValue = Convert.ToDecimal(obj.FieldValue);
                }
                else if (field is PasswordField && obj.FieldValue != null)
                    obj.FieldValue = PasswordUtil.GetMd5Hash(obj.FieldValue as string);

            }

            if (obj.SubQueries != null && obj.SubQueries.Count > 0)
                foreach (var subObj in obj.SubQueries)
                {
                    ProcessQueryObject(entity, subObj);
                }
        }
    }
}