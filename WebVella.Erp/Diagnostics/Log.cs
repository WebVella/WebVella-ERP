using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Data;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;

namespace WebVella.Erp.Diagnostics
{
	public class Log
	{

		public EntityRecordList GetLogs(int page, int pageSize, string querySource = null, string queryMessage = null)
		{
			int offset = (page - 1) * pageSize;
			int limit = pageSize;

			using (var connection = DbContext.Current.CreateConnection())
			{
				var sql = $@"SELECT *, COUNT(*) OVER() AS ___total_count___ FROM system_log WHERE (source  ILIKE  @querySource  OR @querySource is null)  AND (message  ILIKE  @queryMessage OR @queryMessage is null ) ORDER BY created_on DESC LIMIT {limit} OFFSET {offset} ";

				var cmd = connection.CreateCommand(sql);
				cmd.Parameters.Add(new NpgsqlParameter("@querySource", string.IsNullOrWhiteSpace(querySource) ? (object)DBNull.Value : $"%{querySource}%"));
				cmd.Parameters.Add(new NpgsqlParameter("@queryMessage", string.IsNullOrWhiteSpace(queryMessage) ? (object)DBNull.Value : $"%{queryMessage}%"));

				DataTable dt = new DataTable();
				new NpgsqlDataAdapter(cmd).Fill(dt);

				EntityRecordList result = new EntityRecordList();
				foreach (DataRow dr in dt.Rows)
				{
					result.TotalCount = (int)((long)dr["___total_count___"]);
					EntityRecord record = new EntityRecord();
					record["id"] = dr["id"];
					record["created_on"] = dr["created_on"];
					record["type"] = dr["type"];
					record["notification_status"] = dr["notification_status"];
					record["source"] = dr["source"];
					record["message"] = dr["message"];
					if (dr["details"] == DBNull.Value)
						record["details"] = null;
					else
						record["details"] = dr["details"];

					result.Add(record);
				}
				return result;

			}
		}

		public void Create(LogType type, string source, string message, string details, LogNotificationStatus notificationStatus = LogNotificationStatus.NotNotified, bool saveDetailsAsJson = false)
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				var cmd = connection.CreateCommand("INSERT INTO system_log(id,created_on,type,message,source,details,notification_status) VALUES(@id,@created_on,@type,@message,@source,@details,@notification_status);");
				cmd.Parameters.Add(new NpgsqlParameter("@id", Guid.NewGuid()));
				cmd.Parameters.Add(new NpgsqlParameter("@type", ((int)type)));
				cmd.Parameters.Add(new NpgsqlParameter("@source", source ?? string.Empty));
				cmd.Parameters.Add(new NpgsqlParameter("@message", message ?? string.Empty));
				cmd.Parameters.Add(new NpgsqlParameter("@notification_status", ((int)notificationStatus)));
				cmd.Parameters.Add(new NpgsqlParameter("@details", details ?? string.Empty));
				cmd.Parameters.Add(new NpgsqlParameter("@created_on", DateTime.UtcNow));
				cmd.ExecuteNonQuery();
			}
		}

		public void Create(LogType type, string source, Exception ex, HttpRequest request = null, LogNotificationStatus notificationStatus = LogNotificationStatus.NotNotified)
		{
			string details = MakeDetailsJson("", ex, request);
			Create(type, source, ex?.Message, details, notificationStatus);
		}

		public void Create(LogType type, string source, string message, Exception ex, HttpRequest request = null, LogNotificationStatus notificationStatus = LogNotificationStatus.NotNotified)
		{
			string details = MakeDetailsJson("", ex, request);
			Create(type, source, message, details, notificationStatus);
		}

		public static string MakeDetailsJson(string details, Exception ex = null, HttpRequest request = null)
		{
			if (string.IsNullOrWhiteSpace(details) && ex == null && request == null)
				return null;

			EntityRecord eRecord = new EntityRecord();
			eRecord["message"] = details;
			eRecord["stack_trace"] = null;
			eRecord["source"] = null;
			eRecord["inner_exception"] = null;
			eRecord["request_url"] = null;

			if (ex != null)
			{
				eRecord["message"] = details + ex.Message;
				eRecord["stack_trace"] = ex.StackTrace;
				eRecord["source"] = ex.Source;
				eRecord["inner_exception"] = null;
				eRecord["request_url"] = null;

				if (ex.InnerException != null)
				{
					EntityRecord ieRecord = new EntityRecord();
					ieRecord["message"] = ex.InnerException.Message;
					ieRecord["stack_trace"] = ex.InnerException.StackTrace;
					eRecord["inner_exception"] = ieRecord;
				}
			}

			if (request != null)
				eRecord["request_url"] = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";

			return JsonConvert.SerializeObject(eRecord);
		}
	}

	public enum LogType
	{
		Error = 1,
		Info = 2
	}

	public enum LogNotificationStatus
	{
		DoNotNotify = 1,
		NotNotified = 2,
		Notified = 3,
		NotificationFailed = 4
	}
}
