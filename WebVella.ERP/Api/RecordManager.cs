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

		/// <summary>
		/// The contructor
		/// </summary>
		/// <param name="service"></param>
		public RecordManager(IERPService service)
		{
			erpService = service;
			entityCache = new List<Entity>();
			entityManager = new EntityManager(erpService.StorageService);
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
                response.Errors.Add(new ErrorModel { Message="Invalid entity name." });
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

            if( entity == null )
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

            var fieldsMeta = ExtractEntityFieldsMeta(entity);
            var recordFields = record.GetProperties();
            foreach (var field in fieldsMeta )
            {
                var pair = recordFields.SingleOrDefault(x => x.Key == field.Name);
                storageRecordData.Add(new KeyValuePair<string, object>(field.Name, ExractFieldValue(pair, field) ));
            }

            var recRep = erpService.StorageService.GetRecordRepository();
            recRep.Create(entity.Name, storageRecordData);
            return null;
        }


		/// <summary>
		/// Execute search and returns records matching specified query parameters
		/// </summary>
		/// <param name="query"></param>
		/// <param name="security"></param>
		/// <returns></returns>
		public QueryResponse Find(EntityQuery query, QuerySecurity security = null)
		{
			QueryResponse response = new QueryResponse
			{
				Success = true,
				Message = "The entity was successfully created!",
			};

			try
			{
				List<Field> fieldsMeta = ExtractQueryFieldsMeta(query);
				var recRepo = erpService.StorageService.GetRecordRepository();
				var storageRecords = recRepo.Find(query.EntityName, query.Query, query.Sort, query.Skip, query.Limit);

				List<EntityRecord> result = new List<EntityRecord>();

				foreach (var record in storageRecords)
				{
					var resultRecord = new EntityRecord();
					foreach (var field in fieldsMeta)
					{
						if (((IFieldMeta)field).ParentFieldName == null)
						{
							var pair = record.SingleOrDefault(x => x.Key == field.Name);
							resultRecord[field.Name] = ExractFieldValue(pair, field);
						}
						else //relation field
						{
							var relationCompositeFieldName = ((IFieldMeta)field).ParentFieldName + RELATION_NAME_RESULT_SEPARATOR + field.Name;
							var parentField = record.SingleOrDefault(x => x.Key == (((IFieldMeta)field).ParentFieldName));
							if (parentField.Value == null)
								resultRecord[relationCompositeFieldName] = null;
							else
							{
								var childRecord = recRepo.Find(((IFieldMeta)field).EntityName, (Guid)parentField.Value);
								if (childRecord == null)
									resultRecord[relationCompositeFieldName] = null;
								else
								{
									var pair = childRecord.SingleOrDefault(x => x.Key == field.Name);
									resultRecord[relationCompositeFieldName] = ExractFieldValue(pair, field);
								}
							}

						}
					}
				}
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
			if (fieldValue != null && fieldValue.Value.Key != null )
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
				else if (field is PrimaryKeyField)
					return (Guid)pair.Value;
				else if (field is SelectField)
					return pair.Value as string;
				else if (field is TextField)
					return pair.Value as string;
				else if (field is UrlField)
					return pair.Value as string;
				else if (field is LookupRelationField)
					return (Guid?)pair.Value;
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
                    if (((DateField)field).UseCurrentTimeAsDefaultValue)
                        return DateTime.UtcNow.Date;
                    else
                        return ((DateField)field).DefaultValue;
                }
                else if (field is DateTimeField)
                {
                    if (((DateTimeField)field).UseCurrentTimeAsDefaultValue)
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
                else if (field is PrimaryKeyField)
                    throw new Exception("System error. Record primary key value is missing.");
                else if (field is SelectField)
                    return ((SelectField)field).DefaultValue;
                else if (field is TextField)
                    return ((TextField)field).DefaultValue;
                else if (field is UrlField)
                    return ((UrlField)field).DefaultValue;
                else if (field is LookupRelationField)
                    return null;
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
				result.AddRange(entity.Fields.Select(x => WrapFieldMeta(x, entity)));
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
						result.Add(WrapFieldMeta(field, entity));
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

					Field field = entity.Fields.SingleOrDefault(x => x.Name == fieldName);
					if (field == null)
					{
						throw new Exception(string.Format("Invalid query result field '{0}'", token));
					}
					else if (field is LookupRelationField)
					{
						Guid? relatedEntityId = (field as LookupRelationField).RelatedEntityId;

						//there is always value 
						var relatedEntity = GetEntity(relatedEntityId.Value);
						if (relatedEntity == null)
							throw new Exception(string.Format("Invalid query result field '{0}'. Related entity is missing.", token));

						//get and check for related field in related entity
						var relatedField = relatedEntity.Fields.SingleOrDefault(x => x.Name == relationFieldName);
						if (relatedField == null)
							throw new Exception(string.Format("Invalid query result field '{0}'. The relation field does not exist.", token));

						//skip duplication
						if (!result.Any(x => x.Id == relatedField.Id))
							result.Add(WrapFieldMeta(field, relatedEntity, fieldName));

					}
					else
					{
						//any other than LookupRelationField type is not supported
						throw new Exception(string.Format("Invalid query field '{0}'. Sepcified query field used as relation is not supported.", token));
					}
				}
			}

			return result;
		}

        private List<Field> ExtractEntityFieldsMeta(Entity entity)
        {
            List<Field> result = new List<Field>();
            result.AddRange(entity.Fields.Select(x => WrapFieldMeta(x, entity)));
            return result;
        }

        private Field WrapFieldMeta(Field field, Entity entity, string parentFieldName = null)
		{
			if (field is AutoNumberField)
				return new AutoNumberFieldMeta(entity.Id.Value, entity.Name, (AutoNumberField)field, parentFieldName);
			else if (field is CheckboxField)
				return new CheckboxFieldMeta(entity.Id.Value, entity.Name, (CheckboxField)field, parentFieldName);
			else if (field is CurrencyField)
				return new CurrencyFieldMeta(entity.Id.Value, entity.Name, (CurrencyField)field, parentFieldName);
			else if (field is DateField)
				return new DateFieldMeta(entity.Id.Value, entity.Name, (DateField)field, parentFieldName);
			else if (field is DateTimeField)
				return new DateTimeFieldMeta(entity.Id.Value, entity.Name, (DateTimeField)field, parentFieldName);
			else if (field is EmailField)
				return new EmailFieldMeta(entity.Id.Value, entity.Name, (EmailField)field, parentFieldName);
			else if (field is FileField)
				return new FileFieldMeta(entity.Id.Value, entity.Name, (FileField)field, parentFieldName);
			else if (field is HtmlField)
				return new HtmlFieldMeta(entity.Id.Value, entity.Name, (HtmlField)field, parentFieldName);
			else if (field is MultiLineTextField)
				return new MultiLineTextFieldMeta(entity.Id.Value, entity.Name, (MultiLineTextField)field, parentFieldName);
			else if (field is MultiSelectField)
				return new MultiSelectFieldMeta(entity.Id.Value, entity.Name, (MultiSelectField)field, parentFieldName);
			else if (field is NumberField)
				return new NumberFieldMeta(entity.Id.Value, entity.Name, (NumberField)field, parentFieldName);
			else if (field is PasswordField)
				return new PasswordFieldMeta(entity.Id.Value, entity.Name, (PasswordField)field, parentFieldName);
			else if (field is PercentField)
				return new PercentFieldMeta(entity.Id.Value, entity.Name, (PercentField)field, parentFieldName);
			else if (field is PhoneField)
				return new PhoneFieldMeta(entity.Id.Value, entity.Name, (PhoneField)field, parentFieldName);
			else if (field is PrimaryKeyField)
				return new PrimaryKeyFieldMeta(entity.Id.Value, entity.Name, (PrimaryKeyField)field, parentFieldName);
			else if (field is SelectField)
				return new SelectFieldMeta(entity.Id.Value, entity.Name, (SelectField)field, parentFieldName);
			else if (field is TextField)
				return new TextFieldMeta(entity.Id.Value, entity.Name, (TextField)field, parentFieldName);
			else if (field is UrlField)
				return new UrlFieldMeta(entity.Id.Value, entity.Name, (UrlField)field, parentFieldName);
			else if (field is LookupRelationField)
				return new LookupRelationFieldMeta(entity.Id.Value, entity.Name, (LookupRelationField)field, parentFieldName);

			throw new Exception("Not supported field type met.");
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