using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.Erp.Api.Models
{
	[Serializable]
	public class ErpUserPreferences
	{
		[JsonProperty("sidebar_size")]
		public string SidebarSize { get; set; } = "";

		[JsonProperty("component_usage")]
		public List<UserComponentUsage> ComponentUsage { get; set; } = new List<UserComponentUsage>();

        [JsonProperty("component_data_dictionary")]
        public EntityRecord ComponentDataDictionary { get; set; } = new EntityRecord(); // full.component.name: EntityRecord

        public bool Compare(ErpUserPreferences prefs)
		{
			if (prefs == null)
				return false;

			if (SidebarSize != prefs.SidebarSize)
				return false;

			if (JsonConvert.SerializeObject(ComponentUsage) != JsonConvert.SerializeObject(prefs.ComponentUsage))
				return false;

            if (JsonConvert.SerializeObject(ComponentDataDictionary) != JsonConvert.SerializeObject(prefs.ComponentDataDictionary))
                return false;

            return true;
		}
	}
}
