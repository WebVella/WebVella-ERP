using System.Collections.Generic;
using System.Text;
using WebVella.Erp;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Utilities;

namespace System
{
	public static class DateTimeExtensions
	{
		public static DateTime GetErpDate(this DateTime dateTime)
		{
           var erpTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ErpSettings.TimeZoneName);
            
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, erpTimeZone);
        }

        public static DateTime GetErpDateTime(this DateTime dateTime)
        {
            var erpTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ErpSettings.TimeZoneName);

            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, erpTimeZone);
        }
    }
}
