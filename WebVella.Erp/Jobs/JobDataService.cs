using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Database;

namespace WebVella.Erp.Jobs
{
	internal class JobDataService
	{
		private JobManagerSettings Settings { get; set; }

		public JobDataService(JobManagerSettings settings)
		{
			Settings = settings;
		}

		#region << Jobs >>

		public Job CreateJob(Job job)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("id", job.Id) { NpgsqlDbType = NpgsqlDbType.Uuid });
			parameters.Add(new NpgsqlParameter("type_id", job.Type.Id) { NpgsqlDbType = NpgsqlDbType.Uuid });
			parameters.Add(new NpgsqlParameter("type_name", job.TypeName) { NpgsqlDbType = NpgsqlDbType.Text });
			parameters.Add(new NpgsqlParameter("complete_class_name", job.CompleteClassName) { NpgsqlDbType = NpgsqlDbType.Text });
			if (job.Attributes != null)
				parameters.Add(new NpgsqlParameter("attributes", JsonConvert.SerializeObject(job.Attributes, settings).ToString()) { NpgsqlDbType = NpgsqlDbType.Text });
			parameters.Add(new NpgsqlParameter("status", (int)job.Status) { NpgsqlDbType = NpgsqlDbType.Integer });
			parameters.Add(new NpgsqlParameter("priority", (int)job.Priority) { NpgsqlDbType = NpgsqlDbType.Integer });
			if (job.StartedOn.HasValue)
				parameters.Add(new NpgsqlParameter("started_on", job.StartedOn.HasValue) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			if (job.FinishedOn.HasValue)
				parameters.Add(new NpgsqlParameter("finished_on", job.FinishedOn.HasValue) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			if (job.AbortedBy.HasValue)
				parameters.Add(new NpgsqlParameter("aborted_by", job.AbortedBy) { NpgsqlDbType = NpgsqlDbType.Uuid });
			if (job.CanceledBy.HasValue)
				parameters.Add(new NpgsqlParameter("canceled_by", job.CanceledBy) { NpgsqlDbType = NpgsqlDbType.Uuid });
			if (!string.IsNullOrEmpty(job.ErrorMessage))
				parameters.Add(new NpgsqlParameter("error_message", job.ErrorMessage) { NpgsqlDbType = NpgsqlDbType.Text });
			if (job.SchedulePlanId.HasValue)
				parameters.Add(new NpgsqlParameter("schedule_plan_id", job.SchedulePlanId) { NpgsqlDbType = NpgsqlDbType.Uuid });
			parameters.Add(new NpgsqlParameter("created_on", DateTime.UtcNow) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			parameters.Add(new NpgsqlParameter("last_modified_on", DateTime.UtcNow) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			if (job.CreatedBy.HasValue)
				parameters.Add(new NpgsqlParameter("created_by", job.CreatedBy) { NpgsqlDbType = NpgsqlDbType.Uuid });
			if (job.LastModifiedBy.HasValue)
				parameters.Add(new NpgsqlParameter("last_modified_by", job.LastModifiedBy) { NpgsqlDbType = NpgsqlDbType.Uuid });

			string columns = "";
			string values = "";
			foreach (NpgsqlParameter param in parameters)
			{
				columns += $"{param.ParameterName}, ";
				values += $"@{param.ParameterName}, ";
			}

			columns = columns.Remove(columns.Length - 2, 2);
			values = values.Remove(values.Length - 2, 2);

			string sql = $"INSERT INTO jobs ({columns}) VALUES ({values})";

			if (ExecuteNonQuerySqlCommand(sql, parameters))
				return GetJob(job.Id);

			return null;
		}

		public bool UpdateJob(Job job)
		{
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("id", job.Id) { NpgsqlDbType = NpgsqlDbType.Uuid });
			parameters.Add(new NpgsqlParameter("status", (int)job.Status) { NpgsqlDbType = NpgsqlDbType.Integer });
			parameters.Add(new NpgsqlParameter("priority", (int)job.Priority) { NpgsqlDbType = NpgsqlDbType.Integer });
			if (job.StartedOn.HasValue)
				parameters.Add(new NpgsqlParameter("started_on", job.StartedOn) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			if (job.FinishedOn.HasValue)
				parameters.Add(new NpgsqlParameter("finished_on", job.FinishedOn.HasValue ? job.FinishedOn : null) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			if (job.AbortedBy.HasValue)
				parameters.Add(new NpgsqlParameter("aborted_by", job.AbortedBy) { NpgsqlDbType = NpgsqlDbType.Uuid });
			if (job.CanceledBy.HasValue)
				parameters.Add(new NpgsqlParameter("canceled_by", job.CanceledBy) { NpgsqlDbType = NpgsqlDbType.Uuid });
			if (!string.IsNullOrWhiteSpace(job.ErrorMessage))
				parameters.Add(new NpgsqlParameter("error_message", job.ErrorMessage) { NpgsqlDbType = NpgsqlDbType.Text });

			if (job.Result != null)
			{
				JobResultWrapper jrWrap = new JobResultWrapper { Result = job.Result };
				JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
				string result = JsonConvert.SerializeObject(jrWrap, settings);
				parameters.Add(new NpgsqlParameter("result", result) { NpgsqlDbType = NpgsqlDbType.Text });
			}

			parameters.Add(new NpgsqlParameter("last_modified_on", DateTime.UtcNow) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			if (job.LastModifiedBy.HasValue)
				parameters.Add(new NpgsqlParameter("last_modified_by", job.LastModifiedBy) { NpgsqlDbType = NpgsqlDbType.Uuid });

			string setClause = "";
			foreach (NpgsqlParameter param in parameters)
			{
				if (param.ParameterName != "id")
					setClause += $"{param.ParameterName} = @{param.ParameterName}, ";
			}

			setClause = setClause.Remove(setClause.Length - 2, 2);

			string sql = $"UPDATE jobs SET {setClause} WHERE id = @id";

			return ExecuteNonQuerySqlCommand(sql, parameters);
		}

		public Job GetJob(Guid jobId)
		{
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("id", jobId) { NpgsqlDbType = NpgsqlDbType.Uuid });

			string sql = "SELECT * FROM jobs WHERE id = @id";

			DataTable resultTable = ExecuteQuerySqlCommand(sql, parameters);

			Job job = null;
			if (resultTable.Rows != null && resultTable.Rows.Count > 0)
				job = resultTable.Rows[0].MapTo<Job>();

			return job;
		}

		public bool IsJobFinished(Guid id)
		{
			Job job = GetJob(id);

			if (job == null)
				return true;

			return job.FinishedOn.HasValue;
		}

		public List<Job> GetPendingJobs(int? limit = null)
		{
			return GetJobs(JobStatus.Pending, limit);
		}

		public List<Job> GetRunningJobs(int? limit = null)
		{
			return GetJobs(JobStatus.Running, limit);
		}

		public List<Job> GetJobs(JobStatus status, int? limit = null)
		{
			string sql = "SELECT * FROM jobs WHERE status = @status ORDER BY priority DESC, created_on ASC";
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("status", (int)status) { NpgsqlDbType = NpgsqlDbType.Integer });
			if (limit.HasValue)
			{
				if (limit.Value < 0)
					limit = 0;
				parameters.Add(new NpgsqlParameter("limit", limit) { NpgsqlDbType = NpgsqlDbType.Integer });
				sql += " LIMIT @limit";
			}

			DataTable dtJobs = ExecuteQuerySqlCommand(sql, parameters);
			return dtJobs.Rows.MapTo<Job>();
		}

		public List<Job> GetJobs(DateTime? startFromDate = null, DateTime? startToDate = null, DateTime? finishedFromDate = null,
			DateTime? finishedToDate = null, string typeName = null, int? status = null, int? priority = null, Guid? schedulePlanId = null, int? page = null, int? pageSize = null)
		{
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

			string sql = "SELECT * FROM jobs WHERE id IS NOT NULL";

			if (startFromDate.HasValue)
			{
				parameters.Add(new NpgsqlParameter("started_from", startFromDate.Value) { NpgsqlDbType = NpgsqlDbType.Timestamp });
				sql += " AND started_on >= @started_from";
			}
			if (startToDate.HasValue)
			{
				parameters.Add(new NpgsqlParameter("started_to", startToDate.Value) { NpgsqlDbType = NpgsqlDbType.Timestamp });
				sql += " AND started_on <= @started_to";
			}
			if (finishedFromDate.HasValue)
			{
				parameters.Add(new NpgsqlParameter("finished_from", finishedFromDate.Value) { NpgsqlDbType = NpgsqlDbType.Timestamp });
				sql += " AND finished_on <= @finished_from";
			}
			if (finishedToDate.HasValue)
			{
				parameters.Add(new NpgsqlParameter("finished_to", finishedFromDate.Value) { NpgsqlDbType = NpgsqlDbType.Timestamp });
				sql += " AND finished_on <= @finished_to";
			}
			if (!string.IsNullOrWhiteSpace(typeName))
			{
				var typeParameter = "%" + typeName + "%";
				parameters.Add(new NpgsqlParameter("type_name", typeParameter) { NpgsqlDbType = NpgsqlDbType.Text });
				sql += " AND type_name ILIKE @type_name";
			}
			if (status.HasValue)
			{
				parameters.Add(new NpgsqlParameter("status", status.Value) { NpgsqlDbType = NpgsqlDbType.Integer });
				sql += " AND status = @status";
			}
			if (priority.HasValue)
			{
				parameters.Add(new NpgsqlParameter("priority", priority.Value) { NpgsqlDbType = NpgsqlDbType.Integer });
				sql += " AND priority = @priority";
			}
			if (schedulePlanId.HasValue)
			{
				parameters.Add(new NpgsqlParameter("schedule_plan_id", schedulePlanId.Value) { NpgsqlDbType = NpgsqlDbType.Uuid });
				sql += " AND schedule_plan_id = @schedule_plan_id";
			}

			sql += " ORDER BY created_on DESC";

			if (pageSize.HasValue)
			{
				page = page ?? 1;
				int limit = pageSize.Value;
				int skip = (page.Value - 1) * limit;

				parameters.Add(new NpgsqlParameter("limit", limit) { NpgsqlDbType = NpgsqlDbType.Integer });
				parameters.Add(new NpgsqlParameter("offset", skip) { NpgsqlDbType = NpgsqlDbType.Integer });
				sql += " LIMIT @limit OFFSET @offset";
			}

			DataTable dtJobs = ExecuteQuerySqlCommand(sql, parameters);
			return dtJobs.Rows.MapTo<Job>();
		}

		internal long GetJobsTotalCount(DateTime? startFromDate = null, DateTime? startToDate = null, DateTime? finishedFromDate = null,
			DateTime? finishedToDate = null, string typeName = null, int? status = null, int? priority = null, Guid? schedulePlanId = null)
		{
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

			string sql = "SELECT COUNT(id) FROM jobs WHERE id IS NOT NULL";

			if (startFromDate.HasValue)
			{
				parameters.Add(new NpgsqlParameter("started_from", startFromDate.Value) { NpgsqlDbType = NpgsqlDbType.Timestamp });
				sql += " AND started_on >= @started_from";
			}
			if (startToDate.HasValue)
			{
				parameters.Add(new NpgsqlParameter("started_to", startToDate.Value) { NpgsqlDbType = NpgsqlDbType.Timestamp });
				sql += " AND started_on <= @started_to";
			}
			if (finishedFromDate.HasValue)
			{
				parameters.Add(new NpgsqlParameter("finished_from", finishedFromDate.Value) { NpgsqlDbType = NpgsqlDbType.Timestamp });
				sql += " AND finished_on <= @finished_from";
			}
			if (finishedToDate.HasValue)
			{
				parameters.Add(new NpgsqlParameter("finished_to", finishedFromDate.Value) { NpgsqlDbType = NpgsqlDbType.Timestamp });
				sql += " AND finished_on <= @finished_to";
			}
			if (!string.IsNullOrWhiteSpace(typeName))
			{
				var typeParameter = "%" + typeName + "%";
				parameters.Add(new NpgsqlParameter("type_name", typeParameter) { NpgsqlDbType = NpgsqlDbType.Text });
				sql += " AND type_name ILIKE @type_name";
			}
			if (status.HasValue)
			{
				parameters.Add(new NpgsqlParameter("status", status.Value) { NpgsqlDbType = NpgsqlDbType.Integer });
				sql += " AND status = @status";
			}
			if (priority.HasValue)
			{
				parameters.Add(new NpgsqlParameter("priority", priority.Value) { NpgsqlDbType = NpgsqlDbType.Integer });
				sql += " AND priority = @priority";
			}
			if (schedulePlanId.HasValue)
			{
				parameters.Add(new NpgsqlParameter("schedule_plan_id", schedulePlanId.Value) { NpgsqlDbType = NpgsqlDbType.Uuid });
				sql += " AND schedule_plan_id = @schedule_plan_id";
			}

			DataTable dtResult = ExecuteQuerySqlCommand(sql, parameters);
			return (long)dtResult.Rows[0][0];
		}

		#endregion

		#region << Schedule >>

		public bool CreateSchedule(SchedulePlan schedulePlan)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("id", schedulePlan.Id) { NpgsqlDbType = NpgsqlDbType.Uuid });
			parameters.Add(new NpgsqlParameter("name", schedulePlan.Name) { NpgsqlDbType = NpgsqlDbType.Text });
			parameters.Add(new NpgsqlParameter("type", (int)schedulePlan.Type) { NpgsqlDbType = NpgsqlDbType.Integer });
			parameters.Add(new NpgsqlParameter("job_type_id", schedulePlan.JobTypeId) { NpgsqlDbType = NpgsqlDbType.Uuid });
			if (schedulePlan.StartDate.HasValue)
				parameters.Add(new NpgsqlParameter("start_date", schedulePlan.StartDate) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			if (schedulePlan.EndDate.HasValue)
				parameters.Add(new NpgsqlParameter("end_date", schedulePlan.EndDate) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			parameters.Add(new NpgsqlParameter("schedule_days", JsonConvert.SerializeObject(schedulePlan.ScheduledDays, settings).ToString()) { NpgsqlDbType = NpgsqlDbType.Json });
			if (schedulePlan.IntervalInMinutes.HasValue)
				parameters.Add(new NpgsqlParameter("interval_in_minutes", schedulePlan.IntervalInMinutes) { NpgsqlDbType = NpgsqlDbType.Integer });
			if (schedulePlan.StartTimespan.HasValue)
				parameters.Add(new NpgsqlParameter("start_timespan", schedulePlan.StartTimespan) { NpgsqlDbType = NpgsqlDbType.Integer });
			if (schedulePlan.EndTimespan.HasValue)
				parameters.Add(new NpgsqlParameter("end_timespan", schedulePlan.EndTimespan) { NpgsqlDbType = NpgsqlDbType.Integer });
			if (schedulePlan.LastTriggerTime.HasValue)
				parameters.Add(new NpgsqlParameter("last_trigger_time", schedulePlan.LastTriggerTime) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			if (schedulePlan.NextTriggerTime.HasValue)
				parameters.Add(new NpgsqlParameter("next_trigger_time", schedulePlan.NextTriggerTime) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			parameters.Add(new NpgsqlParameter("job_attributes", JsonConvert.SerializeObject(schedulePlan.JobAttributes, settings).ToString()) { NpgsqlDbType = NpgsqlDbType.Text });
			parameters.Add(new NpgsqlParameter("enabled", schedulePlan.Enabled) { NpgsqlDbType = NpgsqlDbType.Boolean });
			if (schedulePlan.LastStartedJobId.HasValue)
				parameters.Add(new NpgsqlParameter("last_started_job_id", schedulePlan.LastStartedJobId) { NpgsqlDbType = NpgsqlDbType.Uuid });
			parameters.Add(new NpgsqlParameter("created_on", DateTime.UtcNow) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			parameters.Add(new NpgsqlParameter("last_modified_on", DateTime.UtcNow) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			if (schedulePlan.LastModifiedBy.HasValue)
				parameters.Add(new NpgsqlParameter("last_modified_by", schedulePlan.LastModifiedBy) { NpgsqlDbType = NpgsqlDbType.Uuid });

			string columns = "";
			string values = "";
			foreach (NpgsqlParameter param in parameters)
			{
				columns += $"{param.ParameterName}, ";
				values += $"@{param.ParameterName}, ";
			}

			columns = columns.Remove(columns.Length - 2, 2);
			values = values.Remove(values.Length - 2, 2);

			string sql = $"INSERT INTO schedule_plan ({columns}) VALUES ({values})";

			return ExecuteNonQuerySqlCommand(sql, parameters);
		}

		public bool UpdateSchedule(SchedulePlan schedulePlan)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("id", schedulePlan.Id) { NpgsqlDbType = NpgsqlDbType.Uuid });
			parameters.Add(new NpgsqlParameter("name", schedulePlan.Name) { NpgsqlDbType = NpgsqlDbType.Text });
			parameters.Add(new NpgsqlParameter("type", (int)schedulePlan.Type) { NpgsqlDbType = NpgsqlDbType.Integer });
			if (schedulePlan.StartDate.HasValue)
				parameters.Add(new NpgsqlParameter("start_date", schedulePlan.StartDate) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			if (schedulePlan.EndDate.HasValue)
				parameters.Add(new NpgsqlParameter("end_date", schedulePlan.EndDate) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			if (schedulePlan.ScheduledDays != null)
				parameters.Add(new NpgsqlParameter("schedule_days", JsonConvert.SerializeObject(schedulePlan.ScheduledDays, settings).ToString()) { NpgsqlDbType = NpgsqlDbType.Json });
			if (schedulePlan.IntervalInMinutes.HasValue)
				parameters.Add(new NpgsqlParameter("interval_in_minutes", schedulePlan.IntervalInMinutes) { NpgsqlDbType = NpgsqlDbType.Integer });
			if (schedulePlan.StartTimespan.HasValue)
				parameters.Add(new NpgsqlParameter("start_timespan", schedulePlan.StartTimespan) { NpgsqlDbType = NpgsqlDbType.Integer });
			if (schedulePlan.EndTimespan.HasValue)
				parameters.Add(new NpgsqlParameter("end_timespan", schedulePlan.EndTimespan) { NpgsqlDbType = NpgsqlDbType.Integer });
			if (schedulePlan.LastTriggerTime.HasValue)
				parameters.Add(new NpgsqlParameter("last_trigger_time", schedulePlan.LastTriggerTime) { NpgsqlDbType = NpgsqlDbType.Timestamp });

			if (schedulePlan.NextTriggerTime.HasValue)
				parameters.Add(new NpgsqlParameter("next_trigger_time", schedulePlan.NextTriggerTime) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			else
				parameters.Add(new NpgsqlParameter("next_trigger_time", DBNull.Value) { NpgsqlDbType = NpgsqlDbType.Timestamp });

			parameters.Add(new NpgsqlParameter("enabled", schedulePlan.Enabled) { NpgsqlDbType = NpgsqlDbType.Boolean });
			if (schedulePlan.LastStartedJobId.HasValue)
				parameters.Add(new NpgsqlParameter("last_started_job_id", schedulePlan.LastStartedJobId) { NpgsqlDbType = NpgsqlDbType.Uuid });
			parameters.Add(new NpgsqlParameter("last_modified_on", DateTime.UtcNow) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			if (schedulePlan.LastModifiedBy.HasValue)
				parameters.Add(new NpgsqlParameter("last_modified_by", schedulePlan.LastModifiedBy) { NpgsqlDbType = NpgsqlDbType.Uuid });

			string setClause = "";
			foreach (NpgsqlParameter param in parameters)
			{
				if (param.ParameterName != "id")
					setClause += $"{param.ParameterName} = @{param.ParameterName}, ";
			}

			setClause = setClause.Remove(setClause.Length - 2, 2);

			string sql = $"UPDATE schedule_plan SET {setClause} WHERE id = @id";

			return ExecuteNonQuerySqlCommand(sql, parameters);
		}

		public bool UpdateSchedule(Guid schedulePlanId, DateTime? lastTriggerTime, DateTime? nextTriggerTime,
			Guid? modifiedBy, Guid? lastStartedJobId)
		{
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("id", schedulePlanId) { NpgsqlDbType = NpgsqlDbType.Uuid });
			if (lastTriggerTime.HasValue)
				parameters.Add(new NpgsqlParameter("last_trigger_time", lastTriggerTime) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			if (nextTriggerTime.HasValue)
				parameters.Add(new NpgsqlParameter("next_trigger_time", nextTriggerTime.Value) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			else
				parameters.Add(new NpgsqlParameter("next_trigger_time", DBNull.Value) { NpgsqlDbType = NpgsqlDbType.Timestamp });

			if (lastStartedJobId.HasValue)
				parameters.Add(new NpgsqlParameter("last_started_job_id", lastStartedJobId) { NpgsqlDbType = NpgsqlDbType.Uuid });
			parameters.Add(new NpgsqlParameter("last_modified_on", DateTime.UtcNow) { NpgsqlDbType = NpgsqlDbType.Timestamp });
			if (modifiedBy.HasValue)
				parameters.Add(new NpgsqlParameter("last_modified_by", modifiedBy) { NpgsqlDbType = NpgsqlDbType.Uuid });

			string setClause = "";
			foreach (NpgsqlParameter param in parameters)
			{
				if (param.ParameterName != "id")
					setClause += $"{param.ParameterName} = @{param.ParameterName}, ";
			}

			setClause = setClause.Remove(setClause.Length - 2, 2);

			string sql = $"UPDATE schedule_plan SET {setClause} WHERE id = @id";

			return ExecuteNonQuerySqlCommand(sql, parameters);
		}

		public SchedulePlan GetSchedulePlan(Guid id)
		{
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("id", id) { NpgsqlDbType = NpgsqlDbType.Uuid });

			string sql = "SELECT * FROM schedule_plan WHERE id = @id";

			DataTable dtSchedulePlans = ExecuteQuerySqlCommand(sql, parameters);

			SchedulePlan schedulePlan = null;
			if (dtSchedulePlans.Rows != null && dtSchedulePlans.Rows.Count > 0)
				schedulePlan = dtSchedulePlans.Rows[0].MapTo<SchedulePlan>();

			return schedulePlan;
		}

		public List<SchedulePlan> GetSchedulePlans()
		{
			string sql = "SELECT * FROM schedule_plan ORDER BY name";

			DataTable dtSchedulePlans = ExecuteQuerySqlCommand(sql);

			return dtSchedulePlans.Rows.MapTo<SchedulePlan>();
		}

		public List<SchedulePlan> GetReadyForExecutionScheduledPlans()
		{
			string sql = "SELECT * FROM schedule_plan" +
				" WHERE enabled = true AND next_trigger_time <= @utc_now AND start_date <= @utc_now" +
				" AND COALESCE(end_date, @utc_now) >= @utc_now" +
				" ORDER BY next_trigger_time ASC";
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("utc_now", DateTime.UtcNow) { NpgsqlDbType = NpgsqlDbType.Timestamp });

			DataTable dtSchedulePlans = ExecuteQuerySqlCommand(sql, parameters);

			return dtSchedulePlans.Rows.MapTo<SchedulePlan>();
		}

		public List<SchedulePlan> GetScheduledPlansByType(SchedulePlanType type)
		{
			string sql = "SELECT * FROM schedule_plan WHERE type = @type ORDER BY name";
			List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
			parameters.Add(new NpgsqlParameter("type", (int)type) { NpgsqlDbType = NpgsqlDbType.Integer });

			DataTable dtSchedulePlans = ExecuteQuerySqlCommand(sql, parameters);

			return dtSchedulePlans.Rows.MapTo<SchedulePlan>();
		}

		#endregion

		#region << Helper methods >>

		private bool ExecuteNonQuerySqlCommand(string sql, List<NpgsqlParameter> parameters = null)
		{
			using (NpgsqlConnection con = new NpgsqlConnection(Settings.DbConnectionString))
			{
				try
				{
					con.Open();
					NpgsqlCommand command = new NpgsqlCommand(sql, con);
					command.CommandType = CommandType.Text;
					if (parameters != null && parameters.Count > 0)
						command.Parameters.AddRange(parameters.ToArray());
					return command.ExecuteNonQuery() > 0;
				}
				finally
				{
					con.Close();
				}
			}
		}

		private DataTable ExecuteQuerySqlCommand(string sql, List<NpgsqlParameter> parameters = null)
		{
			using (NpgsqlConnection con = new NpgsqlConnection(Settings.DbConnectionString))
			{
				try
				{
					con.Open();
					NpgsqlCommand command = new NpgsqlCommand(sql, con);
					command.CommandType = CommandType.Text;
					if (parameters != null && parameters.Count > 0)
						command.Parameters.AddRange(parameters.ToArray());

					DataTable resultTable = new DataTable();
					NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
					adapter.Fill(resultTable);
					return resultTable;
				}
				finally
				{
					con.Close();
				}
			}
		}

		#endregion
	}
}
