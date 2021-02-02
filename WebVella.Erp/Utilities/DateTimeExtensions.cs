using WebVella.Erp;

namespace System
{
	public static class DateTimeExtensions
	{

		public static DateTime ClearKind(this DateTime datetime)
		{
			return ((DateTime?)datetime).ClearKind().Value;
		}

		public static DateTime? ClearKind(this DateTime? datetime)
		{
			if (datetime == null)
				return null;

			return new DateTime(datetime.Value.Ticks, DateTimeKind.Unspecified);
		}

		public static DateTime ConvertToAppDate(this DateTime datetime)
		{
			return ((DateTime?)datetime).ConvertToAppDate().Value;
		}

		public static DateTime? ConvertToAppDate(this DateTime? datetime )
        {
			if (datetime == null)
				return null;

			//If unspecified assume it is already in app TZ
			if(datetime.Value.Kind == DateTimeKind.Unspecified)
				return datetime;

			TimeZoneInfo appTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ErpSettings.TimeZoneName);
			return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(datetime.Value, appTimeZone.Id);
		}

		public static DateTime ConvertAppDateToUtc(this DateTime datetime)
		{
			return ((DateTime?)datetime).ConvertAppDateToUtc().Value;
		}

		public static DateTime? ConvertAppDateToUtc(this DateTime? appDate)
		{
			if (appDate == null)
				return null;
			TimeZoneInfo appTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ErpSettings.TimeZoneName);

			DateTime tmpDT = appDate.Value;
			if (tmpDT.Kind == DateTimeKind.Utc)
				return tmpDT;
			else if (tmpDT.Kind == DateTimeKind.Local && appTimeZone != TimeZoneInfo.Local)
			{
				var convertedToAppZoneDate = TimeZoneInfo.ConvertTime(tmpDT, appTimeZone);
				return TimeZoneInfo.ConvertTimeToUtc(convertedToAppZoneDate, appTimeZone);
			}

			return TimeZoneInfo.ConvertTimeToUtc(appDate.Value, appTimeZone);
		}
	}
}
