using System;
using WebVella.Erp.Jobs;

namespace WebVella.Erp.Api.Models.Mapping;

internal class SchedulePlanToOutputSchedulePlanStrategy : IMapStrategy<SchedulePlan, OutputSchedulePlan>
{
	public OutputSchedulePlan Map(SchedulePlan source)
	{
		if (source == null)
			return null;

		var dest = new OutputSchedulePlan();
		dest.Id = source.Id;
		dest.Name = source.Name;
		dest.Type = source.Type;
		dest.StartDate = source.StartDate;
		dest.EndDate = source.EndDate;
		dest.ScheduledDays = source.ScheduledDays;
		dest.IntervalInMinutes = source.IntervalInMinutes;
		if (source.StartTimespan.HasValue)
		{
			var startTimespan = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			dest.StartTimespan = startTimespan.AddMinutes(source.StartTimespan.Value);
		}
		if (source.EndTimespan.HasValue)
		{
			var endTimespan = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			dest.EndTimespan = endTimespan.AddMinutes(source.EndTimespan.Value);
		}
		dest.LastTriggerTime = source.LastTriggerTime;
		dest.NextTriggerTime = source.NextTriggerTime;
		dest.JobTypeId = source.JobTypeId;
		dest.JobAttributes = source.JobAttributes;
		dest.Enabled = source.Enabled;
		dest.LastStartedJobId = source.LastStartedJobId;
		dest.CreatedOn = source.CreatedOn;
		dest.LastModifiedBy = source.LastModifiedBy;
		dest.LastModifiedOn = source.LastModifiedOn;
		return dest;
	}
}
