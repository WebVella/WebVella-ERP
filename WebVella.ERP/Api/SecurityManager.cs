using System;
using System.Linq;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Api.Models.AutoMapper;

namespace WebVella.ERP.Api
{
    public class SecurityManager
    {
        IErpService service;

        const string fieldsToQuery = @"id,email,password,first_name,last_name,created_on,created_by,
                                            last_modified_on,last_modified_by, last_logged_in,enabled, 
                                            $user_role.id, $user_role.name";

        
        public SecurityManager(IErpService service)
        {
            this.service = service;
        }

        public ErpUser GetUser(Guid userId)
        {
            RecordManager recMan = new RecordManager(service, true);
            EntityQuery query = new EntityQuery("user", fieldsToQuery, EntityQuery.QueryEQ("id", userId), null, null, null);
            var result = recMan.Find(query);
            if (!result.Success)
                throw new Exception(result.Message);

            if (!result.Object.Data.Any())
                return null;

            var record = result.Object.Data.Single();
            return record.DynamicMapTo<ErpUser>();

        }

        public ErpUser GetUser(string email)
        {
            RecordManager recMan = new RecordManager(service, true);
            EntityQuery query = new EntityQuery("user", fieldsToQuery, EntityQuery.QueryEQ("email", email), null, null, null);
            var result = recMan.Find(query);
            if (!result.Success)
                throw new Exception(result.Message);

            if (!result.Object.Data.Any())
                return null;

            var record = result.Object.Data.Single();
            return record.DynamicMapTo<ErpUser>();
        }

        public ErpUser GetUser(string email, string password)
        {
            var query = EntityQuery.QueryAND(EntityQuery.QueryEQ("email", email), EntityQuery.QueryEQ("password", password));
            var result = new RecordManager(service, true).Find(new EntityQuery("user", fieldsToQuery, query));

            if (!result.Success)
                throw new Exception(result.Message);

            ErpUser user = null;
            if (result.Object.Data != null && result.Object.Data.Any())
                user = result.Object.Data[0].DynamicMapTo<ErpUser>();

            return user;
        }

        public bool UpdateUserLastLoginTime(Guid userId)
        {
            RecordManager recMan = new RecordManager(service);
            var record = new EntityRecord();
            record["id"] = userId;
            record["last_logged_in"] = DateTime.UtcNow;
            var response = recMan.UpdateRecord("user", record);
            return response.Success;
        }
    }
}
