using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Services
{
	public class UserPreferencies
	{
		public void SetSidebarSize(Guid userId, string size) {
			var currentUser = new SecurityManager().GetUser(userId);
			if (currentUser == null)
				throw new Exception("Unknown user");

			var currentPref = currentUser.Preferences;
			currentPref.SidebarSize = size;

			var updateRecord = new EntityRecord();
			updateRecord["id"] = currentUser.Id;
			updateRecord["preferences"] = JsonConvert.SerializeObject(currentPref);
			var updateResponse = new RecordManager().UpdateRecord("user", updateRecord);

			if (!updateResponse.Success)
				throw new Exception(updateResponse.Message);
		}

		public void SdkUseComponent(Guid userId, string componentFullName)
		{
			var currentUser = new SecurityManager().GetUser(userId);
			if (currentUser == null)
				throw new Exception("Unknown user");

			var currentPref = currentUser.Preferences;

			var currentComponentUsage = currentPref.ComponentUsage.FirstOrDefault(x => x.Name == componentFullName);
			if (currentComponentUsage == null)
			{
				//First time use
				var newComponentUse = new UserComponentUsage()
				{
					Name = componentFullName,
					SdkUsed = 1,
					SdkUsedOn = DateTime.UtcNow
				};
				currentPref.ComponentUsage.Add(newComponentUse);
			}
			else {
				currentComponentUsage.SdkUsed++;
				currentComponentUsage.SdkUsedOn = DateTime.UtcNow;
			}



			var updateRecord = new EntityRecord();
			updateRecord["id"] = currentUser.Id;
			updateRecord["preferences"] = JsonConvert.SerializeObject(currentPref);
			var updateResponse = new RecordManager().UpdateRecord("user", updateRecord);

			if (!updateResponse.Success)
				throw new Exception(updateResponse.Message);
		}

        public EntityRecord GetComponentData(Guid userId, string componentFullName) {
            var currentUser = new SecurityManager().GetUser(userId);
            if (currentUser == null)
                throw new Exception("Unknown user");

            var currentPref = currentUser.Preferences;

            var componentFullNameLowerCase = componentFullName.ToLowerInvariant();

            var componentDataDictionary = currentPref.ComponentDataDictionary;

            if (!componentDataDictionary.Properties.ContainsKey(componentFullNameLowerCase))
                return null;

            EntityRecord record = ((JObject)componentDataDictionary[componentFullNameLowerCase]).ToObject<EntityRecord>();

            return record;
        }

        public void SetComponentData(Guid userId, string componentFullName, EntityRecord data)
        {
            var currentUser = new SecurityManager().GetUser(userId);
            if (currentUser == null)
                throw new Exception("Unknown user");

            var currentPref = currentUser.Preferences;
            var componentFullNameLowerCase = componentFullName.ToLowerInvariant();
            currentPref.ComponentDataDictionary[componentFullNameLowerCase] = data;

            var updateRecord = new EntityRecord();
            updateRecord["id"] = currentUser.Id;
            updateRecord["preferences"] = JsonConvert.SerializeObject(currentPref);
            var updateResponse = new RecordManager().UpdateRecord("user", updateRecord);

            if (!updateResponse.Success)
                throw new Exception(updateResponse.Message);

        }

        public void RemoveComponentData(Guid userId, string componentFullName)
        {
            var currentUser = new SecurityManager().GetUser(userId);
            if (currentUser == null)
                throw new Exception("Unknown user");

            var currentPref = currentUser.Preferences;

            var componentFullNameLowerCase = componentFullName.ToLowerInvariant();

            currentPref.ComponentDataDictionary.Properties.Remove(componentFullNameLowerCase);
            var updateRecord = new EntityRecord();
            updateRecord["id"] = currentUser.Id;
            updateRecord["preferences"] = JsonConvert.SerializeObject(currentPref);
            var updateResponse = new RecordManager().UpdateRecord("user", updateRecord);

            if (!updateResponse.Success)
                throw new Exception(updateResponse.Message);

        }
    }
}
