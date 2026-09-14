using System;
using System.Data;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using WebVella.Erp.Jobs;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DataRowToSchedulePlanStrategy : IMapStrategy<DataRow, SchedulePlan>
{
	public SchedulePlan Map(DataRow source)
	{
		if (source == null)
			return null;

		JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

		var dest = new SchedulePlan();
		dest.Id = (Guid)source["id"];
		dest.Name = (string)source["name"];
		dest.Type = (SchedulePlanType)source["type"];

		if (source["start_date"] != DBNull.Value)
			dest.StartDate = DateTime.SpecifyKind((DateTime)source["start_date"], DateTimeKind.Utc);

		if (source["end_date"] != DBNull.Value)
			dest.EndDate = DateTime.SpecifyKind((DateTime)source["end_date"], DateTimeKind.Utc);

		dest.ScheduledDays = JsonConvert.DeserializeObject<SchedulePlanDaysOfWeek>((string)source["schedule_days"], settings);

		if (source["interval_in_minutes"] != DBNull.Value)
			dest.IntervalInMinutes = (int)source["interval_in_minutes"];

		if (source["start_timespan"] != DBNull.Value)
			dest.StartTimespan = (int)source["start_timespan"];
		if (source["end_timespan"] != DBNull.Value)
			dest.EndTimespan = (int)source["end_timespan"];
		if (source["last_trigger_time"] != DBNull.Value)
			dest.LastTriggerTime = DateTime.SpecifyKind((DateTime)source["last_trigger_time"], DateTimeKind.Utc);

		if (source["next_trigger_time"] != DBNull.Value)
			dest.NextTriggerTime = DateTime.SpecifyKind((DateTime)source["next_trigger_time"], DateTimeKind.Utc);

		dest.JobTypeId = (Guid)source["job_type_id"];

		if (JobManager.JobTypes.Any(t => t.Id == dest.JobTypeId))
			dest.JobType = JobManager.JobTypes.FirstOrDefault(t => t.Id == dest.JobTypeId);

		if (!string.IsNullOrWhiteSpace(source["job_attributes"].ToString()))
			dest.JobAttributes = JsonConvert.DeserializeObject<ExpandoObject>((string)source["job_attributes"], settings);

		dest.Enabled = (bool)source["enabled"];

		if (source["last_started_job_id"] != DBNull.Value)
			dest.LastStartedJobId = (Guid)source["last_started_job_id"];

		dest.CreatedOn = DateTime.SpecifyKind((DateTime)source["created_on"], DateTimeKind.Utc);

		if (source["last_modified_by"] != DBNull.Value)
			dest.LastModifiedBy = (Guid)source["last_modified_by"];

		dest.LastModifiedOn = DateTime.SpecifyKind((DateTime)source["last_modified_on"], DateTimeKind.Utc);

		return dest;
	}
}
