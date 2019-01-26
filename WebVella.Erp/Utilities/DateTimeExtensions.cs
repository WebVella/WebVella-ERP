using System.Collections.Generic;
using System.Text;
using WebVella.Erp;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Utilities;

namespace System
{
	public static class DateTimeExtensions
	{

        public static DateTime? ConvertToAppDate(this DateTime? utcDate)
        {
			if (utcDate == null)
				return null;

			TimeZoneInfo appTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ErpSettings.TimeZoneName);
			return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcDate.Value, appTimeZone.Id);
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
