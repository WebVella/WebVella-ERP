using Microsoft.Extensions.Logging;
using System;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Diagnostics
{
	public class Log
	{
		public void Create(LogType type, string source, string message, string details, LogNotificationStatus notificationStatus = LogNotificationStatus.NotNotified)
		{
			EntityRecord logRecord = new EntityRecord();
			logRecord["id"] = Guid.NewGuid();
			logRecord["type"] = (int)type;
			logRecord["source"] = source;
			logRecord["message"] = message;
			logRecord["notification_status"] = (int)notificationStatus;
			logRecord["details"] = details;

			RecordManager recMan = new RecordManager();
			recMan.CreateRecord("system_log", logRecord);
		}

		public void UpdateNotificationStatus(Guid id, LogNotificationStatus notificationStatus)
		{
			RecordManager recMan = new RecordManager();

			var response = recMan.Find(new EntityQuery("system_log", "*", EntityQuery.QueryEQ("id", id)));

			if (!response.Success || response.Object == null || response.Object.Data == null || response.Object.Data.Count == 0)
				return; //Maybe it have to throw exception here

			EntityRecord logRecord = response.Object.Data[0];
			logRecord["notification_status"] = (int)notificationStatus;

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
