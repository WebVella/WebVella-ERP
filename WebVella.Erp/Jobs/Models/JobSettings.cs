using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Erp.Jobs
{
    public class JobManagerSettings
    {
		[JsonProperty(PropertyName = "db_connection_string")]
		public string DbConnectionString { get; set; }

		[JsonProperty(PropertyName = "enabled")]
		public bool Enabled { get; set; }
	}
}
