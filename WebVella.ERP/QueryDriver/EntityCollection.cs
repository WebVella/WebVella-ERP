using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Storage;

namespace WebVella.ERP.QueryDriver
{
    public class EntityCollection
    {
        private const string WILDCARD_SYMBOL = "*";
        private const char PROPERTY_SEPARATOR = ',';
        private const char RELATION_PROPERTY_SEPARATOR = '.';

        private string entityName;
        private string fields;
        private EntityQuerySecurity security;

        internal EntityCollection(string entityName, string fields = "*", EntityQuerySecurity security = null )
        {
            if (string.IsNullOrWhiteSpace(entityName))
                throw new ArgumentException("Invalid entity name.");

            if (string.IsNullOrWhiteSpace(fields))
                throw new ArgumentException("Invalid result fields list.");

            this.entityName = entityName;
            this.security = security;
            this.fields = fields;
        }

        public IEnumerable<EntityQueryResultDoc> Find(EntityQueryDoc query)
        {
            return Find(query, null, -1, -1);
        }

        public IEnumerable<EntityQueryResultDoc> Find(EntityQueryDoc query, EntityQuerySortDoc sort)
        {
            return Find(query, sort, -1, -1);
        }

        public IEnumerable<EntityQueryResultDoc> Find(EntityQueryDoc query, int page, int pageSize)
        {
            return Find(query, null, page, pageSize);
        }

        public IEnumerable<EntityQueryResultDoc> Find(EntityQueryDoc query, EntityQuerySortDoc sort, int page, int pageSize)
        {
            var entityRepository = ERPService.Current.StorageService.GetEntityRepository();
            var entityQueryRepository = ERPService.Current.StorageService.GetQueryRepository();

            var entity = entityRepository.Read(entityName);
            if (entity == null)
                throw new Exception(string.Format("The entity '{0}' does not exists.", entityName));

            var processedFields = ProcessFields(entityRepository, entity, fields);

            var entityFields = processedFields[entityName];
            var result = entityQueryRepository.Execute(entityName, entityFields , query, new[] { sort }, page, pageSize);

            return null;
        }
        
        /// <summary>
        /// Processes specified field selection string and splits it to entity and field names
        /// During the procecess validates if all specified fields are present.
        /// </summary>
        /// <param name="entityRepository"></param>
        /// <param name="entity"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        private IDictionary<string, List<string>> ProcessFields(IStorageEntityRepository entityRepository, IStorageEntity entity, string fields)
        {
            IDictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            List<string> processedFields = fields.Split(PROPERTY_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            //We check for wildcard symbol and if presend include all 
            //fields of the entity in result, ignoring any other listed fields
            //in current entity. Finally we clear the list from processed field strings.
            bool wildcardSelectionEnabled = processedFields.Any(x => x == WILDCARD_SYMBOL );
            if (wildcardSelectionEnabled)
            {
                result.Add(entity.Name, entity.Fields.Select(x => x.Name).ToList());
                processedFields = processedFields.Where(x => !x.Contains(RELATION_PROPERTY_SEPARATOR) || x != WILDCARD_SYMBOL).ToList();
            }
            else
            {
                List<string> localFieldsList = new List<string>();
                var fieldNames = processedFields.Where(x => !x.Contains("."));
                foreach( var fieldName in fieldNames )
                {
                    //check field name is correct for specified entity
                    if( !entity.Fields.Any(x=>x.Name == fieldName ) )
                        throw new Exception(string.Format("Invalid query result field '{0}'. The field name is incorrect.", fieldName));

                    //skip duplicated field names
                    if (!localFieldsList.Any(x => x == fieldName))
                        localFieldsList.Add(fieldName);
                }
            }

            foreach ( var field in processedFields )
            {
                var relationData = field.Split(RELATION_PROPERTY_SEPARATOR).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                if (relationData.Count > 2)
                    throw new Exception(string.Format("The specified query result  field '{0}' is incorrect. Only first level relation can be specified.", field));

                string relationEntityFieldName = relationData[0];
                string relatedFieldName = relationData[1];

                if ( string.IsNullOrWhiteSpace(relationEntityFieldName) )
                    throw new Exception(string.Format("Invalid query result field '{0}'. The entity name is not specified.", field ));

                if (string.IsNullOrWhiteSpace(relatedFieldName))
                    throw new Exception(string.Format("Invalid query result field '{0}'. The field name is not specified.", field));

                IStorageLookupRelationField relationField = entity.Fields.SingleOrDefault(x => x.Name == relationEntityFieldName) as IStorageLookupRelationField;
                if( relationField == null )
                    throw new Exception(string.Format("Invalid query result field '{0}'", field ));

                IStorageEntity relatedEntity = entityRepository.Read(relationField.Value);
                if( relatedEntity == null )
                    throw new Exception(string.Format("Invalid query result field '{0}'. Related entity is missing.", field));

                if (relatedEntity == null)
                   throw new Exception(string.Format("Invalid query result field '{0}'. The entity '{1}' does not exist.", field, relationEntityFieldName ));

                if ( !relatedEntity.Fields.Any(x=>x.Name == relatedFieldName ))
                    throw new Exception(string.Format("Invalid query result field '{0}'. The field does not exist.", field ));

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
                var baseEntityFields = result[entityName];
                if (baseEntityFields == null)
                    baseEntityFields = new List<string>();

                if (!baseEntityFields.Any(x => x == relationEntityFieldName))
                {
                    baseEntityFields.Add(relationEntityFieldName);
                    result[entityName] = baseEntityFields;
                }
            }

            return result;
        }

                

    }

}