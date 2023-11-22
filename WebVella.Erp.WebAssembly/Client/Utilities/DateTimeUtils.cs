using Microsoft.VisualBasic;
using System.Globalization;

namespace WebVella.Erp.WebAssembly.Utilities;

public static class DateTimeUtils
{
    public static string ToUrlString(DateTime? date)
    {
        if (date is null)
            return null;
        return date.Value.ToString(WasmConstants.DateFormatUrl, CultureInfo.InvariantCulture);
    }

    public static DateTime? FromUrlString(string dateString)
    {
        if (String.IsNullOrWhiteSpace(dateString))
            return null;

        DateTime? result = null;
        if (DateTime.TryParseExact(dateString, WasmConstants.DateFormatUrl, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeLocal, out DateTime outDate))
        {
            result = outDate;
        }
        if(result is null)
            throw new Exception("date string not in proper format");

        return result;
    }

}
