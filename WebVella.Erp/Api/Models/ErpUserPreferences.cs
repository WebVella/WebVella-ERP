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

		public bool Compare(ErpUserPreferences prefs)
		{
			if (prefs == null)
				return false;

			if (SidebarSize != prefs.SidebarSize)
				return false;

			if (JsonConvert.SerializeObject(ComponentUsage) != JsonConvert.SerializeObject(prefs.ComponentUsage))
				return false;

			return true;
		}
	}
}
