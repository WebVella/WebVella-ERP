using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api
{
	public class RecordManager
	{
		private const string RECORD_COLLECTION_PREFIX = "ev_";
		private const string WILDCARD_SYMBOL = "*";
		private const char FIELDS_SEPARATOR = ',';
		private const char RELATION_SEPARATOR = '.';

		/// <summary>
		/// Execute search and returns records matching specified query parameters
		/// </summary>
		/// <param name="query"></param>
		/// <param name="security"></param>
		/// <returns></returns>
		public IEnumerable<EntityRecord> Find( EntityQuery query, QuerySecurity security = null )
		{
			var entityRepository = ERPService.Current.StorageService.GetEntityRepository();

			var entity = entityRepository.Read(query.EntityName);
			if (entity == null)
				throw new Exception(string.Format("The entity '{0}' does not exists.", query.EntityName));

			var processedFields = ProcessFields(entityRepository, entity, query);
			var entityRecordRepository = ERPService.Current.StorageService.GetRecordRepository();
			var storageRecords = entityRecordRepository.Find(RECORD_COLLECTION_PREFIX + query.EntityName, query.Query, query.Sort, query.Skip, query.Limit);

			foreach( var record in storageRecords )
			{
				//record.FirstOrDefault(x=>x.Key == )
			}
			var entityFields = processedFields[query.EntityName];
		

			return null;
		}

		/// <summary>
		/// Processes specified field selection string and splits it to entity and field names
		/// During the process validates if all specified fields are present.
		/// </summary>
		/// <param name="entityRepository"></param>s
		/// <param name="entity"></param>
		/// <param name="query"></param>
		/// <returns></returns>
		private IDictionary<string, List<string>> ProcessFields(IStorageEntityRepository entityRepository, IStorageEntity entity, EntityQuery query)
		{
			IDictionary<string, List<string>> result = new Dictionary<string, List<string>>();
			List<string> processedFields = query.Fields.Split(FIELDS_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

			//We check for wildcard symbol and if presend include all 
			//fields of the entity in result, ignoring any other listed fields
			//in current entity. Finally we clear the list from processed field strings.
			bool wildcardSelectionEnabled = processedFields.Any(x => x == WILDCARD_SYMBOL);
			if (wildcardSelectionEnabled)
			{
				result.Add(entity.Name, entity.Fields.Select(x => x.Name).ToList());
				processedFields = processedFields.Where(x => !x.Contains(RELATION_SEPARATOR) || x != WILDCARD_SYMBOL).ToList();
			}
			else
			{
				List<string> localFieldsList = new List<string>();
				var fieldNames = processedFields.Where(x => !x.Contains("."));
				foreach (var fieldName in fieldNames)
				{
					//check field name is correct for specified entity
					if (!entity.Fields.Any(x => x.Name == fieldName))
						throw new Exception(string.Format("Invalid query result field '{0}'. The field name is incorrect.", fieldName));

					//skip duplicated field names
					if (!localFieldsList.Any(x => x == fieldName))
						localFieldsList.Add(fieldName);
				}

				//if no local fields found or ID is missing add it
				if (!localFieldsList.Any(x => x == "Id"))
					localFieldsList.Add("Id");
			}

			foreach (var field in processedFields)
			{
				var relationData = field.Split(RELATION_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
				if (relationData.Count > 2)
					throw new Exception(string.Format("The specified query result  field '{0}' is incorrect. Only first level relation can be specified.", field));

				string relationEntityFieldName = relationData[0];
				string relatedFieldName = relationData[1];

				if (string.IsNullOrWhiteSpace(relationEntityFieldName))
					throw new Exception(string.Format("Invalid query result field '{0}'. The entity name is not specified.", field));

				if (string.IsNullOrWhiteSpace(relatedFieldName))
					throw new Exception(string.Format("Invalid query result field '{0}'. The field name is not specified.", field));

				IStorageLookupRelationField relationField = entity.Fields.SingleOrDefault(x => x.Name == relationEntityFieldName) as IStorageLookupRelationField;
				if (relationField == null)
					throw new Exception(string.Format("Invalid query result field '{0}'", field));

				IStorageEntity relatedEntity = entityRepository.Read(relationField.RelatedEntityId);
				if (relatedEntity == null)
					throw new Exception(string.Format("Invalid query result field '{0}'. Related entity is missing.", field));

				if (relatedEntity == null)
					throw new Exception(string.Format("Invalid query result field '{0}'. The entity '{1}' does not exist.", field, relationEntityFieldName));

				if (!relatedEntity.Fields.Any(x => x.Name == relatedFieldName))
					throw new Exception(string.Format("Invalid query result field '{0}'. The field does not exist.", field));

				//if first field for the relation then create list
				var relatedEntityFields = result[relationEntityFieldName];
				if (relatedEntityFields == null)
					relatedEntityFields = new List<string>();

				//if field is not already added, then add it
				if (!relatedEntityFields.Any(x => x == relatedFieldName))
					relatedEntityFields.Add(relatedFieldName);

				result[relationEntityFieldName] = relatedEntityFields;

				//check if queried entity fields include relation field
				//this is needed because later we need the ID to query related entity
				var baseEntityFields = result[entity.Name];
				if (baseEntityFields == null)
					baseEntityFields = new List<string>();

				if (!baseEntityFields.Any(x => x == relationEntityFieldName))
				{
					baseEntityFields.Add(relationEntityFieldName);
					result[entity.Name] = baseEntityFields;
				}
			}

			return result;
		}
	}
}