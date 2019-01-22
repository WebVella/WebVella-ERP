using System;
using System.Globalization;

namespace WebVella.Erp.Web.Services
{
	public class LocalizationService : BaseService
	{
		public DateTime? ConvertAppDateToUtc(DateTime? appDate)
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
		
		public DateTime? ConvertUtcToAppDate(DateTime? appDate)
		{
			if (appDate == null)
				return null;

			TimeZoneInfo appTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ErpSettings.TimeZoneName);
			return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(appDate.Value, appTimeZone.Id);
		}

		public CultureInfo GetAppCultureInfo()
		{
			return new CultureInfo(ErpSettings.Locale);
		}
	}
}
