using System.Collections;

namespace WebVella.Erp.WebAssembly.Exceptions;


public class BaseException : Exception
{
	private string oldStackTrace;
	public override string StackTrace { get { return this.oldStackTrace; } }

	public BaseException() : base() { }

	public BaseException(string message) : base(message) { }

	public BaseException(string message, Exception innerException) : base(message, innerException)
	{
		if (innerException != null && innerException.Data.Count > 0)
			AddData(innerException.Data);
	}

	public BaseException(string message, System.Collections.IDictionary dictionary) : base(message)
	{
		AddData(dictionary);
	}

	public BaseException(string message, Dictionary<string, List<string>> dictionary) : base(message)
	{
		AddData(dictionary);
	}

	public void UpsertDataList(string key, string value)
	{
		if (!Data.Contains(key))
			Data.Add(key, new List<string>());

		(Data[key] as List<string>)?.Add(value);
	}

	public void ThrowIfContainsErrors()
	{
		if (Data.Count > 0)
			throw this;
	}

	public void AddData(IDictionary dictionary)
	{
		if (dictionary != null)
		{
			foreach (DictionaryEntry item in dictionary)
				Data.Add(item.Key, item.Value);
		}
	}

	public void AddData(Dictionary<string, List<string>> dictionary)
	{
		if (dictionary != null)
		{
			foreach (var item in dictionary)
				Data.Add(item.Key, item.Value);
		}
	}

	public void SetStackTrace(string stackTrace)
	{
		oldStackTrace = stackTrace;
	}

	// public void AddData(string key, params string[] values) => Data.Add(key, values);

	public IDictionary<string, List<string>> GetDataAsUsableDictionary()
	{
		IDictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
		foreach (var key in Data.Keys)
		{
			dict.Add(key.ToString(), (Data[key] as List<string>) ?? new List<string>());
		}
		return dict;
	}
}

