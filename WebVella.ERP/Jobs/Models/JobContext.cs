using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Erp.Jobs
{
    public class JobContext
    {
		[JsonProperty(PropertyName = "job_id")]
		public Guid JobId { get; set; }

		[JsonProperty(PropertyName = "aborted")]
		public bool Aborted { get; set; }

		[JsonProperty(PropertyName = "priority")]
		public JobPriority Priority { get; set; }

		[JsonProperty(PropertyName = "attributes")]
		public dynamic Attributes { get; set; }

        [JsonProperty(PropertyName = "result")]
        public dynamic Result { get; set; }

        [JsonProperty(PropertyName = "type")]
		public JobType Type { get; set; }

		internal JobContext()
		{

		}
	}
}
