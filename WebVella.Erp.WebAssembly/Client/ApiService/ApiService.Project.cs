using Blazored.LocalStorage;

namespace WebVella.Erp.WebAssembly.ApiService;

public partial interface IApiService
{
    ValueTask<WvUser> GetCurrentUserAsync();
}

public partial class ApiService : IApiService
{
    public async ValueTask<WvUser> GetCurrentUserAsync()
    {
        var httpClient = await GetAuthorizedHttpClientAsync();

        //return await httpClient.GetAndReadAsJsonAsync<WvUser>($"{apiProjectRoot}user/get-current");
        return await httpClient.PostAndReadAsJsonAsync<object,WvUser>($"/api/v3/en_US/eql",null);
    }

}