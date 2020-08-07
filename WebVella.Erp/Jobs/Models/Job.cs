using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Jobs
{
	[Serializable]
	public enum JobStatus
	{
		Pending = 1,
		Running = 2,
		Canceled = 3,
		Failed = 4,
		Finished = 5,
		Aborted = 6,
	}

	[Serializable]
	internal class JobResultWrapper
	{
		[JsonProperty(PropertyName = "result")]
		public dynamic Result { get; set; } = null;
	}

	[Serializable]
	public class Job
    {
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "type_id")]
		public Guid TypeId { get; set; }

		[JsonProperty(PropertyName = "type")]
		public JobType Type { get; set; }

		[JsonProperty(PropertyName = "type_name")]
		public string TypeName { get; set; }

		[JsonProperty(PropertyName = "complete_class_name")]
		public string CompleteClassName { get; set; }

		[JsonProperty(PropertyName = "attributes")]
		public dynamic Attributes { get; set; }

        [JsonProperty(PropertyName = "result")]
        public dynamic Result { get; set; }

        [JsonProperty(PropertyName = "status")]
		public JobStatus Status { get; set; }

		[JsonProperty(PropertyName = "priority")]
		public JobPriority Priority { get; set; }

		[JsonProperty(PropertyName = "started_on")]
		public DateTime? StartedOn { get; set; }

		[JsonProperty(PropertyName = "finished_on")]
		public DateTime? FinishedOn { get; set; }

		[JsonProperty(PropertyName = "aborted_by")]
		public Guid? AbortedBy { get; set; }

		[JsonProperty(PropertyName = "canceled_by")]
		public Guid? CanceledBy { get; set; }

		[JsonProperty(PropertyName = "error_message")]
		public string ErrorMessage { get; set; }

		[JsonProperty(PropertyName = "schedule_plan_id")]
		public Guid? SchedulePlanId { get; set; }

		[JsonProperty(PropertyName = "created_on")]
		public DateTime CreatedOn { get; set; }

		[JsonProperty(PropertyName = "created_by")]
		public Guid? CreatedBy { get; set; }

		[JsonProperty(PropertyName = "last_modified_on")]
		public DateTime LastModifiedOn { get; set; }

		[JsonProperty(PropertyName = "last_modified_by")]
		public Guid? LastModifiedBy { get; set; }
	}
}
