using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api
{
	public class RecordManager
	{
		private ERPService erpService;
		private const string RECORD_COLLECTION_PREFIX = "ev_";
		private const string ID_FIELD_NAME = "Id";
		private const string WILDCARD_SYMBOL = "*";
		private const char FIELDS_SEPARATOR = ',';
		private const char RELATION_SEPARATOR = '.';
		private List<Entity> entityCache;
		private EntityManager entityManager;

		/// <summary>
		/// The contructor
		/// </summary>
		/// <param name="service"></param>
		public RecordManager(ERPService service)
		{
			erpService = service;
			entityCache = new List<Entity>();
			entityManager = new EntityManager(erpService.StorageService);
		}

		/// <summary>
		/// Execute search and returns records matching specified query parameters
		/// </summary>
		/// <param name="query"></param>
		/// <param name="security"></param>
		/// <returns></returns>
		public IEnumerable<EntityRecord> Find(EntityQuery query, QuerySecurity security = null)
		{
			List<QueryResultFieldRecordMeta> recordMetas = ProcessQueryFieldsMeta(query);
			var errep = ERPService.Current.StorageService.GetRecordRepository();
			var storageRecords = errep.Find(RECORD_COLLECTION_PREFIX + query.EntityName, query.Query, query.Sort, query.Skip, query.Limit);

			//we need specified entity fields initially and then we will process relation fields
			var entityFieldMetas = recordMetas.Where(x => x.Entity.Name == query.EntityName);
			//we select only relation fields and order them in order to speedup the process later
			var relatedEntityFieldMetas = recordMetas.Where(x => x.Entity.Name != query.EntityName).OrderBy(x => x.Entity.Name);

			List<EntityRecord> result = new List<EntityRecord>();

			foreach (var record in storageRecords)
			{
				var resultRecord = new EntityRecord();
				foreach (var meta in entityFieldMetas)
				{
					if (record.Any(x => x.Key == meta.Field.Name))
					{
						#region <--- The field value exists
						var pair = record.Single(x => x.Key == meta.Field.Name);

						if (meta.Field is AutoNumberField)
						{
							AutoNumberField field = (AutoNumberField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = pair.Value as decimal?, Format = field.DisplayFormat };
						}
						else if (meta.Field is CheckboxField)
						{
							CheckboxField field = (CheckboxField)meta.Field;
							resultRecord[meta.Field.Name] = pair.Value as bool?;
						}
						else if (meta.Field is CurrencyField)
						{
							CurrencyField field = (CurrencyField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = pair.Value as decimal?, Currency = field.Currency };
						}
						else if (meta.Field is DateField)
						{
							DateField field = (DateField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = pair.Value as DateTime?, Format = field.Format };
						}
						else if (meta.Field is DateTimeField)
						{
							DateTimeField field = (DateTimeField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = pair.Value as DateTime?, Format = field.Format };
						}
						else if (meta.Field is EmailField)
						{
							EmailField field = (EmailField)meta.Field;
							resultRecord[meta.Field.Name] = pair.Value as string;
						}
						else if (meta.Field is FileField)
						{
							//TODO convert file path to url path
							FileField field = (FileField)meta.Field;
							resultRecord[meta.Field.Name] = pair.Value as string;
						}
						else if (meta.Field is HtmlField)
						{
							HtmlField field = (HtmlField)meta.Field;
							resultRecord[meta.Field.Name] = pair.Value as string;
						}
						else if (meta.Field is MultiLineTextField)
						{
							MultiLineTextField field = (MultiLineTextField)meta.Field;
							resultRecord[meta.Field.Name] = pair.Value as string;
						}
						else if (meta.Field is MultiSelectField)
						{
							MultiSelectField field = (MultiSelectField)meta.Field;
							resultRecord[meta.Field.Name] = pair.Value as IEnumerable<string>;
						}
						else if (meta.Field is NumberField)
						{
							NumberField field = (NumberField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = pair.Value as decimal?, DecimalPlaces = field.DecimalPlaces };
						}
						else if (meta.Field is PasswordField)
						{
							//at the moment password field is only encoded and no default value
							PasswordField field = (PasswordField)meta.Field;
							resultRecord[meta.Field.Name] = null;
						}
						else if (meta.Field is PercentField)
						{
							PercentField field = (PercentField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = pair.Value as decimal?, DecimalPlaces = field.DecimalPlaces };
						}
						else if (meta.Field is PhoneField)
						{
							PhoneField field = (PhoneField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = pair.Value as string, Format = field.Format };
						}
						else if (meta.Field is PrimaryKeyField)
						{
							PrimaryKeyField field = (PrimaryKeyField)meta.Field;
							resultRecord[meta.Field.Name] = (Guid)pair.Value;
						}
						else if (meta.Field is SelectField)
						{
							SelectField field = (SelectField)meta.Field;
							resultRecord[meta.Field.Name] = pair.Value as string;
						}
						else if (meta.Field is TextField)
						{
							TextField field = (TextField)meta.Field;
							resultRecord[meta.Field.Name] = pair.Value as string;
						}
						else if (meta.Field is UrlField)
						{
							UrlField field = (UrlField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = pair.Value as string, OpenTargetInNewWindow = field.OpenTargetInNewWindow };
						}
						else if (meta.Field is LookupRelationField)
						{
							LookupRelationField lookupFieldMeta = (LookupRelationField)meta.Field;

							var relationFields = relatedEntityFieldMetas.Where(x => x.Entity.Id == lookupFieldMeta.RelatedEntityId);
							//if there are any field different by ID field, then process it
							if (relationFields.Any(x => x.Field.Name != ID_FIELD_NAME))
							{

							}
							else
							{
								resultRecord[meta.Field.Name] = (Guid?)pair.Value;
							}
						}
						else if (meta.Field is MasterDetailsRelationshipField)
						{
							resultRecord[meta.Field.Name] = (Guid?)pair.Value;
						}

						#endregion
					}
					else
					{
						#region <--- the field value doesn't exist. Set defaults from meta
						if (meta.Field is AutoNumberField)
						{
							AutoNumberField field = (AutoNumberField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = field.DefaultValue, Format = field.DisplayFormat };
						}
						else if (meta.Field is CheckboxField)
						{
							CheckboxField field = (CheckboxField)meta.Field;
							resultRecord[meta.Field.Name] = field.DefaultValue;
						}
						else if (meta.Field is CurrencyField)
						{
							CurrencyField field = (CurrencyField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = field.DefaultValue, Currency = field.Currency };
						}
						else if (meta.Field is DateField)
						{
							DateField field = (DateField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = field.DefaultValue, Format = field.Format };
						}
						else if (meta.Field is DateTimeField)
						{
							DateTimeField field = (DateTimeField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = field.DefaultValue, Format = field.Format };
						}
						else if (meta.Field is EmailField)
						{
							EmailField field = (EmailField)meta.Field;
							resultRecord[meta.Field.Name] = field.DefaultValue;
						}
						else if (meta.Field is FileField)
						{
							FileField field = (FileField)meta.Field;
							resultRecord[meta.Field.Name] = field.DefaultValue;
						}
						else if (meta.Field is HtmlField)
						{
							HtmlField field = (HtmlField)meta.Field;
							resultRecord[meta.Field.Name] = field.DefaultValue;
						}
						else if (meta.Field is MultiLineTextField)
						{
							MultiLineTextField field = (MultiLineTextField)meta.Field;
							resultRecord[meta.Field.Name] = field.DefaultValue;
						}
						else if (meta.Field is MultiSelectField)
						{
							MultiSelectField field = (MultiSelectField)meta.Field;
							resultRecord[meta.Field.Name] = field.DefaultValue;
						}
						else if (meta.Field is NumberField)
						{
							NumberField field = (NumberField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = field.DefaultValue, DecimalPlaces = field.DecimalPlaces };
						}
						else if (meta.Field is PasswordField)
						{
							//at the moment password field is only encoded and no default value
							PasswordField field = (PasswordField)meta.Field;
							resultRecord[meta.Field.Name] = null;
						}
						else if (meta.Field is PercentField)
						{
							PercentField field = (PercentField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = field.DefaultValue, DecimalPlaces = field.DecimalPlaces };
						}
						else if (meta.Field is PhoneField)
						{
							PhoneField field = (PhoneField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = field.DefaultValue, Format = field.Format };
						}
						else if (meta.Field is PrimaryKeyField)
						{
							PrimaryKeyField field = (PrimaryKeyField)meta.Field;
							resultRecord[meta.Field.Name] = field.DefaultValue;
						}
						else if (meta.Field is SelectField)
						{
							SelectField field = (SelectField)meta.Field;
							resultRecord[meta.Field.Name] = field.DefaultValue;
						}
						else if (meta.Field is TextField)
						{
							TextField field = (TextField)meta.Field;
							resultRecord[meta.Field.Name] = field.DefaultValue;
						}
						else if (meta.Field is UrlField)
						{
							UrlField field = (UrlField)meta.Field;
							resultRecord[meta.Field.Name] = new { Value = field.DefaultValue, OpenTargetInNewWindow = field.OpenTargetInNewWindow };
						}
						else if (meta.Field is LookupRelationField)
							resultRecord[meta.Field.Name] = null;
						else if (meta.Field is MasterDetailsRelationshipField)
							resultRecord[meta.Field.Name] = null;
						#endregion
					}
				}
			}

			return null;
		}

		private object CombineFieldMetaAndValue(Field fieldmeta, object value, bool useDefaultValue = false)
		{
			if (fieldmeta is AutoNumberField)
			{
				AutoNumberField field = (AutoNumberField)fieldmeta;
				return new { Value = value as decimal?, Format = field.DisplayFormat };
			}
			else if (fieldmeta is CheckboxField)
			{
				CheckboxField field = (CheckboxField)fieldmeta;
				return value as bool?;
			}
			else if (fieldmeta is CurrencyField)
			{
				CurrencyField field = (CurrencyField)fieldmeta;
				return new { Value = value as decimal?, Currency = field.Currency };
			}
			else if (fieldmeta is DateField)
			{
				DateField field = (DateField)fieldmeta;
				return new { Value = value as DateTime?, Format = field.Format };
			}
			else if (fieldmeta is DateTimeField)
			{
				DateTimeField field = (DateTimeField)fieldmeta;
				return new { Value = value as DateTime?, Format = field.Format };
			}
			else if (fieldmeta is EmailField)
			{
				EmailField field = (EmailField)fieldmeta;
				return value as string;
			}
			else if (fieldmeta is FileField)
			{
				//TODO convert file path to url path
				FileField field = (FileField)fieldmeta;
				return value as string;
			}
			else if (fieldmeta is HtmlField)
			{
				HtmlField field = (HtmlField)fieldmeta;
				return value as string;
			}
			else if (fieldmeta is MultiLineTextField)
			{
				MultiLineTextField field = (MultiLineTextField)fieldmeta;
				return value as string;
			}
			else if (fieldmeta is MultiSelectField)
			{
				MultiSelectField field = (MultiSelectField)fieldmeta;
				return value as IEnumerable<string>;
			}
			else if (fieldmeta is NumberField)
			{
				NumberField field = (NumberField)fieldmeta;
				return new { Value = value as decimal?, DecimalPlaces = field.DecimalPlaces };
			}
			else if (fieldmeta is PasswordField)
			{
				//at the moment password field is only encoded and no default value
				PasswordField field = (PasswordField)fieldmeta;
				return null;
			}
			else if (fieldmeta is PercentField)
			{
				PercentField field = (PercentField)fieldmeta;
				return new { Value = value as decimal?, DecimalPlaces = field.DecimalPlaces };
			}
			else if (fieldmeta is PhoneField)
			{
				PhoneField field = (PhoneField)fieldmeta;
				return new { Value = value as string, Format = field.Format };
			}
			else if (fieldmeta is PrimaryKeyField)
			{
				PrimaryKeyField field = (PrimaryKeyField)fieldmeta;
				return (Guid)value;
			}
			else if (fieldmeta is SelectField)
			{
				SelectField field = (SelectField)fieldmeta;
				return value as string;
			}
			else if (fieldmeta is TextField)
			{
				TextField field = (TextField)fieldmeta;
				return value as string;
			}
			else if (fieldmeta is UrlField)
			{
				UrlField field = (UrlField)fieldmeta;
				return new { Value = value as string, OpenTargetInNewWindow = field.OpenTargetInNewWindow };
			}
			else if (fieldmeta is LookupRelationField)
			{
				LookupRelationField lookupFieldMeta = (LookupRelationField)fieldmeta;
				return (Guid?)value;
			}
			else if (fieldmeta is MasterDetailsRelationshipField)
			{
				return (Guid?)value;
			}

			throw new Exception("System error. Unsupported field type during process of query result.");

		}

		private List<QueryResultFieldRecordMeta> ProcessQueryFieldsMeta(EntityQuery query)
		{
			List<QueryResultFieldRecordMeta> result = new List<QueryResultFieldRecordMeta>();

			//split field string into tokens speparated by FIELDS_SEPARATOR
			List<string> tokens = query.Fields.Split(FIELDS_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

			//get entity by name
			Entity entity = entityManager.ReadEntity(query.EntityName).Object;

			if (entity == null)
				throw new Exception(string.Format("The entity '{0}' does not exists.", query.EntityName));

			if (!entityCache.Any(x => x.Id == entity.Id))
				entityCache.Add(entity);

			//We check for wildcard symbol and if present include all 
			//fields of the queried entity in result, ignoring any other listed fields
			//in it. Finally we clear the list from processed field tokens.
			bool wildcardSelectionEnabled = tokens.Any(x => x == WILDCARD_SYMBOL);
			if (wildcardSelectionEnabled)
			{
				//add all field from queried entity in result
				result.AddRange(entity.Fields.Select(x => new QueryResultFieldRecordMeta { Entity = entity, Field = x }).ToList());

				//remove wildcard and all other tokens related to queried entity
				//by selecting only fields to related entities if any
				tokens = tokens.Where(x => !x.Contains(RELATION_SEPARATOR) || x != WILDCARD_SYMBOL).ToList();
			}
			else
			{
				//process only tokens do not contain RELATION_SEPARATOR 
				foreach (var token in tokens.Where(x => !x.Contains(RELATION_SEPARATOR)))
				{
					//locate the field
					var field = entity.Fields.SingleOrDefault(x => x.Name == token);

					//found no field for specified token
					if (field == null)
						throw new Exception(string.Format("Invalid query result field '{0}'. The field name is incorrect.", token));

					//check for duplicated field names and ignore them
					if (!result.Any(x => x.Field == field && x.Entity == entity))
						result.Add(new QueryResultFieldRecordMeta { Entity = entity, Field = field });
				}

				//check the result for missing ID field
				//the ID field should always be part of result record even not specified
				if (!result.Any(x => x.Field.Name == ID_FIELD_NAME))
					result.Add(new QueryResultFieldRecordMeta { Entity = entity, Field = entity.Fields.Single(x => x.Name == ID_FIELD_NAME) });
			}

			//for every of the left token we know it is a relation field
			foreach (var token in tokens)
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
					if (!result.Any(x => x.Entity.Id == relatedEntity.Id && x.Field.Id == relatedField.Id))
						result.Add(new QueryResultFieldRecordMeta { Entity = relatedEntity, Field = relatedField });

					//check and include ID field, if not specified
					//the ID field of any related entity record,
					//should always be part of result record even not specified
					if (!result.Any(x => x.Entity.Id == relatedEntity.Id && x.Field.Name == ID_FIELD_NAME))
					{
						var idField = relatedEntity.Fields.Single(x => x.Name == ID_FIELD_NAME);
						result.Add(new QueryResultFieldRecordMeta { Entity = relatedEntity, Field = idField });
					}
				}
				else if (field is MasterDetailsRelationshipField)
				{
					throw new Exception(string.Format("Invalid query result field '{0}'. Relation to field type MasterDetailsRelationshipField is not supported.", token));
				}
				else
				{
					throw new Exception(string.Format("Invalid query field '{0}'. Non relation field is used as relation field.", token));
				}
			}

			return result;
		}

		private class QueryResultFieldRecordMeta
		{
			public Entity Entity { get; set; }
			public Field Field { get; set; }
		}

		private Entity GetEntity(string entityName)
		{
			var entity = entityCache.SingleOrDefault(x => x.Name == entityName);
			if (entity == null)
			{
				//TODO switch on when read by name is ready
				//entity = entityManager.Read(entityName).Object;
				entity = null;

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

		private List<Field> ExtractQueryFieldsMeta(EntityQuery query)
		{
			List<Field> result = new List<Field>();

			//split field string into tokens speparated by FIELDS_SEPARATOR
			List<string> tokens = query.Fields.Split(FIELDS_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

			//check the query tokens for widcard symbol and validate it is only that symbol
			if (tokens.Count > 1 && tokens.Any(x => x == WILDCARD_SYMBOL))
				throw new Exception("Invalid query syntax. Wildcard symbol can be used only with no other fields.");

			//get entity by name
			Entity entity = entityManager.ReadEntity(query.EntityName).Object;

			if (entity == null)
				throw new Exception(string.Format("The entity '{0}' does not exists.", query.EntityName));

			if (!entityCache.Any(x => x.Id == entity.Id))
				entityCache.Add(entity);

			//We check for wildcard symbol and if present include all fields of the queried entity 
			bool wildcardSelectionEnabled = tokens.Any(x => x == WILDCARD_SYMBOL);
			if (wildcardSelectionEnabled)
			{
				result.AddRange(entity.Fields.Select(x => WrapFieldMeta(x, entity)));
				return result;
			}
			
			//process only tokens do not contain RELATION_SEPARATOR 
			foreach (var token in tokens )
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
							result.Add(WrapFieldMeta(field,relatedEntity));
						
					}
					else if (field is MasterDetailsRelationshipField)
					{
						throw new Exception(string.Format("Invalid query result field '{0}'. Relation to field type MasterDetailsRelationshipField is not supported.", token));
					}
					else
					{
						throw new Exception(string.Format("Invalid query field '{0}'. Non relation field is used as relation field.", token));
					}
				}
			}

			return result;
		}

		private Field WrapFieldMeta(Field field, Entity entity)
		{
			return field;
		}

	}
}