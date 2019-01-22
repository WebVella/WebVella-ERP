using System;

namespace WebVella.Erp.Notifications
{
	public class ErpRecordChangeNotification
	{
		public Guid EntityId { get; set; }
		public string EntityName { get; set; }
		public Guid RecordId { get; set; }
	}
}
