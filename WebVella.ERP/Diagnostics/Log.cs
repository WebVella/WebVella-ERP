using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Diagnostics
{
	public class Log
	{
		public void Create(LogType type, string source, string message, string details, LogNotificationStatus notificationStatus = LogNotificationStatus.NotNotified, bool saveDetailsAsJson = false)
		{
			EntityRecord logRecord = new EntityRecord();
			logRecord["id"] = Guid.NewGuid();
			logRecord["type"] = ((int)type).ToString();
			logRecord["source"] = source;
			logRecord["message"] = message;
			logRecord["notification_status"] = ((int)notificationStatus).ToString();
			logRecord["details"] = saveDetailsAsJson ? MakeDetailsJson(details) : details;
			logRecord["created_by"] = SystemIds.SystemUserId;
			logRecord["last_modified_by"] = SystemIds.SystemUserId;
			logRecord["created_on"] = DateTime.UtcNow;
			logRecord["last_modified_on"] = DateTime.UtcNow;

			RecordManager recMan = new RecordManager(true);
			var response = recMan.CreateRecord("system_log", logRecord);
		}

		public void Create(LogType type, string source, Exception ex, HttpRequest request = null, LogNotificationStatus notificationStatus = LogNotificationStatus.NotNotified)
		{
			string details = MakeDetailsJson("", ex, request);
			Create(LogType.Error, source, ex.Message, details, notificationStatus);
		}

		public void Create(LogType type, string source, string message, Exception ex, HttpRequest request = null, LogNotificationStatus notificationStatus = LogNotificationStatus.NotNotified)
		{
			string details = MakeDetailsJson("", ex, request);
			Create(LogType.Error, source, message, details, notificationStatus);
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

		public void UpdateNotificationStatus(Guid id, LogNotificationStatus notificationStatus)
		{
			RecordManager recMan = new RecordManager(true);

			var response = recMan.Find(new EntityQuery("system_log", "*", EntityQuery.QueryEQ("id", id)));

			if (!response.Success || response.Object == null || response.Object.Data == null || response.Object.Data.Count == 0)
				return; //Maybe it have to throw exception here

			EntityRecord logRecord = response.Object.Data[0];
			logRecord["notification_status"] = ((int)notificationStatus).ToString();
			logRecord["last_modified_on"] = DateTime.UtcNow;

			recMan.UpdateRecord("system_log", logRecord);
		}

		public void SendEmails()
		{
			RecordManager recMan = new RecordManager();

			var response = recMan.Find(new EntityQuery("system_log", "*", EntityQuery.QueryEQ("notification_status", (int)LogNotificationStatus.NotNotified)));

			if (!response.Success || response.Object == null || response.Object.Data == null || response.Object.Data.Count == 0)
				return;

			foreach (EntityRecord log in response.Object.Data)
			{
				Guid id = (Guid)log["id"];
				LogNotificationStatus notificationStatus = LogNotificationStatus.Notified;

				//TODO: SendEmail method have to be implemented
				//try
				//{
				//	SendEmail(string toEmailAddress, string subject, string body)
				//}
				//catch (Exception)
				//{
				//	notificationStatus = LogNotificationStatus.NotificationFailed;
				//}

				UpdateNotificationStatus(id, notificationStatus);
			}
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
