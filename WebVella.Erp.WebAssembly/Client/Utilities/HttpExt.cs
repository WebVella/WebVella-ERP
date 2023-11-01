using System.Net.Http.Json;
using System.Text.Json;

namespace WebVella.Erp.WebAssembly.Utilities;

public static class HttpExt
{
	public static async Task<HttpResponseMessage> VerifyAsync(this HttpResponseMessage response)
	{
		if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
		{
			ApiErrorModel err = await response.Content.ReadFromJsonAsync<ApiErrorModel>();
			if (err is not null)
			{
				switch (err.Type)
				{
					case ApiErrorType.ValidationException:
						{
							var ex = new ValidationException(err.Message);
							ex.SetStackTrace(err.StackTrace);
							ex.AddData(err.ValidationData);
							throw ex;
						}
					default:
						throw new Exception($"Not supported ApiErrorType {err.Type} with response code {response.StatusCode}");
				}
			}
			else
				throw new Exception($"Unspecified api response error recieved");
		}

		if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
		{
			ApiErrorModel err = await response.Content.ReadFromJsonAsync<ApiErrorModel>();
			if (err is not null)
			{
				switch (err.Type)
				{
					case ApiErrorType.Exception:
						{
							var ex = new ApiException(err.Message);
							ex.SetStackTrace(err.StackTrace);
							throw ex;
						}
					default:
						throw new Exception($"Not supported ApiErrorType {err.Type} with response code {response.StatusCode}");
				}
			}
			else
				throw new Exception($"Unspecified api response error recieved");
		}

		if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
			throw new Exception("API Request Call URL not found");
		return response;
	}


	public static async Task<RT> PostAndReadAsJsonAsync<PT, RT>(this HttpClient httpClient, string apiUrl, PT postObject)
	{
		try
		{
			var response = await httpClient.PostAsJsonAsync<PT>(apiUrl, postObject);
			await response.VerifyAsync();
			var prefetch = await response.Content.ReadAsStringAsync();
			if (String.IsNullOrWhiteSpace(prefetch))
				return default(RT);

			return await response.Content.ReadFromJsonAsync<RT>();
		}
		catch (HttpRequestException httpException)
		{
			//this exception is thrown when no connection is established or connection get disconnected
			throw new ApiConnectionException("Няма връзка със сървър приложението", httpException);
		}
		catch (TaskCanceledException taskCanceledException)
		{
			//this exception is thrown when connection timeouts
			throw new ApiConnectionException("Връзката със сървър приложението се разпадна", taskCanceledException);
		}
		catch (Exception ex)
		{
			throw;
		}
	}

	public static async Task<string> PostAndReadAsStringAsync<PT>(this HttpClient httpClient, string apiUrl, PT postObject)
	{
		try
		{
			var response = await httpClient.PostAsJsonAsync<PT>(apiUrl, postObject);
			await response.VerifyAsync();
			return await response.Content.ReadAsStringAsync();
		}
		catch (HttpRequestException httpException)
		{
			//this exception is thrown when no connection is established or connection get disconnected
			throw new ApiConnectionException("Няма връзка със сървър приложението", httpException);
		}
		catch (TaskCanceledException taskCanceledException)
		{
			//this exception is thrown when connection timeouts
			throw new ApiConnectionException("Връзката със сървър приложението се разпадна", taskCanceledException);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static async Task<RT> GetAndReadAsJsonAsync<RT>(this HttpClient httpClient, string apiUrl) where RT : class
	{
		try
		{
			var response = await httpClient.GetAsync(apiUrl);
			await response.VerifyAsync();
			try
			{
				return await response.Content.ReadFromJsonAsync<RT>();
			}
			catch
			{
				var stringData = await response.Content.ReadAsStringAsync();
				if (string.IsNullOrWhiteSpace(stringData))
					return null;
				return JsonSerializer.Deserialize<RT>(stringData);
			}
		}
		catch (HttpRequestException httpException)
		{
			//this exception is thrown when no connection is established or connection get disconnected
			throw new ApiConnectionException("Няма връзка със сървър приложението", httpException);
		}
		catch (TaskCanceledException taskCanceledException)
		{
			//this exception is thrown when connection timeouts
			throw new ApiConnectionException("Връзката със сървър приложението се разпадна", taskCanceledException);
		}
		catch (Exception)
		{
			throw;
		}
	}


	public static async Task<int?> PostAndReadAsIntAsync<PT>(this HttpClient httpClient, string apiUrl, PT postObject)
	{
		try
		{
			var response = await httpClient.PostAsJsonAsync<PT>(apiUrl, postObject);
			await response.VerifyAsync();
			var content = await response.Content.ReadAsStringAsync();
			if (String.IsNullOrWhiteSpace(content))
				return null;
			if (int.TryParse(content, out int outInt))
				return outInt;

			throw new Exception("Api Request Response is not integer as expected");
		}
		catch (HttpRequestException httpException)
		{
			//this exception is thrown when no connection is established or connection get disconnected
			throw new ApiConnectionException("Няма връзка със сървър приложението", httpException);
		}
		catch (TaskCanceledException taskCanceledException)
		{
			//this exception is thrown when connection timeouts
			throw new ApiConnectionException("Връзката със сървър приложението се разпадна", taskCanceledException);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static async Task<int?> GetAndReadAsIntAsync(this HttpClient httpClient, string apiUrl)
	{
		try
		{
			var response = await httpClient.GetAsync(apiUrl);
			await response.VerifyAsync();
			var content = await response.Content.ReadAsStringAsync();
			if (String.IsNullOrWhiteSpace(content))
				return null;
			if (int.TryParse(content, out int outInt))
				return outInt;

			throw new Exception("Api Request Response is not integer as expected");

		}
		catch (HttpRequestException httpException)
		{
			//this exception is thrown when no connection is established or connection get disconnected
			throw new ApiConnectionException("Няма връзка със сървър приложението", httpException);
		}
		catch (TaskCanceledException taskCanceledException)
		{
			//this exception is thrown when connection timeouts
			throw new ApiConnectionException("Връзката със сървър приложението се разпадна", taskCanceledException);
		}
		catch (Exception)
		{
			throw;
		}
	}
}
