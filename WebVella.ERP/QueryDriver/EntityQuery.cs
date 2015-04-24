using System;
using System.Collections.Generic;

namespace WebVella.ERP.QueryDriver
{
    public static class EntityQuery
    {
        public static EntityQueryDoc EQ(string fieldName, object value)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentNullException("fieldName");

            return new EntityQueryDoc { QueryType = EntityQueryType.EQ, FieldName = fieldName, FieldValue = value };
        }
		
        public static EntityQueryDoc NOT(string fieldName, object value)
        { 
            return new EntityQueryDoc { QueryType = EntityQueryType.NOT, FieldName = fieldName, FieldValue = value };
        }

        public static EntityQueryDoc LT(string fieldName, object value)
        {
            return new EntityQueryDoc { QueryType = EntityQueryType.LT, FieldName = fieldName, FieldValue = value };
        }

        public static EntityQueryDoc LTE(string fieldName, object value)
        {
            return new EntityQueryDoc { QueryType = EntityQueryType.LTE, FieldName = fieldName, FieldValue = value };
        }

        public static EntityQueryDoc GT(string fieldName, object value)
        {
            return new EntityQueryDoc { QueryType = EntityQueryType.GT, FieldName = fieldName, FieldValue = value };
        }

        public static EntityQueryDoc GTE(string fieldName, object value)
        {
            return new EntityQueryDoc { QueryType = EntityQueryType.GTE, FieldName = fieldName, FieldValue = value };
        }

        public static EntityQueryDoc AND(params EntityQueryDoc[] queries)
        {
            foreach (var query in queries)
            {
                if (query == null)
                    throw new ArgumentException("Queries contains null values.");
            }

            return new EntityQueryDoc { QueryType = EntityQueryType.AND, SubQueries = new List<EntityQueryDoc>(queries) };
        }

        public static EntityQueryDoc OR(params EntityQueryDoc[] queries)
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
