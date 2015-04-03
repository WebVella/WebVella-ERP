using System;
using System.Collections.Generic;

namespace WebVella.ERP.QueryDriver
{
    public class EntityQueryDoc 
    {
        internal EntityQueryType QueryType { get; set; }
        public string FieldName { get; set; }
        public object FieldValue { get; set; }
        public List<EntityQueryDoc> SubQueries { get; set; }
    }
}