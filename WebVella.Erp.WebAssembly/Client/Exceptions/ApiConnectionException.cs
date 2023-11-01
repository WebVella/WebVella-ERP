namespace WebVella.Erp.WebAssembly.Exceptions;

public class ApiConnectionException : BaseException
{
	public ApiConnectionException() : base() { }

	public ApiConnectionException(string message) : base(message) { }

	public ApiConnectionException(string message, Exception innerException) : base(message, innerException) { }

}