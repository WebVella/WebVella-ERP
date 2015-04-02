using System;
using System.Collections.Generic;

namespace WebVella.ERP
{ 
    public class EntityQueryBuilder
    {
        public string EntityName { get; private set; }

        public EntityQueryBuilder(string entityName )
        {
            if (string.IsNullOrWhiteSpace(entityName))
                throw new ArgumentNullException("entityName");

            EntityName = entityName;
        }
        public EntityQueryDoc EQ( string fieldName, object value )
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentNullException("fieldName");

           return new EntityQueryDoc { QueryType = EntityQueryType.EQ, FieldName = fieldName, FieldValue = value };
        }

        public EntityQueryDoc NOT(string fieldName, object value)
        {
            return new EntityQueryDoc { QueryType = EntityQueryType.NOT, FieldName = fieldName, FieldValue = value };
        }

        public EntityQueryDoc LT(string fieldName, object value)
        {
            return new EntityQueryDoc { QueryType = EntityQueryType.LT, FieldName = fieldName, FieldValue = value };
        }

        public EntityQueryDoc LTE(string fieldName, object value)
        {
            return new EntityQueryDoc { QueryType = EntityQueryType.LTE, FieldName = fieldName, FieldValue = value };
        }

        public EntityQueryDoc GT(string fieldName, object value)
        {
            return new EntityQueryDoc { QueryType = EntityQueryType.GT, FieldName = fieldName, FieldValue = value };
        }

        public EntityQueryDoc GTE(string fieldName, object value)
        {
            return new EntityQueryDoc { QueryType = EntityQueryType.GTE, FieldName = fieldName, FieldValue = value };
        }

        public EntityQueryDoc AND( params EntityQueryDoc[] queries )
        {
            foreach ( var query in queries )
            {
                if (query == null)
                    throw new ArgumentException("Queries contains null values.");
            }

            return new EntityQueryDoc { QueryType = EntityQueryType.AND, SubQueries = new List<EntityQueryDoc>(queries) };
        }

        public EntityQueryDoc OR( params EntityQueryDoc[] queries )
        {
            foreach (var query in queries)
            {
                if (query == null)
                    throw new ArgumentException("Queries contains null values.");
            }

            return new EntityQueryDoc { QueryType = EntityQueryType.OR, SubQueries = new List<EntityQueryDoc>(queries) };
        }
    }
}
