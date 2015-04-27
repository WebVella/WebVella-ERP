//Test model for a SiteMeta

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Web.Models
{
    public class SiteMeta
    {
		public SiteMeta() {
			Areas = new List<Area>();
        }

		[JsonProperty(PropertyName = "areas")]
		public List<Area> Areas { get; set; }
	}
}