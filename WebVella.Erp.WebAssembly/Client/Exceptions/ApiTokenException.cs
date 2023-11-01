namespace WebVella.Erp.WebAssembly.Exceptions;

public class ApiTokenException : BaseException
{
	public ApiTokenException() : base() { }

	public ApiTokenException(string message) : base(message) { }

	public ApiTokenException(string message, Exception innerException) : base(message, innerException) { }

}
