using System;
using AutoMapper;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
    internal class ErpUserConverter : ITypeConverter<EntityRecord, ErpUser>
    {
        public ErpUser Convert(EntityRecord source, ErpUser destination, ResolutionContext context)
        {
            var src = source;

            if (src == null)
                return null;

            ErpUser dest = new ErpUser();
            dest.Id = (Guid)src["id"];
            dest.Username = (string)src["username"];
            dest.Email = (string)src["email"];

            try
            {
                dest.Password = (string)src["password"];
            }
            catch (KeyNotFoundException)
            {
                //set password to null if it is not selected from DB
                dest.Password = null;
            }

            dest.FirstName = (string)src["first_name"];
            dest.LastName = (string)src["last_name"];
            dest.Image = (string)src["image"];
            dest.CreatedOn = (DateTime)src["created_on"];
            dest.CreatedBy = (Guid?)src["created_by"];
            dest.ModifiedOn = (DateTime)src["last_modified_on"];
            dest.ModifiedBy = (Guid?)src["last_modified_by"];
            dest.LastLoggedIn = (DateTime?)src["last_logged_in"];
            dest.Enabled = (bool)src["enabled"];

            dest.Roles = new List<ErpRole>();
            if (src.Properties.ContainsKey("$user_role") && src["$user_role"] != null)
                ((List<EntityRecord>)src["$user_role"]).ForEach(x => dest.Roles.Add(x.MapTo<ErpRole>()));

            return dest;
        }
    }

    internal class ErpUserConverterOposite : ITypeConverter<ErpUser, EntityRecord>
    {
        public EntityRecord Convert(ErpUser source, EntityRecord destination, ResolutionContext context)
        {
            var src = source;

            if (src == null)
                return null;

            EntityRecord dest = new EntityRecord();
            dest["id"] = src.Id;
            dest["username"] = src.Username;
            dest["email"] = src.Email;
            dest["password"] = src.Password;
            dest["first_name"] = src.FirstName;
            dest["last_name"] = src.LastName;
            dest["created_on"] = src.CreatedOn;
            dest["created_by"] = src.CreatedBy;
            dest["last_modified_on"] = src.ModifiedOn;
            dest["last_modified_by"] = src.ModifiedBy;
            dest["last_logged_in"] = src.LastLoggedIn;
            dest["enabled"] = src.Enabled;

            return dest;
        }
    }
}