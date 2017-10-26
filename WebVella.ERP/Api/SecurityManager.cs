using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Api.Models.AutoMapper;
using WebVella.ERP.Database;

namespace WebVella.ERP.Api
{
    public class SecurityManager
    {
        const string fieldsToQuery = @"id,username,email,password,first_name,last_name,image,created_on,created_by,last_modified_on,last_modified_by, last_logged_in,enabled, $user_role.id, $user_role.name";

        public SecurityManager()
        {
        }

        public ErpUser GetUser(Guid userId)
        {
            RecordManager recMan = new RecordManager(true);
            EntityQuery query = new EntityQuery("user", fieldsToQuery, EntityQuery.QueryEQ("id", userId), null, null, null);
            var result = recMan.Find(query);
            if (!result.Success)
                throw new Exception(result.Message);

            if (!result.Object.Data.Any())
                return null;

            var record = result.Object.Data.Single();
            return record.MapTo<ErpUser>();

        }

        public ErpUser GetUser(string email)
        {
            RecordManager recMan = new RecordManager(true);
            EntityQuery query = new EntityQuery("user", fieldsToQuery, EntityQuery.QueryEQ("email", email), null, null, null);
            var result = recMan.Find(query);
            if (!result.Success)
                throw new Exception(result.Message);

            if (!result.Object.Data.Any())
                return null;

            var record = result.Object.Data.Single();
            return record.MapTo<ErpUser>();
        }

        public ErpUser GetUser(string email, string password)
        {
            var query = EntityQuery.QueryAND(EntityQuery.QueryEQ("email", email), EntityQuery.QueryEQ("password", password));
            var result = new RecordManager(true).Find(new EntityQuery("user", fieldsToQuery, query));

            if (!result.Success)
                throw new Exception(result.Message);

            ErpUser user = null;
            if (result.Object.Data != null && result.Object.Data.Any())
                user = result.Object.Data[0].MapTo<ErpUser>();

            return user;
        }

        public void UpdateUserLastLoginTime(Guid userId)
        {
            List<KeyValuePair<string, object>> storageRecordData = new List<KeyValuePair<string, object>>();
            storageRecordData.Add(new KeyValuePair<string, object>("id", userId ));
            storageRecordData.Add(new KeyValuePair<string, object>("last_logged_in", DateTime.UtcNow ));
            DbContext.Current.RecordRepository.Update("user", storageRecordData);
        }

        public List<ErpUser> GetUsers(params Guid[] roleIds)
        {
            const string fieldsToQuery = @"id,username,email,first_name,last_name,image,created_on,created_by,last_modified_on,last_modified_by, last_logged_in,enabled, $user_role.id, $user_role.name";
            QueryObject query = null;
            if (roleIds.Count() > 0)
            {
                List<QueryObject> list = new List<QueryObject>();
                foreach (var roleId in roleIds)
                {
                    list.Add(EntityQuery.QueryEQ("$user_role.id", roleId));
                }
                
                query = EntityQuery.QueryOR(list.ToArray());
            }

            var result = new RecordManager(true).Find(new EntityQuery("user", fieldsToQuery, query));

            if (!result.Success)
                throw new Exception(result.Message);
            
            if (result.Object.Data != null && result.Object.Data.Any())
                return result.Object.Data.MapTo<ErpUser>();

            return new List<ErpUser>();
        }
    }
}
