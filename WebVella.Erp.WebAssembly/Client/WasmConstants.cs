namespace WebVella.Erp.WebAssembly;

public class WasmConstants
{
    public static CultureInfo Culture = new CultureInfo("bg-BG");
    public static CultureInfo NumberCulture = new CultureInfo("en-US");
    public const string DateFormat = "dd.MM.yyyy";
    public const string HourFormat = "HH:mm";
    public const string DateFormatUrl = "yyyy-MM-dd";
    public const string YearMonthFormatUrl = "yyyy-MM";
    public const string DateHourFormat = "dd MMM yyyy HH:mm";
    public const string DateTimeFormat = "dd MMM yyyy HH:mm:ss";
    public const string NumberFormat = "G0";

    //Query params
    public const string ReturnUrlQuery = "returnUrl";
}
