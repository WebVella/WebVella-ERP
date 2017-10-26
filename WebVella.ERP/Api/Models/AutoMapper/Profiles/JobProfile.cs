using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Dynamic;
using System.Linq;
using WebVella.ERP.Jobs;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	public class JobProfile : Profile
	{
		public JobProfile()
		{
			CreateMap<DataRow, Job>().ConvertUsing(source => JobConvert(source));
			CreateMap<DataRow, SchedulePlan>().ConvertUsing(source => SchedulePlanConvert(source));
			CreateMap<SchedulePlan, OutputSchedulePlan>().ConvertUsing(source => OutputSchedulePlanConvert(source));
		}

		private static Job JobConvert(DataRow src)
		{
			if (src == null)
				return null;

			Job job = new Job();

			job.Id = (Guid)src["id"];
			job.TypeId = (Guid)src["type_id"];
			job.Type = JobManager.JobTypes.FirstOrDefault(t => t.Id == job.TypeId);
			job.TypeName = (string)src["type_name"];
			job.Assembly = (string)src["assembly"];
			job.CompleteClassName = (string)src["complete_class_name"];
			job.MethodName = (string)src["method_name"];
			if (!string.IsNullOrWhiteSpace(src["attributes"].ToString()))
			{
				JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
				job.Attributes = JsonConvert.DeserializeObject<ExpandoObject>((string)src["attributes"], settings);
			}

			if (!string.IsNullOrWhiteSpace(src["result"].ToString()))
			{
				JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
				job.Result = JsonConvert.DeserializeObject<ExpandoObject>((string)src["result"], settings);
			}

			job.Status = (JobStatus)(int)src["status"];
			job.Priority = (JobPriority)(int)src["priority"];
			if (src["started_on"] != DBNull.Value)
				job.StartedOn = (DateTime?)src["started_on"];
			if (src["finished_on"] != DBNull.Value)
				job.FinishedOn = (DateTime?)src["finished_on"];
			if (src["aborted_by"] != DBNull.Value)
				job.AbortedBy = (Guid?)src["aborted_by"];
			if (src["canceled_by"] != DBNull.Value)
				job.CanceledBy = (Guid?)src["canceled_by"];
			if (src["error_message"] != DBNull.Value)
				job.ErrorMessage = (string)src["error_message"];
			job.CreatedOn = (DateTime)src["created_on"];
			if (src["created_by"] != DBNull.Value)
				job.CreatedBy = (Guid?)src["created_by"];

			return job;
		}

		private static SchedulePlan SchedulePlanConvert(DataRow src)
		{
			if (src == null)
				return null;

			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

			SchedulePlan schedulePlan = new SchedulePlan();

			schedulePlan.Id = (Guid)src["id"];
			schedulePlan.Name = (string)src["name"];
			schedulePlan.Type = (SchedulePlanType)src["type"];
			if (src["start_date"] != DBNull.Value)
				schedulePlan.StartDate = (DateTime)src["start_date"];
			if (src["end_date"] != DBNull.Value)
				schedulePlan.EndDate = (DateTime)src["end_date"];
			schedulePlan.ScheduledDays = JsonConvert.DeserializeObject<SchedulePlanDaysOfWeek>((string)src["schedule_days"], settings);
			if (src["interval_in_minutes"] != DBNull.Value)
				schedulePlan.IntervalInMinutes = (int)src["interval_in_minutes"];
			if (src["start_timespan"] != DBNull.Value)
				schedulePlan.StartTimespan = (int)src["start_timespan"];
			if (src["end_timespan"] != DBNull.Value)
				schedulePlan.EndTimespan = (int)src["end_timespan"];
			if (src["last_trigger_time"] != DBNull.Value)
				schedulePlan.LastTriggerTime = (DateTime)src["last_trigger_time"];
			if (src["next_trigger_time"] != DBNull.Value)
				schedulePlan.NextTriggerTime = (DateTime)src["next_trigger_time"];
			schedulePlan.JobTypeId = (Guid)src["job_type_id"];
			if (JobManager.JobTypes.Any(t => t.Id == schedulePlan.JobTypeId))
				schedulePlan.JobType = JobManager.JobTypes.FirstOrDefault(t => t.Id == schedulePlan.JobTypeId);
			if (!string.IsNullOrWhiteSpace(src["job_attributes"].ToString()))
				schedulePlan.JobAttributes = JsonConvert.DeserializeObject<ExpandoObject>((string)src["job_attributes"], settings);
			schedulePlan.Enabled = (bool)src["enabled"];
			if (src["last_started_job_id"] != DBNull.Value)
				schedulePlan.LastStartedJobId = (Guid)src["last_started_job_id"];
			schedulePlan.CreatedOn = (DateTime)src["created_on"];
			if (src["last_modified_by"] != DBNull.Value)
				schedulePlan.LastModifiedBy = (Guid)src["last_modified_by"];
			schedulePlan.LastModifiedOn = (DateTime)src["last_modified_on"];

			return schedulePlan;
		}

		private static OutputSchedulePlan OutputSchedulePlanConvert(SchedulePlan src)
		{
			if (src == null)
				return null;

			OutputSchedulePlan outSchedulePlan = new OutputSchedulePlan();

			outSchedulePlan.Id = src.Id;
			outSchedulePlan.Name = src.Name;
			outSchedulePlan.Type = src.Type;
			outSchedulePlan.StartDate = src.StartDate;
			outSchedulePlan.EndDate = src.EndDate;
			outSchedulePlan.ScheduledDays = src.ScheduledDays;
			outSchedulePlan.IntervalInMinutes = src.IntervalInMinutes;
			if (src.StartTimespan.HasValue)
			{
				var startTimespan = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				outSchedulePlan.StartTimespan = startTimespan.AddMinutes(src.StartTimespan.Value);
			}
			if (src.EndTimespan.HasValue)
			{
				var endTimespan = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				outSchedulePlan.EndTimespan = endTimespan.AddMinutes(src.EndTimespan.Value);
			}
			outSchedulePlan.LastTriggerTime = src.LastTriggerTime;
			outSchedulePlan.NextTriggerTime = src.NextTriggerTime;
			outSchedulePlan.JobTypeId = src.JobTypeId;
			outSchedulePlan.JobAttributes = src.JobAttributes;
			outSchedulePlan.Enabled = src.Enabled;
			outSchedulePlan.LastStartedJobId = src.LastStartedJobId;
			outSchedulePlan.CreatedOn = src.CreatedOn;
			outSchedulePlan.LastModifiedBy = src.LastModifiedBy;
			outSchedulePlan.LastModifiedOn = src.LastModifiedOn;

			return outSchedulePlan;
		}
	}
}
