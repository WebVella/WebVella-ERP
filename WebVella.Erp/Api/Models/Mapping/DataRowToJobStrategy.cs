using System;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using WebVella.Erp.Jobs;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DataRowToJobStrategy : IMapStrategy<DataRow, Job>
{
	public Job Map(DataRow source)
	{
		if (source == null)
			return null;

		var dest = new Job();
		dest.Id = (Guid)source["id"];
		dest.TypeId = (Guid)source["type_id"];
		dest.Type = JobManager.JobTypes.FirstOrDefault(t => t.Id == dest.TypeId);
		dest.TypeName = (string)source["type_name"];
		dest.CompleteClassName = (string)source["complete_class_name"];

		if (!string.IsNullOrWhiteSpace(source["attributes"].ToString()))
		{
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
			dest.Attributes = JsonConvert.DeserializeObject<ExpandoObject>((string)source["attributes"], settings);
		}

		if (!string.IsNullOrWhiteSpace(source["result"].ToString()))
		{
			try
			{
				try
				{
					JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
					dest.Result = JsonConvert.DeserializeObject<ExpandoObject>((string)source["result"], settings);
				}
				catch
				{
					JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
					dest.Result = JsonConvert.DeserializeObject<JobResultWrapper>((string)source["result"], settings).Result;
				}
			}
			catch
			{
				dest.Result = "ERROR WHILE DESERIALIZE: " + (string)source["result"];
			}
		}

		dest.Status = (JobStatus)(int)source["status"];
		dest.Priority = (JobPriority)(int)source["priority"];
		if (source["started_on"] != DBNull.Value)
			dest.StartedOn = (DateTime?)source["started_on"];
		if (source["finished_on"] != DBNull.Value)
			dest.FinishedOn = (DateTime?)source["finished_on"];
		if (source["aborted_by"] != DBNull.Value)
			dest.AbortedBy = (Guid?)source["aborted_by"];
		if (source["canceled_by"] != DBNull.Value)
			dest.CanceledBy = (Guid?)source["canceled_by"];
		if (source["error_message"] != DBNull.Value)
			dest.ErrorMessage = (string)source["error_message"];
		dest.CreatedOn = (DateTime)source["created_on"];
		if (source["created_by"] != DBNull.Value)
			dest.CreatedBy = (Guid?)source["created_by"];

		if (dest.StartedOn.HasValue && dest.StartedOn.Value.Kind == DateTimeKind.Unspecified)
			dest.StartedOn = (DateTime?)DateTime.SpecifyKind(dest.StartedOn.Value, DateTimeKind.Utc);
		if (dest.FinishedOn.HasValue && dest.FinishedOn.Value.Kind == DateTimeKind.Unspecified)
			dest.FinishedOn = (DateTime?)DateTime.SpecifyKind(dest.FinishedOn.Value, DateTimeKind.Utc);
		if (dest.CreatedOn.Kind == DateTimeKind.Unspecified)
			dest.CreatedOn = DateTime.SpecifyKind(dest.CreatedOn, DateTimeKind.Utc);

		return dest;
	}
}
