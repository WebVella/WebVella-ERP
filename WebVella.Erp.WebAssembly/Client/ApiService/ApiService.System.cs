using Blazored.LocalStorage;

namespace WebVella.Erp.WebAssembly.ApiService;

public partial interface IApiService
{
    ValueTask<HttpClient> GetAuthorizedHttpClientAsync();
    HttpClient GetNotAuthorizedHttpClientAsync();
}

public partial class ApiService : IApiService
{
    public async ValueTask<HttpClient> GetAuthorizedHttpClientAsync()
    {
        string token = await _tokenManagerService.GetTokenAsync();
        if (String.IsNullOrWhiteSpace(token))
        {
            //naavigate to root for login
            _navManager.NavigateTo("/login");
            return null;
            //throw new ApiTokenException("Token not found");
        }

        //Get them with the token
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        return _httpClient;
    }

    public HttpClient GetNotAuthorizedHttpClientAsync()
    {
        return _httpClient;

    }
}