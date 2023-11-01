namespace WebVella.Erp.WebAssembly.Exceptions;

public class ApiException : BaseException
{
	public ApiException() : base() { }

	public ApiException(string message) : base(message) { }

	public ApiException(string message, Exception innerException) : base(message, innerException) { }

	public ApiException(string message, System.Collections.IDictionary data) : base(message, data) { }

	public ApiException(string message, Dictionary<string, List<string>> data) : base(message, data) { }
}