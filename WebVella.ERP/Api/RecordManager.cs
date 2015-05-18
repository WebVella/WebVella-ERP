using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Storage;
using System.Dynamic;

namespace WebVella.ERP.Api
{
    public class RecordManager
    {
        private IERPService erpService;

        private const string ID_FIELD_NAME = "Id";
        private const string WILDCARD_SYMBOL = "*";
        private const char FIELDS_SEPARATOR = ',';
        private const char RELATION_SEPARATOR = '.';
        private const char RELATION_NAME_RESULT_SEPARATOR = '$';
        private List<Entity> entityCache;
        private EntityManager entityManager;
        private EntityRelationManager entityRelationManager;

        /// <summary>
        /// The contructor
        /// </summary>
        /// <param name="service"></param>
        public RecordManager(IERPService service)
        {
            erpService = service;
            entityCache = new List<Entity>();
            entityManager = new EntityManager(erpService.StorageService);
            entityRelationManager = new EntityRelationManager(erpService.StorageService);
        }

        public SingleRecordResponse CreateRecord(string entityName, EntityRecord record)
        {
            if (string.IsNullOrWhiteSpace(entityName))
            {
                SingleRecordResponse response = new SingleRecordResponse
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
                SingleRecordResponse response = new SingleRecordResponse
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

        public SingleRecordResponse CreateRecord(Guid entityId, EntityRecord record)
        {
            Entity entity = GetEntity(entityId);
            if (entity == null)
            {
                SingleRecordResponse response = new SingleRecordResponse
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

        public SingleRecordResponse CreateRecord(Entity entity, EntityRecord record)
        {

            if (entity == null)
            {
                //TODO 
                return null;
            }

            if (record == null)
            {
                //TODO 
                return null;
            }

            List<KeyValuePair<string, object>> storageRecordData = new List<KeyValuePair<string, object>>();

            var recordFields = record.GetProperties();
            foreach (var field in entity.Fields)
            {
                var pair = recordFields.SingleOrDefault(x => x.Key == field.Name);
                storageRecordData.Add(new KeyValuePair<string, object>(field.Name, ExractFieldValue(pair, field)));
            }

            var recRep = erpService.StorageService.GetRecordRepository();
            recRep.Create(entity.Name, storageRecordData);
            return null;
        }

        public QueryResponse Find(EntityQuery query )
        {
            QueryResponse response = new QueryResponse
            {
                Success = true,
                Message = "The entity was successfully created!",
                Timestamp = DateTime.UtcNow
            };

            try
            {
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
                        if (!(field is GuidFieldMeta))
                        {
                            dataRecord[field.Name] = ExractFieldValue(recValue, field);
                        }
                        else //relation field
                        {
                            //if we don't have any value for targetField, we set null
                            if (!record.Any(x => x.Key == field.Name))
                            {
                                dataRecord[field.Name] = null;
                                continue;
                            }

                            GuidFieldMeta targetField = (GuidFieldMeta)field;
                            var originEntity = GetEntity(targetField.Relation.OriginEntityId);
                            var originField = originEntity.Fields.Single(x => x.Id == targetField.Relation.OriginFieldId);

                            //both kind of relations return only single record from other entity
                            if (targetField.Relation.RelationType == EntityRelationType.OneToOne ||
                                targetField.Relation.RelationType == EntityRelationType.OneToMany)
                            {
                                var relQuery = EntityQuery.QueryEQ(originField.Name, recValue.Value);
                                var relatedStorageRecord = recRepo.Find(originEntity.Name, relQuery, null, 0, 1).SingleOrDefault();
                                if (relatedStorageRecord == null)
                                {
                                    dataRecord[field.Name] = null;
                                    continue;
                                }

                                var relatedObject = new EntityRecord();
                                foreach (var relField in targetField.RelationFields)
                                {
                                    var relValue = relatedStorageRecord.SingleOrDefault(x => x.Key == relField.Name);
                                    relatedObject[relField.Name] = ExractFieldValue(relValue, relField);
                                }
                                dataRecord[field.Name] = relatedObject;
                            }
                            //in this case we need to retrieve multiple records from system table,
                            //which containsrecords of pair ids
                            else if (targetField.Relation.RelationType == EntityRelationType.ManyToMany)
                            {
                                if (recValue.Value == null || !(recValue.Value is Guid))
                                {
                                    dataRecord[field.Name] = null;
                                    continue;
                                }

                                
                                List<Guid> relatedRecordIds = entityRelationRepository.ReadManyToManyRecordByTarget(targetField.Relation.Id, (Guid)recValue.Value);
                                List<EntityRecord> relatedListObject = new List<EntityRecord>();
                                foreach( var id in relatedRecordIds )
                                {
                                    var relQuery = EntityQuery.QueryEQ(originField.Name, id);
                                    var relatedStorageRecord = recRepo.Find(originEntity.Name, relQuery, null, 0, 1).SingleOrDefault();
                                    //is a perfect world there should not be any related id while entity record do not exist
                                    if ( relatedStorageRecord == null )
                                        continue;

                                    var rowRecord = new EntityRecord();
                                    foreach (var relField in targetField.RelationFields)
                                    {
                                        var relValue = relatedStorageRecord.SingleOrDefault(x => x.Key == relField.Name);
                                        rowRecord[relField.Name] = ExractFieldValue(relValue, relField);
                                    }
                                    relatedListObject.Add(rowRecord);
                                }
                                dataRecord[field.Name] = relatedListObject;
                            }
                        }
                    }
                    data.Add(dataRecord);
                }

                response.Object = new QueryResult { FieldsMeta = fields, Data= data };
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

        private object ExractFieldValue(KeyValuePair<string, object>? fieldValue, Field field)
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
                    return pair.Value as decimal?;
                else if (field is PasswordField)
                    //TODO decide what to return, at the moment NULL
                    return null;
                else if (field is PercentField)
                    return pair.Value as decimal?;
                else if (field is PhoneField)
                    return pair.Value as string;
                else if (field is GuidField)
                    return (Guid)pair.Value;
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
                    throw new Exception("System error. Record primary key value is missing.");
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

                    string fieldName = relationData[0];
                    string relationFieldName = relationData[1];

                    if (string.IsNullOrWhiteSpace(fieldName))
                        throw new Exception(string.Format("Invalid query result field '{0}'. The field name is not specified.", token));

                    if (string.IsNullOrWhiteSpace(relationFieldName))
                        throw new Exception(string.Format("Invalid query result field '{0}'. The relation field name is not specified.", token));


                    Field field = result.SingleOrDefault(x => x.Name == fieldName);
                    GuidFieldMeta guidMetaField = null;
                    if (field == null)
                    {
                        field = entity.Fields.SingleOrDefault(x => x.Name == fieldName);
                        if (field == null)
                            throw new Exception(string.Format("Invalid query result field '{0}'", token));

                        //add 
                        if (field is GuidField)
                        {
                            //we add GuidFieldMeta object, and we are going to set relation and relation fields later bellow
                            guidMetaField = new GuidFieldMeta(field as GuidField);
                            result.Add(guidMetaField);
                        }
                        else
                            //any other than GuidField type is not supported
                            throw new Exception(string.Format("Invalid query field '{0}'. Specified query field used as relation is not valid.", token));
                    }
                    else
                    {
                        //if field is already added to result and it is not a GuidFildMeta, remove it and 
                        //add GuidFieldMeta object, are going to set relation and relation fields later bellow
                        if ((field is GuidField) && !(field is GuidFieldMeta))
                        {
                            result.Remove(field);
                            guidMetaField = new GuidFieldMeta(field as GuidField);
                            result.Add(guidMetaField);
                        }
                    }

                    if (guidMetaField.Relation == null)
                    {
                        guidMetaField.Relation = entityRelationManager.Read(entity.Id.Value, field.Id.Value).Object;
                        if (guidMetaField.Relation == null)
                            throw new Exception(string.Format("Invalid query field '{0}'. No relation found.", token));
                    }

                    //if field already added 
                    if (guidMetaField.RelationFields.Any(x => x.Name == relationFieldName))
                        continue;

                    var originEntity = GetEntity(guidMetaField.Relation.OriginEntityId);
                    //this should not happen in a perfect (no bugs) world
                    if (originEntity == null)
                        throw new Exception(string.Format("Invalid query result field '{0}'. Related entity is missing.", token));

                    //get and check for related field in related entity
                    var relatedField = originEntity.Fields.SingleOrDefault(x => x.Name == relationFieldName);
                    if (relatedField == null)
                        throw new Exception(string.Format("Invalid query result field '{0}'. The relation field does not exist.", token));

                    //check for duplication and add it
                    if (!guidMetaField.RelationFields.Any(x => x.Id == relatedField.Id))
                        guidMetaField.RelationFields.Add(field);
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

    }
}