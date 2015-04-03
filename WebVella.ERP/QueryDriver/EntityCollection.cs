using System;

namespace WebVella.ERP.QueryDriver
{
    public class EntityCollection
    {
        public string Name { get; private set; }

        internal EntityCollection(string name)
        {
            Name = name;
        }

        public object Find(EntityQueryDoc query)
        {
            return Find(query, null, -1, -1);
        }

        public object Find(EntityQueryDoc query, EntityQuerySortDoc sort)
        {
            return Find(query, sort, -1, -1);
        }

        public object Find(EntityQueryDoc query, EntityQuerySortDoc sort, int page, int pageSize)
        {
            return null;
        }
        
        public object Find(EntityQueryDoc query, int page, int pageSize)
        {
            return Find(query, null, page, pageSize);
        }

    }

}