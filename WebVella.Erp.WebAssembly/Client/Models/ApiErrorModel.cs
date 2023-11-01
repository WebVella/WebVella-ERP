namespace WebVella.Erp.WebAssembly.Models;
public enum ApiErrorType
{
	ValidationException = 0,
	Exception = 1
}

public class ApiErrorModel
{
	public ApiErrorType Type { get; set; }

	public string Message { get; set; }

	public string StackTrace { get; set; }

	public Dictionary<string, List<string>> ValidationData { get; set; }
}

