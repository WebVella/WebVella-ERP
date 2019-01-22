using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Erp.Jobs
{
	[Serializable]
	public enum JobPriority
	{
		Low = 1,
		Medium = 2,
		High = 3,
		Higher = 4,
		Highest = 5
	}

	[Serializable]
	public class JobType
    {
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "default_job_priority_id")]
		public JobPriority DefaultPriority { get; set; }

		[JsonProperty(PropertyName = "assembly")]
		public string Assembly { get; set; }

		[JsonProperty(PropertyName = "complete_class_name")]
		public string CompleteClassName { get; set; }

		[JsonProperty(PropertyName = "allow_single_instance")]
		public bool AllowSingleInstance { get; set; }

		[JsonIgnore]
		public Type ErpJobType { get; set; }
	}
}
