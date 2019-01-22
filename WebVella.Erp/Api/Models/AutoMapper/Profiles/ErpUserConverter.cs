using System;
using AutoMapper;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebVella.Erp.Api.Models.AutoMapper.Profiles
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
            dest.LastLoggedIn = (DateTime?)src["last_logged_in"];
            dest.Enabled = (bool)src["enabled"];
			dest.Verified = (bool)src["verified"];

            if (src.Properties.ContainsKey("$user_role") && src["$user_role"] != null)
                ((List<EntityRecord>)src["$user_role"]).ForEach(x => dest.Roles.Add(x.MapTo<ErpRole>()));

			dest.Preferences = new ErpUserPreferences();
			if (source.Properties.ContainsKey("preferences"))
			{
				string prefsJson = (string)source["preferences"];
				if (!string.IsNullOrWhiteSpace(prefsJson))
				{
					var jsonSerializerSettings = new JsonSerializerSettings();
					jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
					dest.Preferences = JsonConvert.DeserializeObject<ErpUserPreferences>(prefsJson, jsonSerializerSettings);
				}
			}

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
            dest["last_logged_in"] = src.LastLoggedIn;
            dest["enabled"] = src.Enabled;
			dest["verified"] = src.Verified;
			dest["preferences"] = src.Preferences??new ErpUserPreferences();

			return dest;
        }
    }
}