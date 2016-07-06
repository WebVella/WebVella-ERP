using WebVella.ERP.Notifications;

namespace WebVella.ERP.SamplePlugin
{
    public class NotificationHandler
    {
		[NotificationHandler(channel:"*")]
		public void HandleNotification(Notification notification)
		{
		}
    }
}
