using System;

namespace WebVella.ERP.Notifications
{
    public interface INotificationService
	{
		void Initialize(IServiceProvider serviceProvider);

		void SendNotification(Notification notification);
	}
}