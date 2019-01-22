using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Erp.Notifications
{
	public class NotificationHandlerAttribute : Attribute
	{
		public string Channel { get; set; }

		public NotificationHandlerAttribute(string channel = null)
		{
			Channel = channel;
		}
	}
}
