using Microsoft.AspNetCore.Http;
using System;
using WebVella.Erp.Diagnostics;

namespace WebVella.Erp.Web.Services
{
    public class LogService : BaseService
	{
        /// <summary>
        /// Creates log of specified type and send email notification.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="details">The details.</param>
        /// <param name="notificationStatus">The notification status.</param>
        /// <param name="saveDetailsAsJson">if set to <c>true</c> [save details as json].</param>
        public void Create(LogType type, string source, string message, string details, LogNotificationStatus notificationStatus = LogNotificationStatus.NotNotified, bool saveDetailsAsJson = false)
        {
            if (notificationStatus == LogNotificationStatus.NotNotified)
            {
                bool messageSent = new MailService().SendLogMessage(type, source, message, details);

                if (messageSent)
                    notificationStatus = LogNotificationStatus.Notified;
            }

            Log log = new Log();
            log.Create(type, source, message, details, notificationStatus, saveDetailsAsJson);
           
        }

        /// <summary>
        ///  Creates log of specified type and send email notification.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="source">The source.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="request">The request.</param>
        /// <param name="notificationStatus">The notification status.</param>
        public void Create(LogType type, string source, Exception ex, HttpRequest request = null, LogNotificationStatus notificationStatus = LogNotificationStatus.NotNotified)
        {  
            if (notificationStatus == LogNotificationStatus.NotNotified)
            {
                string details = Log.MakeDetailsJson("", ex, request);
                bool messageSent = new MailService().SendLogMessage(type, source, ex.Message, details, request == null ? null: request.Host.ToString());
                if (messageSent)
                    notificationStatus = LogNotificationStatus.Notified;
            }

            Log log = new Log();
            log.Create(type, source, ex, request, notificationStatus);
        }

        /// <summary>
        ///  Creates log of specified type and send email notification.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="request">The request.</param>
        /// <param name="notificationStatus">The notification status.</param>
        public void Create(LogType type, string source, string message, Exception ex, HttpRequest request = null, LogNotificationStatus notificationStatus = LogNotificationStatus.NotNotified)
        {
            if (notificationStatus == LogNotificationStatus.NotNotified)
            {
                string details = Log.MakeDetailsJson("", ex, request);
                bool messageSent = new MailService().SendLogMessage(type, source, message, details, request == null ? null : request.Host.ToString());
                if (messageSent)
                    notificationStatus = LogNotificationStatus.Notified;
            }

            Log log = new Log();
            log.Create(type, source, message, ex, request, notificationStatus);
        }
    }
}
