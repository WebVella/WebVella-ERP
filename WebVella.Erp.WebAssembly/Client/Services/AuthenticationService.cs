using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace WebVella.Erp.WebAssembly.Services;

public interface IAuthenticationService
{
	ValueTask<bool> HasTokenAsync();
	ValueTask<bool> LoginAsync(LoginModel model);
	ValueTask<bool> LogoutAsync();
}

public class AuthenticationService : IAuthenticationService
{
	private const string apiAuthRoot = "v3/en_US/auth/jwt/";

	private readonly HttpClient _httpClient;
	private readonly ITokenManagerService _tokenManagerService;
	private readonly AuthenticationStateProvider _customAuthenticationProvider;
	private readonly ILocalStorageService _localStorageService;
	
	public AuthenticationService(
		 HttpClient httpClient,
		ITokenManagerService tokenManagerService,
		ILocalStorageService localStorageService,
		AuthenticationStateProvider customAuthenticationProvider)
	{
		_httpClient = httpClient;
		_tokenManagerService = tokenManagerService;
		_localStorageService = localStorageService;
		_customAuthenticationProvider = customAuthenticationProvider;
	}

	public async ValueTask<bool> HasTokenAsync()
	{
		string token = await _tokenManagerService.GetTokenAsync();
		if (String.IsNullOrWhiteSpace(token))
			return false;

		return true;
	}

	public async ValueTask<bool> LoginAsync(LoginModel model)
	{
		AuthResponse authData = await _httpClient.PostAndReadAsJsonAsync<LoginModel, AuthResponse>($"{apiAuthRoot}token", model);
		string token = authData.Object?.ToString();
		if (string.IsNullOrWhiteSpace(token))
			return false;
		await _localStorageService.SetItemAsync("token", token);
		(_customAuthenticationProvider as CustomAuthenticationProvider).Notify();
		return true;
	}

	public async ValueTask<bool> LogoutAsync()
	{
		await _localStorageService.RemoveItemAsync($"token");
		(_customAuthenticationProvider as CustomAuthenticationProvider).Notify();
		return true;
	}
}