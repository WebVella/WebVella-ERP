using Microsoft.VisualBasic;
using System.Globalization;

namespace WebVella.Erp.WebAssembly.Utilities;

public static class DateOnlyUtils
{
    public static string ToString(DateOnly? date) => date?.ToString(WasmConstants.DateFormatUrl);
    public static string ForView(DateOnly? date) => date?.ToString(WasmConstants.DateFormat, WasmConstants.Culture);
    public static string FromStringForView(string dateString)
    {
        var date = FromString(dateString);
        return ForView(date);
    }
    public static DateOnly? FromString(string dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
            return null;

        if (DateOnly.TryParseExact(dateString, WasmConstants.DateFormatUrl, out DateOnly outDate))
            return outDate;

        throw new Exception($"Date only string \"{dateString}\" not in proper format: {WasmConstants.DateFormatUrl}");
    }

    public static string ToUrlString(DateOnly? date)
    {
        if (date is null)
            return null;
        return date.Value.ToString(WasmConstants.DateFormatUrl, CultureInfo.InvariantCulture);
    }

    public static DateOnly? FromUrlString(string dateString)
    {
        if (String.IsNullOrWhiteSpace(dateString))
            return null;

        DateOnly? result = null;
        if (DateOnly.TryParseExact(dateString, WasmConstants.DateFormatUrl, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeLocal, out DateOnly outDate))
        {
            result = outDate;
        }
        if (result is null)
            throw new Exception("date string not in proper format");

        return result;
    }
}
