using System;

namespace WebVella.ERP.QueryDriver
{
    public class EntityDatabase
    {
        public static EntityCollection GetCollection(string entityName)
        {
            //TODO validate
            return new EntityCollection(entityName);
        }
    }
}